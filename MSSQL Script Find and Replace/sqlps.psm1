## Use Invoke-SqlCmd instead of SqlCmd which is designed for cmd env instead of powershell
#http://blogs.msdn.com/b/mwories/archive/2008/06/14/sql2008_5f00_powershell.aspx
#http://sev17.com/2010/07/making-a-sqlps-module/

#
# Initialize-SqlpsEnvironment.ps1
#
# Loads the SQL Server provider extensions
#
# Usage: Powershell -NoExit -Command "& '.\Initialize-SqlPsEnvironment.ps1'"
#
# Change log:
# June 14, 2008: Michiel Wories
#   Initial Version
# June 17, 2008: Michiel Wories
#   Fixed issue with path that did not allow for snapin\provider:: prefix of path
#   Fixed issue with provider variables. Provider does not handle case yet
#   that these variables do not exist (bug has been filed)
$ErrorActionPreference = "Stop"
$sqlpsreg="HKLM:\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.SqlServer.Management.PowerShell.sqlps"
if (Get-ChildItem $sqlpsreg -ErrorAction "SilentlyContinue")
{
    throw "SQL Server Powershell is not installed."
}
else
{
    $item = Get-ItemProperty $sqlpsreg
    $sqlpsPath = [System.IO.Path]::GetDirectoryName($item.Path)
}

#
# Preload the assemblies. Note that most assemblies will be loaded when the provider
# is used. if you work only within the provider this may not be needed. It will reduce
# the shell's footprint if you leave these out.
#
$assemblylist = 
"Microsoft.SqlServer.Smo",
"Microsoft.SqlServer.Dmf ",
"Microsoft.SqlServer.SqlWmiManagement ",
"Microsoft.SqlServer.ConnectionInfo ",
"Microsoft.SqlServer.SmoExtended ",
"Microsoft.SqlServer.Management.RegisteredServers ",
"Microsoft.SqlServer.Management.Sdk.Sfc ",
"Microsoft.SqlServer.SqlEnum ",
"Microsoft.SqlServer.RegSvrEnum ",
"Microsoft.SqlServer.WmiEnum ",
"Microsoft.SqlServer.ServiceBrokerEnum ",
"Microsoft.SqlServer.ConnectionInfoExtended ",
"Microsoft.SqlServer.Management.Collector ",
"Microsoft.SqlServer.Management.CollectorEnum"

foreach ($asm in $assemblylist)
{
    $asm = [Reflection.Assembly]::LoadWithPartialName($asm)
}
#
# Set variables that the provider expects (mandatory for the SQL provider)
#
Set-Variable -scope Global -name SqlServerMaximumChildItems -Value 0
Set-Variable -scope Global -name SqlServerConnectionTimeout -Value 30
Set-Variable -scope Global -name SqlServerIncludeSystemObjects -Value $false
Set-Variable -scope Global -name SqlServerMaximumTabCompletion -Value 1000
#
# Load the snapins, type data, format data
#
Push-Location
cd $sqlpsPath
Add-PSSnapin SqlServerCmdletSnapin100
Add-PSSnapin SqlServerProviderSnapin100
Update-TypeData -PrependPath SQLProvider.Types.ps1xml 
update-FormatData -prependpath SQLProvider.Format.ps1xml 
Pop-Location
Write-Host -ForegroundColor Yellow 'SQL Server Powershell extensions are loaded.'
Write-Host
Write-Host -ForegroundColor Yellow 'Type "cd SQLSERVER:\" to step into the provider.'
Write-Host
Write-Host -ForegroundColor Yellow 'For more information, type "help SQLServer".'