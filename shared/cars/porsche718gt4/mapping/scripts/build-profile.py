"""Build the auditable index from measured reference components and reviewed evidence.

Python standard library only. Does not edit PSDs or installed paint. Keep semantic
findings here; segmentation coordinates come from build-atlas.ps1.
"""
import csv
import hashlib
import json
import struct
import zipfile
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
REPO = ROOT.parents[3]

def digest(path):
    return hashlib.file_digest(path.open('rb'), 'sha256').hexdigest()

def save(name, data):
    (ROOT / name).write_text(json.dumps(data, indent=2, ensure_ascii=False) + '\n', encoding='utf-8')

# status applies to physical identity, not a production clipping boundary or seam.
# Orientation and geometry have separate confidence, so a named part cannot imply
# a verified wrap, logo transform, or an exact junction with its neighbor.
known = {
    4: ('Rear wing underside candidate', 'provisional', ['C02'], 'Unknown; confirm bottom face with d003 label 004.'),
    7: ('Rear bumper outer shell', 'confirmed', ['G04','G01','G03','C01','C02'], '+x travels from driver side toward passenger side around rear; +y goes down the rear face. Corners distort.'),
    21: ('Rear bumper center inset', 'confirmed', ['G04','C01','C02'], 'Cells 11,2 and 12,2 identify the lower center. +x toward passenger side; exact join to outer shell uncalibrated.'),
    30: ('Rear wing top', 'confirmed', ['G02','G05','C01','C02'], '+x runs from passenger/right toward driver/left across the wing. Chord direction and wrapped lip need d003.'),
    44: ('Driver/left wing outer endplate', 'confirmed', ['C01','C02','G01'], 'Clyde L labels the left outer face. Exact text-up direction is not measured.'),
    45: ('Passenger/right wing outer endplate', 'confirmed', ['C01','C02','G03'], 'Clyde R labels the right outer face. Exact text-up direction is not measured.'),
    63: ('Passenger/right wing inner endplate candidate', 'provisional', ['C02'], 'R label and paired shape; inner face needs a readable diagnostic view.'),
    64: ('Driver/left wing inner endplate candidate', 'provisional', ['C02'], 'L label and paired shape; inner face needs a readable diagnostic view.'),
    78: ('Passenger mirror shell candidate', 'provisional', ['G03','C01','C02'], 'Mirror uses cells near 15,4. This component is a candidate within that region; confirm 078 versus adjoining pieces.'),
    80: ('Passenger side intake outer surround', 'confirmed', ['G03','C01','C02'], 'Separate from the side body. Cell 6,5 is upside down; detailed rotation and tip correspondence pending.'),
    81: ('Passenger main body: fender, door, sill, pillars and rear quarter', 'confirmed', ['G03','G02','G06','C01','C02'], '+x front to rear; +y up the physical side. Rotate upright text 180 degrees relative to the driver side, never reflect lettering.'),
    116: ('Front bumper main skin', 'confirmed', ['G02','G01','G03','C01','C02'], '+y goes from passenger/right across nose toward driver/left. At the central face +x goes upward toward hood. Lamp joins pending.'),
    119: ('Front lower splitter', 'confirmed', ['G02','C01','C02'], 'Narrow left-edge strip. Cells 0,9 and 0,10 visible across nose; +y goes toward driver/left. Lip thickness and returns pending.'),
    120: ('Passenger door handle outer piece candidate', 'provisional', ['G03','C02'], 'Separate from handle recess on main body. Confirm label 120; do not place art here from the candidate name alone.'),
    122: ('Hood', 'confirmed', ['G02','G06','C01','C02','E07'], '+x nose to windshield; +y passenger/right to driver/left. Front-readable upright logo rotates 90 degrees clockwise in UV.'),
    125: ('Exterior windshield banner', 'confirmed', ['C01','C02','E02','E07','T01'], 'Runs across car along UV y. Readable front-view artwork uses top toward +x and reading direction +y. Visible crop limits remain unmeasured.'),
    126: ('Interior windshield banner', 'provisional', ['C02','T01'], 'INSIDE is printed on the guide. Cockpit projection, reading direction and visible crop need an interior view.'),
    128: ('Roof', 'confirmed', ['G02','G05','G06','C01','C02'], '+x windshield to rear glass; +y passenger/right to driver/left. Escape flap is a feature within this island.'),
    129: ('Rear deck and rear-glass painted surround', 'confirmed', ['G04','G05','G06','C01','C02'], '+x roof to tail; +y passenger/right to driver/left. The center is not body paint; atlas components sit inside its UV opening.'),
    131: ('Grille/radiator texture atlas inside rear-glass UV opening', 'unknown', ['T01','C02'], 'Template content identifies an atlas texture. Physical use or repeated instances unknown; not rear-glass paint.'),
    132: ('Porsche crest texture atlas inside rear-glass UV opening', 'unknown', ['T01','C02'], 'Template content identifies a crest atlas. Physical instances and mirroring unknown; not rear-glass paint.'),
    134: ('Rear badging atlas: PORSCHE text and upper lines', 'unknown', ['T01','C02'], 'Texture atlas; physical use of each line and shared texels must be checked.'),
    136: ('Driver main body: fender, door, sill, pillars and rear quarter', 'confirmed', ['G01','G02','G06','C01','C02'], '+x front to rear; +y down the physical side. Text upright in UV reads upright in the side view.'),
    137: ('Rear badging atlas: lower lines', 'unknown', ['T01','C02'], 'Texture atlas; not a body panel.'),
    139: ('Rear badging atlas: bottom line', 'unknown', ['T01','C02'], 'Texture atlas; not a body panel.'),
    143: ('Driver door handle outer piece candidate', 'provisional', ['G01','C02'], 'Separate from handle recess on main body. Confirm label 143.'),
    153: ('Driver side intake outer surround', 'confirmed', ['G01','C01','C02'], 'Separate from side body; cells near column 6, rows 14-15. Exact tip/edge correspondence pending.'),
    156: ('Driver mirror shell candidate', 'provisional', ['G01','G06','C01','C02'], 'Mirror uses lower-right cells. Confirm label 156 versus adjoining pieces before drawing on a shell face.'),
}

