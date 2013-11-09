#Modified and simplified version of https://www.windowsazure.com/en-us/develop/net/common-tasks/continuous-delivery/


$thumbprint = "{Your Cert's Thumbprint}"
$myCert = Get-Item cert:\\CurrentUser\My\$thumbprint
$subscriptionId = "{Your Subscription Id}"
$subscriptionName = "{Your Subscription Name}"
$webroleservice = "{Your Web Role Name}"
$workerroleservice = "{Your Worker Role Name}"

$slot = "staging" #staging or production

$package = "{Path of your Azure project}\bin\Release\app.publish\{Your Project}.cspkg"
$configuration = {Path of your Azure project}\bin\Release\app.publish\ServiceConfiguration.Cloud.cscfg"

$timeStampFormat = "g"

Write-Output "Running Azure Imports"
Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\Azure\Azure.psd1"
Import-AzurePublishSettingsFile "{Path where you stored your azure setting\Azure.publishsettings"

function Publish(){
  PublishInternal $webroleservice
  PublishInternal $workerroleservice
}

function PublishInternal($service){

 Write-Output "Publising" 
 Write-Output $service

 Set-AzureSubscription -CurrentStorageAccount $service -SubscriptionName $subscriptionName -SubscriptionId $subscriptionId -Certificate $myCert
 Write-Output "Set-AzureSubscription" 


 $deploymentLabel = "ContinuousDeploy to $service v%build.number%"

 Write-Output $deploymentLabel 

 $deployment = Get-AzureDeployment -ServiceName $service -Slot $slot -ErrorVariable a -ErrorAction silentlycontinue 
 Write-Output a 

 if ($a[0] -ne $null) {
    Write-Output "$(Get-Date -f $timeStampFormat) - No deployment is detected. Creating a new deployment. "
 }
 
 if ($deployment.Name -ne $null) {
    #Update deployment inplace (usually faster, cheaper, won't destroy VIP)
    Write-Output "$(Get-Date -f $timeStampFormat) - Deployment exists in $servicename.  Upgrading deployment."
    UpgradeDeployment $service $deploymentLabel 
 } else {
    CreateNewDeployment $service $deploymentLabel 
 }
}


function CreateNewDeployment($service, $deploymentLabel)
{
    write-progress -id 3 -activity "Creating New Deployment" -Status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Creating New Deployment: In progress"

    $opstat = New-AzureDeployment -Slot $slot -Package $package -Configuration $configuration -label $deploymentLabel -ServiceName $service

    $completeDeployment = Get-AzureDeployment -ServiceName $service -Slot $slot
    $completeDeploymentID = $completeDeployment.deploymentid

    write-progress -id 3 -activity "Creating New Deployment" -completed -Status "Complete"
    Write-Output "$(Get-Date -f $timeStampFormat) - Creating New Deployment: Complete, Deployment ID: $completeDeploymentID"
}

function UpgradeDeployment($service, $deploymentLabel)
{
    write-progress -id 3 -activity "Upgrading Deployment" -Status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Upgrading Deployment: In progress"

    # perform Update-Deployment
    $setdeployment = Set-AzureDeployment -Upgrade -Slot $slot -Package $package -Configuration $configuration -label $deploymentLabel -ServiceName $service -Force

    $completeDeployment = Get-AzureDeployment -ServiceName $service -Slot $slot
    $completeDeploymentID = $completeDeployment.deploymentid

    write-progress -id 3 -activity "Upgrading Deployment" -completed -Status "Complete"
    Write-Output "$(Get-Date -f $timeStampFormat) - Upgrading Deployment: Complete, Deployment ID: $completeDeploymentID"
}

Write-Output "Create Azure Deployment"
Publish