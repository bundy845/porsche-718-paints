"""Read-only PSD v1 inventory and selected original raster references.

Uses the project's existing standard-library PSD/PackBits approach. Does not
reconstruct or write PSDs. Layer masks/effects remain recorded, not rendered.
"""
import json, struct, zlib
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
SOURCE=ROOT.parents[3]/'cel-shaded/inputs/Porsche 718 GT4.psd'
data=SOURCE.read_bytes()
def u16(p):return struct.unpack_from('>H',data,p)[0]
def u32(p):return struct.unpack_from('>I',data,p)[0]
assert data[:6]==b'8BPS\x00\x01'
channels=u16(12); h,w,depth,mode=struct.unpack_from('>IIHH',data,14)
assert (w,h,depth,mode)==(2048,2048,8,3)
pos=26; pos+=4+u32(pos)
resource_size=u32(pos);resource_end=pos+4+resource_size;pos+=4
resources=[]
while pos<resource_end:
    assert data[pos:pos+4]==b'8BIM'
    rid=u16(pos+4);pos+=6
    length=data[pos];pos+=(length+2)&~1
    size=u32(pos);pos+=4
    resources.append({'id':rid,'length':size})
    pos+=size+(size&1)
pos=resource_end
pos+=4  # layer/mask length
layer_length=u32(pos);pos+=4
count=abs(struct.unpack_from('>h',data,pos)[0]);pos+=2
records=[]
for i in range(count):
    top,left,bottom,right=struct.unpack_from('>iiii',data,pos);pos+=16
    n=u16(pos);pos+=2;ch=[]
    for _ in range(n):
        cid,length=struct.unpack_from('>hI',data,pos);pos+=6;ch.append((cid,length))
    assert data[pos:pos+4]==b'8BIM'
    blend=data[pos+4:pos+8].decode('ascii');opacity,clip,flags=struct.unpack_from('BBB',data,pos+8);pos+=12
    size=u32(pos);pos+=4;end=pos+size
    mask_size=u32(pos);pos+=4+mask_size
    range_size=u32(pos);pos+=4+range_size
    length=data[pos];name=data[pos+1:pos+1+length].decode('macroman');pos+=(length+4)&~3
    tags=[];section=None
    while pos+12<=end:
        sig=data[pos:pos+4];key=data[pos+4:pos+8].decode('ascii',errors='replace');pos+=8
        if sig not in (b'8BIM',b'8B64'):break
        size=u32(pos);pos+=4;payload=data[pos:pos+size]
        tags.append(key)
        if key=='luni':name=payload[4:4+struct.unpack_from('>I',payload)[0]*2].decode('utf-16-be')
        if key in ('lsct','lsdk'):section=struct.unpack_from('>I',payload)[0]
        pos+=size+(size&1)
    pos=end
    records.append(dict(index=i,name=name,bounds=[left,top,right,bottom],channels=ch,
                        blend=blend,opacity=opacity,clipping=clip,visible=not bool(flags&2),
                        layer_mask_bytes=mask_size,section_type=section,additional_tags=tags,
                        text=('TySh' in tags),smart_object=bool(set(tags)&{'SoLd','SoLE','PlLd','plLd'})))

def decode(block,width,height):
    method=struct.unpack_from('>H',block)[0]
    if method==0:return block[2:]
    if method==2:return zlib.decompress(block[2:])
    if method!=1:raise ValueError(f'Unsupported compression {method}')
    sizes=struct.unpack_from('>'+str(height)+'H',block,2);at=2+height*2;out=bytearray()
    for size in sizes:
        row=block[at:at+size];at+=size;j=0;result=bytearray()
        while j<len(row):
            n=struct.unpack_from('b',row,j)[0];j+=1
            if n>=0:result+=row[j:j+n+1];j+=n+1
            elif n!=-128:result+=row[j:j+1]*(1-n);j+=1
        assert len(result)==width
        out+=result
    return out
def chunk(kind,value):return struct.pack('>I',len(value))+kind+value+struct.pack('>I',zlib.crc32(kind+value)&0xffffffff)
selected={'Pitbox':'original-pitbox-raster.png', 'Car Mandatory':'original-mandatory-raster.png', 'Mask':'original-mask-raster.png'}
for rec in records:
    left,top,right,bottom=rec['bounds'];width=right-left;height=bottom-top;decoded={}
    for cid,length in rec['channels']:
        if rec['name'] in selected and cid in (0,1,2,-1):decoded[cid]=decode(data[pos:pos+length],width,height)
        pos+=length
    if rec['name'] not in selected or not decoded:continue
    rgba=bytearray(w*h*4)
    for cid,component in ((0,0),(1,1),(2,2),(-1,3)):
        values=decoded.get(cid,bytes([255])*(width*height))
        for y in range(height):
            if not 0<=top+y<h:continue
            for x in range(width):
                if 0<=left+x<w:rgba[((top+y)*w+left+x)*4+component]=values[y*width+x]
    rows=b''.join(b'\0'+rgba[y*w*4:(y+1)*w*4] for y in range(h))
    png=b'\x89PNG\r\n\x1a\n'+chunk(b'IHDR',struct.pack('>IIBBBBB',w,h,8,6,0,0,0))+chunk(b'IDAT',zlib.compress(rows,9))+chunk(b'IEND',b'')
    (ROOT/'template'/selected[rec['name']]).write_bytes(png)
    rec['raster_reference']='template/'+selected[rec['name']]
report=dict(source=str(SOURCE),canvas=[w,h],depth=depth,mode=mode,channels=channels,
            image_resources=resources,layers=records,
            limitations='Read-only structural inventory. Raw selected layer channels, without compositing masks/effects. Original PSD remains authoritative and editable.')
(ROOT/'template/inventory.json').write_text(json.dumps(report,indent=2)+'\n',encoding='utf-8')
print('\n'.join(f'{r["index"]}: {r["name"]} {r["bounds"]}' for r in records))