# New readable d003 views supplied during the full-car audit.
known.update({
    2: ('Shared vertical front louver surround ahead of both front wheels', 'confirmed', ['K01','K02'], 'Large 2 appears on both sides: shared source. Driver-side digit upright; passenger-side digit reversed/inverted. An asymmetric letter/axis check is needed to distinguish reflection from rotation exactly. Separate small fins have separate UV pieces.'),
    4: ('Rear wing underside and wrapped rear lip', 'confirmed', ['D07','D09','D10'], 'Underside uses 004, including the orange lip visible above. Arrows point toward driver/left across span when viewed from rear; exact top/bottom edge correspondence remains uncalibrated.'),
    9: ('Shared front brake-duct outer surround, both sides', 'confirmed', ['D02','D03'], 'The same 009 appears on both front openings. Driver-side digits upright; passenger-side digits rotated 180 degrees. +x points outward toward each front wheel. Independent left/right graphics are impossible on these shared texels.'),
    20: ('Shared front brake-duct inner return', 'confirmed', ['D02','D03'], '020 appears on the vertical inner return behind the 009 surround in both openings. Duct mesh stays separate. Complete interior extent and rotation need more sampling.'),
    21: ('Rear bumper center inset', 'confirmed', ['G04','D07','D08'], '021 reads upright from behind; +x goes toward passenger/right, +y down. The tow strap and circular tow opening remain separate.'),
    30: ('Rear wing top', 'confirmed', ['G02','G05','D06','D08'], '+x runs passenger/right to driver/left across wing; digits upside down from rear, so +y runs toward vehicle front on the visible upper face.'),
    40: ('Rear deck trailing lip, upper face', 'confirmed', ['D06','D08'], '040 is the orange horizontal ledge above the tail lamp/badge strip; distinct from the elevated rear wing. +x points toward driver/left; digits upside down when viewed from rear.'),
    41: ('Rear deck trailing lip, rear-facing face', 'confirmed', ['D07','D08','D09'], '041 is the narrow face just above PORSCHE. +x points toward driver/left; lettering upside down from rear. This is not the wing underside.'),
    44: ('Driver/left wing outer endplate', 'confirmed', ['D07','C02'], '044 upright from outside driver side; +x points toward rear, +y down.'),
    45: ('Passenger/right wing outer endplate', 'confirmed', ['D11','C02'], '045 upright from outside passenger side; +x points toward front, +y down. Do not copy driver text by reflection.'),
    63: ('Passenger/right wing inner endplate', 'confirmed', ['D07','D10'], '063 upright on inner face. Exact fore/aft arrow direction is not readable in these views.'),
    64: ('Driver/left wing inner endplate', 'confirmed', ['D09'], '064 upright on inner face. Exact fore/aft arrow direction is not readable in this view.'),
    78: ('Passenger mirror outer shell', 'confirmed', ['D01'], '078 on outer/top shell, locally upright from passenger side; arrows go toward vehicle front. Underside black insert and stalk are separate; far face not yet observed.'),
    80: ('Passenger side intake outer surround', 'confirmed', ['D01','G03'], '080 upright from passenger side; +x points rearward, +y down. This differs from the upside-down passenger main-body texture.'),
    120: ('Passenger door handle outer face', 'confirmed', ['D01'], '120 upright from passenger side. The handle recess is on main body; the physical handle face is this separate island. Text follows a curved horizontal face.'),
    134: ('PORSCHE rear lettering texture atlas', 'confirmed', ['D06','D07','D08','T01'], 'Rear letters pick up diagnostic pink from this atlas. Each glyph uses a small texture area: do not draw a body stripe through the atlas. Smaller badging lines remain unassigned.'),
    143: ('Driver door handle outer face', 'confirmed', ['D05'], '143 upside down from driver side; handle face separate from main-body recess. Use a 180-degree text rotation relative to the diagnostic.'),
    153: ('Driver side intake outer surround', 'confirmed', ['D05','G01'], '153 upright from driver side; +x points rearward, +y down. Separate from side body.'),
    156: ('Driver mirror outer shell', 'confirmed', ['D05'], '156 locally upside down from driver side, arrows point toward rear. Underside black insert/stalk separate; far face not yet observed.'),
    121: ('Passenger side-intake inner return candidate', 'provisional', ['K06','C02'], 'Cyan diagnostic appears behind the gray intake outer shell; full 121 ID is not readable. Paired source geometry supports this candidate only.'),
    141: ('Driver side-intake inner return candidate', 'provisional', ['K03','C02'], 'Green diagnostic appears behind the gray outer shell; digit fragments are insufficient for an exact-face assignment. Confirm full 141 or use one isolated patch.'),
})

