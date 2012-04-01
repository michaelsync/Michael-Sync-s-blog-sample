----Note: This script will be more cool if you get the default file location by using this script
----http://blogs.technet.com/b/sqlman/archive/2009/07/20/tsql-script-determining-default-database-file-log-path.aspx

IF EXISTS(SELECT 1 FROM [master].[sys].[databases] WHERE [name] = 'zzTempDBForDefaultPath')   

BEGIN  
    DROP DATABASE zzTempDBForDefaultPath   
END;

-- Create temp database. Because no options are given, the default data and --- log path locations are used 
CREATE DATABASE zzTempDBForDefaultPath;

--Declare variables for creating temp database   
DECLARE @Default_Data_Path VARCHAR(512),   
        @Default_Log_Path VARCHAR(512);

--Get the default data path   
SELECT @Default_Data_Path =    
(   SELECT LEFT(physical_name,LEN(physical_name)-CHARINDEX('\',REVERSE(physical_name))+1) 
    FROM sys.master_files mf   
    INNER JOIN sys.[databases] d   
    ON mf.[database_id] = d.[database_id]   
    WHERE d.[name] = 'zzTempDBForDefaultPath' AND type = 0);

--Get the default Log path   

SELECT @Default_Log_Path =    
(   SELECT LEFT(physical_name,LEN(physical_name)-CHARINDEX('\',REVERSE(physical_name))+1)   
    FROM sys.master_files mf   
    INNER JOIN sys.[databases] d   
    ON mf.[database_id] = d.[database_id]   
    WHERE d.[name] = 'zzTempDBForDefaultPath' AND type = 1);

--Clean up. Drop de temp database 

IF EXISTS(SELECT 1 FROM [master].[sys].[databases] WHERE [name] = 'zzTempDBForDefaultPath')   
BEGIN  
    DROP DATABASE zzTempDBForDefaultPath   
END;

SELECT @Default_Data_Path as 'DataPath', @Default_Log_Path as 'LogFilePath';
