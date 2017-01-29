$nuspec = "C:\Users\MathiasColpaert\Source\Repos\matisco\Matisco.WebApi.Client\Matisco.WebApi.Client.nuspec"

$text = [System.IO.File]::ReadAllText($nuspec)
$rxVersionTag = New-Object System.Text.RegularExpressions.Regex "\<version\>(?<Version>\d+\.\d+\.\d+)\</version\>", SingleLine
$version = $rxVersionTag.Match($text).Groups["Version"].Value

Write-Host $version
$majorMinor = $version.SubString(0, $version.LastIndexOf(".") + 1)
$revision = $version.SubString($version.LastIndexOf(".") + 1)
$rev = ([int]::Parse($revision) + 1)

$newVersion = [string]$majorMinor + $rev
Write-Host $newVersion

$text = $rxVersionTag.Replace($text, "<version>" + $newVersion + "</version>")
[System.IO.File]::WriteAllText($nuspec, $text)