sources = []
def evidence(eid, file, role, observations, original=None):
    p = ROOT/file
    sources.append(dict(id=eid, file=file, sha256=digest(p), role=role,
                        observations=observations, original_file=original))

for eid, file, obs, stamp in [
    ('G01','G01-driver','Driver side, upright grid; body +x front to rear; intake separate; wing outer face.','160427'),
    ('G02','G02-front-top','Hood nose at low x; roof and hood +y toward driver; front bumper and splitter cells.','160444'),
    ('G03','G03-passenger','Passenger side grid upside down; fuel cap on front fender; intake separate.','160451'),
    ('G04','G04-rear','Rear bumper shell versus central insert; rear deck separate from wing.','160459'),
    ('G05','G05-rear-overhead','Roof, flap, rear-glass ring and wing top; readable span direction.','160507'),
    ('G06','G06-whole-overhead','Hood/roof/deck orientation together; side pillars and separate wing.','160515')]:
    evidence(eid, f'rendered/{file}.png','numbered in-car screenshot',obs,f'Screenshot 2026-10-16 {stamp}.png')
for eid, file, obs, original in [
    ('E01','E01-fuel-cap','Oval on passenger front fender is the fuel cap, not a door handle.','Screenshot 2026-10-17 110542.png'),
    ('E02','E02-banner-clipping','Exterior BUNDY strip visible; lower letter crop of earlier layout.','Screenshot 2026-10-17 191437.png'),
    ('E03','E03-passenger-rejected-return','Lamp-loop bumper return runs wheelward; rejected exact path retained in seams.json.','Screenshot 2026-10-18 091306.png'),
    ('E04','E04-driver-rejected-return','Opposite lamp return does not complete the surround.','Screenshot 2026-10-18 091312.png'),
    ('E05','E05-driver-number','Driver number panel reads upright on car.','Screenshot 2026-10-18 154550.png'),
    ('E06','E06-passenger-number','Passenger number panel reads upright after 180-degree source rotation.','Screenshot 2026-10-18 154645.png'),
    ('E07','E07-hood-banner','Hood logo reads from front and inset banner is visible; not a measured safe-area boundary.','Screenshot 2026-10-18 154624.png')]:
    evidence(eid,f'rendered/{file}.png','livery in-car screenshot',obs,original)
