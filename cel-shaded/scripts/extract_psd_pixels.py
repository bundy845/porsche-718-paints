"""Read selected raster previews from a PSD without changing the source.

This is a small PSD v1 reader for the selected layer channels. The original PSD
remains the editable source for text and smart objects.
"""
from __future__ import annotations

import json
import struct
import zlib
from pathlib import Path

SOURCE = Path(r"C:\Dropbox\2 Old 2 Slow\Racing\iRacing\Paints\MWM Season 12\Porsche 992_2 Cup-Recovered.psd")
OUT = Path(__file__).resolve().parent.parent / "design" / "extracted-992-assets"
NAMES = {
    "2old2slow logo - Passenger Door": "2-old-2-slow-passenger-door",
    "2old2slow logo - Driver Door": "2-old-2-slow-driver-door",
    "You don't have to get ready if you stay ready": "stay-ready-bumper-text",
    "Nothing echoes louder than the truth": "truth-bumper-text",
    "Me outlined": "me-outlined",
}


def u16(data: bytes, pos: int) -> int:
    return struct.unpack_from(">H", data, pos)[0]


def i16(data: bytes, pos: int) -> int:
    return struct.unpack_from(">h", data, pos)[0]


def u32(data: bytes, pos: int) -> int:
    return struct.unpack_from(">I", data, pos)[0]


def i32(data: bytes, pos: int) -> int:
    return struct.unpack_from(">i", data, pos)[0]


def read_packbits(row: bytes, width: int) -> bytes:
    out = bytearray()
    pos = 0
    while pos < len(row):
        count = struct.unpack_from("b", row, pos)[0]
        pos += 1
        if count >= 0:
            out.extend(row[pos : pos + count + 1])
            pos += count + 1
        elif count != -128:
            out.extend(row[pos : pos + 1] * (1 - count))
            pos += 1
    if len(out) != width:
        raise ValueError(f"PackBits row: {len(out)} != {width}")
    return bytes(out)


def decode_channel(block: bytes, width: int, height: int) -> bytes:
    method = u16(block, 0)
    if method == 0:
        result = block[2:]
    elif method == 1:
        row_sizes = struct.unpack_from(">" + "H" * height, block, 2)
        at = 2 + height * 2
        rows = []
        for size in row_sizes:
            rows.append(read_packbits(block[at : at + size], width))
            at += size
        result = b"".join(rows)
    elif method in (2, 3):
        result = zlib.decompress(block[2:])
        if method == 3:
            b = bytearray(result)
            for y in range(height):
                start = y * width
                for x in range(1, width):
                    b[start + x] = (b[start + x] + b[start + x - 1]) & 255
            result = bytes(b)
    else:
        raise ValueError(f"Unsupported PSD compression {method}")
    if len(result) != width * height:
        raise ValueError(f"Channel size: {len(result)} != {width * height}")
    return result


def chunk(tag: bytes, data: bytes) -> bytes:
    return struct.pack(">I", len(data)) + tag + data + struct.pack(">I", zlib.crc32(tag + data) & 0xFFFFFFFF)


def save_png(path: Path, width: int, height: int, rgba: bytes) -> None:
    stride = width * 4
    scanlines = b"".join(b"\x00" + rgba[y * stride : (y + 1) * stride] for y in range(height))
    path.write_bytes(
        b"\x89PNG\r\n\x1a\n"
        + chunk(b"IHDR", struct.pack(">IIBBBBB", width, height, 8, 6, 0, 0, 0))
        + chunk(b"IDAT", zlib.compress(scanlines, 9))
        + chunk(b"IEND", b"")
    )


