#Set the path of folder that you want to edit the csprojs
$private:dir = "" 


$private:devEnvName = "dev-env";  

$local:conditionName = "`"'`$(Configuration)|`$(Platform)`' == `'{0}|AnyCPU'`"";

$local:configString =  "<PropertyGroup Condition={0}>
    <OutputPath>bin\</OutputPath>
    <DefineConstants>TRACE;DEBUG;{1}</DefineConstants>
    <Optimize>true</Optimize>
    <DebugType>pdbonly</DebugType>
    <PlatformTarget>AnyCPU</PlatformTarget>
    <ErrorReport>prompt</ErrorReport>
  </PropertyGroup>";

#http://blogs.msdn.com/b/jaredpar/archive/2008/06/12/is-there-anything-in-that-pipeline.aspx by JaredPar MSFT   
function Any() {
    begin {
        $any = $false			
    }
    process {
        $any = $true				
    }
    end {
        $any				
    }
}

function RemovePropertyGroupByConditionName($project, $propertyGroupConditionName){	
	if($project.PropertyGroup){
		$project.PropertyGroup | Foreach {
				if($_.GetAttribute('Condition').Trim() -eq $propertyGroupConditionName.Trim()){				
					$a = $project.RemoveChild($_);
					Write-Host $_.GetAttribute('Condition')"has been removed." -foregroundcolor red;
				}	
		};
	}
}

function AddNewPropertyGroup($sourceXml, $settingName, $preprocessorDirective){
	$propertyGroups = $sourceXml.Project.PropertyGroup;
	
	if($propertyGroups){
		if(!($propertyGroups | Where-Object { $_.GetAttribute('Condition').Trim() -eq [string]::Format($conditionName, $settingName) } | Any)){
			$newPropertyGroup = [xml] [string]::Format([string]::Format($configString, $conditionName, $preprocessorDirective), $settingName);				
			$importNode = $sourceXml.ImportNode($newPropertyGroup.DocumentElement, $true);	
			$_ = $sourceXml.Project.InsertAfter($importNode, $propertyGroups[0]);
			Write-Host $settingName" has been added.";
		}
	}
}

Get-ChildItem $dir *.csproj -recurse | 
% { 
	$content = [xml](gc $_.FullName);	  
	  
	Write-Host "Reading "$_.FullName -foregroundcolor yellow;
	  
	$project = $content.Project;	
	
	#RemovePropertyGroupByConditionName $project "`'`$(Configuration)|`$(Platform)`' == `'old-propertyGroup|AnyCPU`'"	
	  
	#AddNewPropertyGroup $content $devEnvName "LIFA16";
			
	$content = [xml] $content.OuterXml.Replace(" xmlns=`"`"", "")
	$content.Save($_.FullName);	
	
	Write-Host "";
}