for i,stamp,obs in [
    (1,'080548','Passenger mirror 078, handle 120 and side intake 080; arrows and local digit direction readable.'),
    (2,'080604','Passenger front duct 009 upside down and inner return 020; vertical louver label partly obscured.'),
    (3,'080620','Driver front duct 009 upright and inner return 020. Shared texels across both front ducts confirmed against D02.'),
    (4,'080629','Driver front wheelward vertical louver panel is colored, but its three-digit label is not fully readable.'),
    (5,'080641','Driver mirror 156, handle 143 and intake 153. Mirror and handle digits inverted; intake upright.'),
    (6,'080654','Wing top 030; separate deck lip upper face 040 and rear face 041; visible rear lettering picks up atlas color.'),
    (7,'080702','Wing underside 004, driver outer endplate 044 and passenger inner endplate 063; deck strip 041 and bumper inset 021.'),
    (8,'080718','Rear 021 upright, 040/041 inverted; wing top 030 inverted from rear. Rear lettering responds to atlas colors.'),
    (9,'080734','Wing underside 004, driver inner endplate 064 and passenger outer endplate 045.'),
    (10,'080745','Close passenger inner endplate 063 under wing underside 004.'),
    (11,'080754','Passenger outer endplate 045 upright, +x arrow toward vehicle front.')]:
    name=f'Screenshot 2026-10-19 {stamp}.png'
    evidence(f'D{i:02d}',f'rendered/d003/{name}','d003 numbered in-car screenshot',obs,name)
for i,stamp,obs in [
    (1,'081158','Driver headlight: lower painted return is on front bumper, mainly columns 10-11 and rows 47-54; upper/fender surround spans columns 13-18, rows 51-56. Cell-level correspondence only.'),
    (2,'081208','Passenger headlight: lower painted return is on front bumper, mainly columns 10-11 and rows 25-32; upper/fender surround spans columns 13-18, rows 23-28. Cell-level correspondence only.')]:
    name=f'Screenshot 2026-10-19 {stamp}.png'
    evidence(f'H{i:02d}',f'rendered/d004/{name}','d004 32px grid in-car screenshot',obs,name)
for i,stamp,obs in [
    (1,'082209','Passenger vertical front louver displays large 2; shared source with K02. Small fin IDs not fully readable.'),
    (2,'082220','Driver vertical front louver displays upright large 2. Separate colored upper/lower fins; IDs too small for confident full assignment.'),
    (3,'082233','Green inner intake patch behind neutral outer shell. Partial lettering only; candidate I141 remains provisional.'),
    (4,'082243','Mirror back/glass view shows colored rim; insufficient ID lettering to match a specific narrow component.'),
    (5,'082256','Opposite mirror back/glass view shows colored rim; insufficient ID lettering to match a specific narrow component.'),
    (6,'082306','Cyan inner intake return behind neutral outer shell; candidate I121, label not fully readable.')]:
    name=f'Screenshot 2026-10-19 {stamp}.png'
    evidence(f'K{i:02d}',f'rendered/d005/{name}','d005 remaining-parts in-car screenshot',obs,name)
for source in sources:
    if source['id'].startswith('D'):
        source['diagnostic_file']='diagnostics/d003-small-parts.tga'
        source['diagnostic_sha256']=digest(ROOT/source['diagnostic_file'])
    elif source['id'].startswith('H'):
        source['diagnostic_file']='diagnostics/d004-headlight-joins.tga'
        source['diagnostic_sha256']=digest(ROOT/source['diagnostic_file'])
    elif source['id'].startswith('K'):
        source['diagnostic_file']='diagnostics/d005-remaining-parts.tga'
        source['diagnostic_sha256']=digest(ROOT/source['diagnostic_file'])
