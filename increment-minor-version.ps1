Function IncrementNuspecVersion
{
	Param(
	[Parameter(Mandatory=$True)]
	[string] $nuspecPath)

	BEGIN
	{
		$version = GetNuspecVersion($nuspecPath)
		$newVersion = IncrementVersion($version)
		SetNuspecVersion $nuspecPath $newVersion
	}
}

Function GetNuspecVersion
{
	Param(
	[Parameter(Mandatory=$True)]
	[string] $nuspecPath)

	BEGIN
	{
		$text = [System.IO.File]::ReadAllText($nuspecPath)
		$rxVersionTag = New-Object System.Text.RegularExpressions.Regex "\<version\>(?<Version>\d+\.\d+\.\d+)\</version\>", SingleLine
		$version = $rxVersionTag.Match($text).Groups["Version"].Value

		RETURN $version;
	}
}

Function SetNuspecVersion
{
	Param(
	[Parameter(Mandatory=$True)]
	[string] $nuspecPath, 
	[Parameter(Mandatory=$True)]
	[string] $newVersion
	)

	BEGIN
	{
		$text = [System.IO.File]::ReadAllText($nuspecPath)
		$rxVersionTag = New-Object System.Text.RegularExpressions.Regex "\<version\>(?<Version>\d+\.\d+\.\d+)\</version\>", SingleLine

		$text = $rxVersionTag.Replace($text, "<version>" + $newVersion + "</version>")
		[System.IO.File]::WriteAllText($nuspecPath, $text)
	}
}

Function IncrementVersion
{
	Param(
	[Parameter(Mandatory=$True)]
	[string] $version)

	BEGIN
	{
		$majorMinor = $version.SubString(0, $version.LastIndexOf(".") + 1)
		$revision = $version.SubString($version.LastIndexOf(".") + 1)
		$rev = ([int]::Parse($revision) + 1)
		$newVersion = [string]$majorMinor + $rev

		RETURN $newVersion;
	}
}