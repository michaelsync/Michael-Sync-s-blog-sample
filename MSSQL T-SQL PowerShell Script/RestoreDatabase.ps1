# Use Invoke-SqlCmd instead of SqlCmd which is designed for cmd env instead of powershell
#http://blogs.msdn.com/b/mwories/archive/2008/06/14/sql2008_5f00_powershell.aspx
#http://sev17.com/2010/07/making-a-sqlps-module/

$db_server_name = "localhost"
$new_db_name = "your_new_db_name"
$database_bak_path = "C:\BAK\your_db_bak_file.bak"
$script_file_path = "C:\Scripts\" #path of GetDefaultDatabaseFileLocation.sql


$database_file_location = Invoke-Sqlcmd -ServerInstance $db_server_name -InputFile $script_file_path"GetDefaultDatabaseFileLocation.sql" -Variable path=$database_bak_path -AbortOnError -OutputSqlErrors 1

$db_data_file_path = $database_file_location.DataPath
$db_log_file_path = $database_file_location.LogFilePath

$logical_phyiscal_names = Invoke-Sqlcmd -ServerInstance $db_server_name -Query "RESTORE FILELISTONLY FROM DISK ='$database_bak_path'" -AbortOnError -OutputSqlErrors 1

$logical_db_name = $logical_phyiscal_names[0].LogicalName
$logical_logfile_name = $logical_phyiscal_names[1].LogicalName
$physical_db_name = [System.IO.Path]::GetFileName($logical_phyiscal_names[0].PhysicalName)
$physical_logfile_name = [System.IO.Path]::GetFileName($logical_phyiscal_names[1].PhysicalName)

$output = invoke-sqlcmd -serverinstance $db_server_name -query "restore database $new_db_name from disk =  '$database_bak_path' with file = 1,
   move '$logical_db_name' to '$db_data_file_path\$physical_db_name',
   move '$logical_logfile_name' to '$db_log_file_path\$physical_logfile_name'" 

$db_data_file_path
$db_log_file_path
$logical_db_name
$logical_logfile_name
$physical_db_name
$physical_logfile_name

$output

Write "Database '$new_db_name' has been restored."

