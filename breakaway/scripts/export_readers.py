"""Established 24-bit iRacing TGA decoder, retained from the verified exporter checks."""
import struct


def tga(path):
    data = path.read_bytes()
    ident, cmap, kind = data[:3]
    width, height, bits, flags = struct.unpack_from('<HHBB', data, 12)
    assert cmap == 0 and kind in (2, 10) and (bits == 24)
    assert (width, height) == (2048, 2048)
    pos = 18 + ident
    out = bytearray()
    if kind == 2:
        out.extend(data[pos:pos + width * height * 3])
    else:
        while len(out) < width * height * 3:
            packet = data[pos]
            pos += 1
            count = (packet & 127) + 1
            if packet & 128:
                out.extend(data[pos:pos + 3] * count)
                pos += 3
            else:
                out.extend(data[pos:pos + count * 3])
                pos += count * 3
    assert len(out) == width * height * 3
    if not flags & 32:
        stride = width * 3
        out = bytearray(b''.join((out[y * stride:(y + 1) * stride] for y in range(height - 1, -1, -1))))
    return out
