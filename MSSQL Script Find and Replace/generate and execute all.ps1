# Use Invoke-SqlCmd instead of SqlCmd which is designed for cmd env instead of powershell
#http://blogs.msdn.com/b/mwories/archive/2008/06/14/sql2008_5f00_powershell.aspx
#http://sev17.com/2010/07/making-a-sqlps-module/

$db_server_name = "localhost"

$old_db_name = "your_old_db_name"
$new_db_name = "your_new_db_name"
$current_db_name = "your_current_db_name" #When we execute all generated script file, we need to specify the current db name
$script_file_path = "C:\Work\scripttest\"
$output_dir = "C:\Work\scripttest\output\"


$para_array = "old_value='$old_db_name'", "new_value='$new_db_name'","output_dir='$output_dir'"

Invoke-Sqlcmd -ServerInstance $db_server_name -InputFile $script_file_path"Fine_and_replace.sql" -Database $current_db_name -Variable $para_array -AbortOnError -OutputSqlErrors 1

Get-ChildItem $output_dir -recurse -Include *.sql | ForEach-Object { 
    Write-Host $_.FullName
    Invoke-Sqlcmd -ServerInstance $db_server_name -InputFile $_.FullName -Database $current_db_name -AbortOnError -OutputSqlErrors 1
    } 

