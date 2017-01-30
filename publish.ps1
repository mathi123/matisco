  . .\increment-minor-version.ps1

 $solutionDir = split-path -parent $MyInvocation.MyCommand.Definition
 $nugetExe = "$solutionDir\packages\NuGet.CommandLine.3.5.0\tools\NuGet.exe" 

 [string[]] $projects = "Matisco.Wpf","Matisco.WebApi.Client"
 [string[]] $nuspecs = @()
 
 foreach($project in $projects){
	 $nuspecs += "$solutionDir\$project\$project.nuspec"
 }

 $version = "0.0.1"

 foreach ($nuspec in $nuspecs) 
 {
	 $nuspecVersion = GetNuspecVersion($nuspec)
	 if($nuspecVersion -gt $version)
	 {
		 $version = $nuspecVersion;
	 }
 }

 $version = IncrementVersion($version)
 foreach ($nuspec in $nuspecs) 
 {
	 SetNuspecVersion $nuspec $version
 }
 
 Write-Host "Incremented all packages to version $version"

 foreach($nuspec in $nuspecs){
	 Write-Host "packing $nuspec"
	 try
	 {
		Invoke-Expression "$nugetExe pack $nuspec"
	 }
	 catch
	 {
		 Exit 0;
	 }
 }

 foreach($project in $projects){
	 $package = "$solutionDir\$project.$version.nupkg"
	 Write-Host "publishing $package"
	 Invoke-Expression "$nugetExe push $package -Source https://www.nuget.org/api/v2/package"
 }