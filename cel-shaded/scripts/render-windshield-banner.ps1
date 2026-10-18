Add-Type -AssemblyName System.Drawing

$projectRoot = Split-Path -Parent $PSScriptRoot
$templatePath = Join-Path $projectRoot 'design\previews\template-visible-r000.png'
$outputPath = Join-Path $projectRoot 'graphics\current\windshield-banner-black.png'
$template = [System.Drawing.Bitmap]::new($templatePath)
$overlay = [System.Drawing.Bitmap]::new(2048, 2048, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$black = [System.Drawing.Color]::FromArgb(255, 13, 15, 18)
$edge = [System.Drawing.Color]::FromArgb(255, 32, 40, 46)
$firstRow = 2048
$lastRow = -1

try {
    # The exterior strip is the blue Michelin/Pilot Challenge island immediately
    # right of the dark inside-facing windshield strip in this exact PSD.
    # Derive each row's span from the template so the banner follows its curved UV edge.
    for ($y = 990; $y -le 1550; $y++) {
        $minX = 2048
        $maxX = -1
        for ($x = 999; $x -le 1100; $x++) {
            $c = $template.GetPixel($x, $y)
            if ($c.R -lt 25 -and $c.G -ge 70 -and $c.G -lt 130 -and $c.B -gt 140 -and $c.B -lt 200) {
                if ($x -lt $minX) { $minX = $x }
                if ($x -gt $maxX) { $maxX = $x }
            }
        }
        if ($maxX -lt $minX -or ($maxX - $minX) -lt 12) { continue }
        if ($y -lt $firstRow) { $firstRow = $y }
        if ($y -gt $lastRow) { $lastRow = $y }
        $left = [Math]::Max(1001, $minX - 1)
        $right = [Math]::Min(1094, $maxX + 1)
        for ($x = $left; $x -le $right; $x++) {
            $overlay.SetPixel($x, $y, $(if ($x -eq $left -or $x -eq $right) { $edge } else { $black }))
        }
    }
    if ($firstRow -gt 1005 -or $lastRow -lt 1530) { throw "Unexpected exterior windshield island rows: $firstRow-$lastRow" }
    $overlay.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    "Saved $outputPath; exterior strip rows $firstRow-$lastRow"
}
finally {
    $overlay.Dispose()
    $template.Dispose()
}