evidence('C01','references/clyde-718-parts-1.jpg','Clyde exact-car rendered parts reference','Visible panel colors, wing TOP/L/R and paintable exterior surfaces.')
evidence('C02','references/clyde-718-parts-2.jpg','Clyde flat parts atlas JPEG','2048-square flat source; identity-layout correspondence visually checked against original wire; not a lossless paint mask.')
evidence('T01','wire/template-wire-r000.png','original template wire composite','Native source-space layout and template decal content.')
evidence('T02','wire/wire-source.png','isolated original Wire layer','Native green edges and white mesh; edges also enclose texture atlases.')
evidence('T03','template/template-number-blocks.png','original template Number Blocks export','Exact number reserve rectangles; not a physical panel boundary.')
evidence('T04','template/inventory.json','read-only original PSD structure inventory','52 records, layer hierarchy markers, bounds, channels, masks, clipping/blends, text/smart-object tags and image-resource IDs. Pitbox footprint is explicit.')
evidence('T05','template/original-mandatory-raster.png','raw original Car Mandatory layer RGBA','Native alpha establishes protected template overlay coverage. Layer has no additional mask; see inventory. Physical identities are independent of this protection classification.')
save('evidence.json',dict(recovered_on='2026-10-19',sources=sources))

psd = REPO/'cel-shaded/inputs/Porsche 718 GT4.psd'
archive = REPO/'cel-shaded/inputs/119_template_P718.zip'
header = psd.open('rb').read(26)
height,width,depth,mode=struct.unpack_from('>IIHH',header,14)
assert header[:4]==b'8BPS' and (width,height,depth,mode)==(2048,2048,8,3)
with zipfile.ZipFile(archive) as z:
    member = next(n for n in z.namelist() if n.endswith('Porsche 718 GT4.psd'))
    zip_psd_hash=hashlib.sha256(z.read(member)).hexdigest()
identity=dict(car='Porsche 718 Cayman GT4 Clubsport',car_folder='porsche718gt4',
              file='Porsche 718 GT4.psd',width=width,height=height,bit_depth=depth,color_mode='RGB',
              psd_sha256=digest(psd),archive_sha256=digest(archive),archive_member=member,
              archive_member_sha256=zip_psd_hash,psd_matches_archive=(digest(psd)==zip_psd_hash),
              source_path_from_mapping='../../../../cel-shaded/inputs/Porsche 718 GT4.psd',
              wire_sha256=digest(ROOT/'wire/wire-source.png'),
              template_release='Not supplied; hashes identify this exact saved version.')
save('template/identity.json',identity)

panels=[]
coverage={r['id']:r for r in csv.DictReader((ROOT/'template/mandatory-coverage.csv').open())}
with (ROOT/'islands.csv').open(newline='') as f:
    for row in csv.DictReader(f):
        n=int(row['id'][1:])
        if n==1: continue  # JPEG outer frame, not a UV island.
        name,status,ev,orientation=known.get(n,(f'Unidentified reference component {row["id"]}', 'unknown',['C02','T02'],'Unknown; use the matching d003 number before assigning a physical part.'))
        bbox=[int(row[k]) for k in ('x0','y0','x1','y1')]
        panels.append(dict(id=row['id'],name=name,status=status,bbox_px=bbox,
                           bbox_normalized=[v/2048 for v in bbox],mask_file=row['mask_file'],
                           mask_pixels=int(row['pixels']),geometry_status='provisional_reference_segmentation',
                           geometry_limit='JPEG foreground component; not an exact production clipping mask. One component may be part of a larger UV island.',
                           orientation=orientation,evidence=ev,diagnostic_label=f'{n:03d}',
                           exact_seams_confirmed=False))
        c=coverage[row['id']]; fraction=int(c['mandatory_opaque_pixels'])/int(c['pixels'])
        panels[-1]['template_mandatory_overlay']={
            'source':'T05','opaque_pixels':int(c['mandatory_opaque_pixels']),
            'selected_pixels':int(c['pixels']),'opaque_fraction':round(fraction,6),
            'limit':'Measured overlap with native template overlay, not a claim of on-car identity. JPEG edge fringes can lie outside the opaque overlay.'}
        panels[-1]['placement_policy']='protected_template_overlay' if fraction>=.95 else ('mapped_surface_check_orientation_and_seams' if status=='confirmed' else 'unresolved_do_not_guess')