def main() -> None:
    data = SOURCE.read_bytes()
    if data[:4] != b"8BPS" or u16(data, 4) != 1:
        raise ValueError("Expected PSD version 1")
    canvas_h, canvas_w, depth, mode = struct.unpack_from(">IIHH", data, 14)
    if depth != 8 or mode != 3:
        raise ValueError(f"Expected 8-bit RGB, got {depth}-bit mode {mode}")
    pos = 26
    pos += 4 + u32(data, pos)  # color mode data
    pos += 4 + u32(data, pos)  # image resources
    layer_mask_size = u32(data, pos)
    pos += 4
    if not layer_mask_size:
        raise ValueError("No layer data")
    layer_info_size = u32(data, pos)
    pos += 4
    layer_end = pos + layer_info_size
    count = abs(i16(data, pos))
    pos += 2
    records = []
    for _ in range(count):
        top, left, bottom, right = struct.unpack_from(">iiii", data, pos)
        pos += 16
        channel_count = u16(data, pos)
        pos += 2
        channels = []
        for _ in range(channel_count):
            channels.append((i16(data, pos), u32(data, pos + 2)))
            pos += 6
        blend_signature = data[pos : pos + 4]
        if blend_signature != b"8BIM":
            raise ValueError("Bad layer blend signature")
        pos += 12
        extra_size = u32(data, pos)
        pos += 4
        extra_end = pos + extra_size
        pos += 4 + u32(data, pos)  # mask
        pos += 4 + u32(data, pos)  # blending ranges
        name_len = data[pos]
        name = data[pos + 1 : pos + 1 + name_len].decode("macroman", errors="replace")
        pos += (name_len + 1 + 3) & ~3
        while pos + 12 <= extra_end:
            signature = data[pos : pos + 4]
            key = data[pos + 4 : pos + 8]
            pos += 8
            if signature not in (b"8BIM", b"8B64"):
                break
            size = u32(data, pos)
            pos += 4
            payload = data[pos : pos + size]
            if key == b"luni" and len(payload) >= 4:
                nchars = u32(payload, 0)
                name = payload[4 : 4 + nchars * 2].decode("utf-16-be", errors="replace")
            pos += size + (size & 1)
        pos = extra_end
        records.append({"name": name, "bounds": (left, top, right, bottom), "channels": channels})
    OUT.mkdir(parents=True, exist_ok=True)
    report = []
    for record in records:
        left, top, right, bottom = record["bounds"]
        width, height = right - left, bottom - top
        decoded = {}
        for channel_id, length in record["channels"]:
            block = data[pos : pos + length]
            if record["name"] in NAMES and channel_id in (0, 1, 2, -1):
                decoded[channel_id] = decode_channel(block, width, height)
            pos += length
        if record["name"] not in NAMES:
            continue
        if width <= 0 or height <= 0:
            raise ValueError(f"No pixels for {record['name']}")
        count_px = width * height
        channels = [decoded.get(cid, bytes([255]) * count_px) for cid in (0, 1, 2, -1)]
        alpha = channels[3]
        visible = [i for i, value in enumerate(alpha) if value]
        if not visible:
            raise ValueError(f"Entirely transparent: {record['name']}")
        xmin = min(i % width for i in visible)
        xmax = max(i % width for i in visible) + 1
        ymin = min(i // width for i in visible)
        ymax = max(i // width for i in visible) + 1
        crop_w, crop_h = xmax - xmin, ymax - ymin
        rgba = bytearray(crop_w * crop_h * 4)
        for y in range(crop_h):
            for x in range(crop_w):
                source_index = (y + ymin) * width + x + xmin
                target_index = (y * crop_w + x) * 4
                for cid in range(4):
                    rgba[target_index + cid] = channels[cid][source_index]
        basename = NAMES[record["name"]]
        save_png(OUT / f"{basename}.png", crop_w, crop_h, rgba)
        report.append({"layer": record["name"], "file": f"{basename}.png", "source_bounds": [left, top, right, bottom], "crop_within_layer": [xmin, ymin, xmax, ymax], "size": [crop_w, crop_h]})
    if len(report) != len(NAMES):
        found = {r["layer"] for r in report}
        raise ValueError(f"Missing layers: {set(NAMES) - found}")
    (OUT / "extraction-report.json").write_text(json.dumps({"source": str(SOURCE), "canvas": [canvas_w, canvas_h], "assets": report}, indent=2), encoding="utf-8")
    print(json.dumps(report, indent=2))


if __name__ == "__main__":
    main()