features = [
    dict(id='F01',name='Passenger fuel-cap face',parent='I081',bbox_px=[786,751,872,812],status='confirmed',evidence=['E01','G03','T02'],note='Oval source becomes circular on car. This is not the door handle. Native green-wire bounds; accent safe area is outside face.'),
    dict(id='F02',name='Passenger handle recess',parent='I081',center_px=[1372,750],status='provisional',evidence=['T02','G03'],note='White mesh recess; outer handle may use a separate atlas piece. Earlier y771 treatment misplaced. Do not confuse with fuel cap.'),
    dict(id='F03',name='Driver handle recess',parent='I136',center_px=[1372,1787],status='provisional',evidence=['T02','G01'],note='White mesh recess; exact projected stroke bounds unverified.'),
    dict(id='F04',name='Roof escape flap',parent='I128',bbox_px=[1217,1263,1467,1462],status='provisional',evidence=['G05','T02'],note='Physical flap confirmed; approximate source bounds retained from wire review. Same roof texture, not a separate paint island.'),
    dict(id='F05',name='Driver door number reserve',parent='I136',bbox_px=[942,1852,1051,1925],status='confirmed',evidence=['T03','E05'],note='Template number-guide footprint, half-open bounds; text upright in UV. Reserve additional header/footer margin if adding panels.'),
    dict(id='F06',name='Passenger door number reserve',parent='I081',bbox_px=[942,615,1051,688],status='confirmed',evidence=['T03','E06'],note='Template number-guide footprint; text must rotate 180 degrees, not mirror.'),
    dict(id='F07',name='Rear bumper number reserve',parent='I007',bbox_px=[1248,71,1357,144],status='confirmed',evidence=['T03','G04'],note='Template footprint, independently measured from magenta pixels. Text upright from rear; keep body graphics clear of the sim-number area.'),
]
save('panels.json',dict(schema_version=1,updated='2026-10-19',template=identity,
                       coordinate_system='Native 2048 pixels, origin top-left, x right, y down; bbox [x0,y0,x1,y1) is half-open. Normalized values preserve y-down, not a renderer UV convention.',
                       status_scope='Panel status confirms physical identity only. Read orientation, provisional mask geometry, features, and seams independently.',
                       profile_complete=False,panels=panels,features=features,
                       template_regions=[dict(id='X01',name='Pitbox layer swatch',bbox_px=[1826,2022,1852,2046],evidence=['T04'],raster_file='template/original-pitbox-raster.png',placement_policy='reserve_for_pitbox_not_body_artwork',limit='Original layer footprint; pitbox appearance not inspected in-game.')],
                       review_constraints=[dict(date='2026-10-19',region='I126',note='User reports inside-cockpit review is difficult in the viewer and launching the game would be a large interruption. Defer cockpit-only confirmation; no in-game review claimed.')],
                       excluded_components=[dict(id='I001',reason='8189-pixel JPEG outer image frame; not a paint island.')]))

seams=[]
for sid,a,b,description,ev in [
    ('S01','I122','I081','Hood to passenger fender',['G02','G03']),
    ('S02','I122','I136','Hood to driver fender',['G02','G01']),
    ('S03','I122','I116','Hood nose to bumper across grille',['G02']),
    ('S04','I081','I116','Passenger fender to bumper around headlight',['G02','E03']),
    ('S05','I136','I116','Driver fender to bumper around headlight',['G02','E04']),
    ('S06','I116','I119','Front bumper to splitter',['G02']),
    ('S07','I081','I080','Passenger door/body to intake surround',['G03']),
    ('S08','I136','I153','Driver door/body to intake surround',['G01']),
    ('S09','I081','I128','Passenger roof rail to roof',['G05','G06']),
    ('S10','I136','I128','Driver roof rail to roof',['G05','G06']),
    ('S11','I128','I129','Roof to rear-glass surround',['G05']),
    ('S12','I081','I129','Passenger rear quarter to rear deck',['G05']),
    ('S13','I136','I129','Driver rear quarter to rear deck',['G05']),
    ('S14','I081','I007','Passenger rear quarter to rear bumper at lamp',['G03','G04']),
    ('S15','I136','I007','Driver rear quarter to rear bumper at lamp',['G01','G04']),
    ('S16','I007','I021','Rear bumper outer shell to center insert',['G04']),
    ('S17','I030','I044','Wing top to driver endplate',['G01','G05']),
    ('S18','I030','I045','Wing top to passenger endplate',['G03','G05']),
    ('S19','I030','I004','Wing leading/trailing lip to underside candidate',['C02']),
    ('S20','I125','I128','Exterior banner to roof across windshield edge',['G02','C01'])]:
    seams.append(dict(id=sid,panels=[a,b],description=description,status='provisional',
                      adjacency_evidence=ev,paired_boundary_points_px=[],
                      limitation='Views establish broad adjacency only; no matched source-space edge samples yet. A physical gap/trim may separate these surfaces.'))
for seam in seams:
    if seam['id'] in ('S04','S05'):
        driver=seam['id']=='S05'
        seam['adjacency_evidence'].append('H01' if driver else 'H02')
        seam['confirmed_cell_regions']={
            'resolution_px':32,
            'bumper_lower_return_cells_inclusive':{'columns':[10,11],'rows':[47,54] if driver else [25,32]},
            'fender_surround_cells_inclusive':{'columns':[13,18],'rows':[51,56] if driver else [23,28]},
            'limit':'Observed surrounding cells, not a paint polygon or paired endpoint. Lens boundary cuts cells; derive native edge from Wire and use endpoint ticks for exact continuation.'}
save('seams.json',dict(schema_version=1,coordinate_system='Same as panels.json',
    seams=seams,rejected=[
        dict(id='R01',description='Passenger wheelward path mistaken for lower lamp return',points_px=[[325,730],[326,811],[340,850]],evidence=['E03']),
        dict(id='R02',description='Driver wheelward path mistaken for lower lamp return',points_px=[[340,1683],[326,1740],[325,1800]],evidence=['E04']),
        dict(id='R03',description='One universal side reflection',evidence=['E05','E06'],reason='Decorative raster copy used y=2536-y; earlier door art used approximately 2560-y. Neither is an exact car-wide transform, and mirrored lettering is wrong.')]))

lines=['# Panel and feature lookup','',
       'Use this with [overlay.png](overlay.png), [panels.json](panels.json), and [seams.json](seams.json). IDs label reference components, not an asserted one-to-one inventory of physical panels. All masks are provisional JPEG-derived selections. Confirmed below means the named physical surface is established, not that every boundary or orientation is calibrated.','',
       'Bounds are native pixels `[x0,y0,x1,y1)`; x points right and y down. Both sides are named from the seated driver, not the camera.','',
       '| ID | Physical region | Source bounds | Identity | Direction / limit |','| --- | --- | --- | --- | --- |']
for p in panels:
    if int(p['id'][1:]) in known:
        lines.append(f'| {p["id"]} | {p["name"]} | {p["bbox_px"]} | {p["status"]} | {p["orientation"]} |')
lines += ['', '## Features within islands','', '| ID | Feature | Source coordinates | Status | Limit |','| --- | --- | --- | --- | --- |']
for f in features:
    lines.append(f'| {f["id"]} | {f["name"]} ({f["parent"]}) | {f.get("bbox_px",f.get("center_px"))} | {f["status"]} | {f["note"]} |')
lines += ['', '## Protected and unresolved components','',
          'The original Car Mandatory layer covers at least 95% of each component marked protected_template_overlay in panels.json. These selections are reserved from livery placement even when their exact physical fixture is unnamed. Use the native mandatory alpha, not JPEG fringes, for exact protection. This is a measured template classification, not a claim that every hidden fixture has been viewed on the car.','',
          'The unresolved components without that protection are listed below. Do not guess their physical surface or transform. A repeating guide color is not proof of shared UVs.','',
          '| ID | Candidate / unknown | Bounds |','| --- | --- | --- |']
for p in panels:
    if p['placement_policy']=='unresolved_do_not_guess':lines.append(f'| {p["id"]} | {p["name"]} | {p["bbox_px"]} |')
lines += ['',
          'Contact sheets: '+', '.join(f'[sheet {i}](previews/atlas-{i:02d}.jpg)' for i in range(1,8))+'.','']
(ROOT/'panel-guide.md').write_text('\n'.join(lines),encoding='utf-8')
print(f'Indexed {len(panels)} reference components, {len(features)} features, {len(seams)} provisional joins and {len(sources)} sources. Full mapping remains incomplete pending diagnostic review.')
