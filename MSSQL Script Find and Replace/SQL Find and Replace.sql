

DECLARE @old_value NVARCHAR(20)
SET @old_value = 'your_old_value'

DECLARE @new_value NVARCHAR(20)
SET @new_value = 'your_new_value'

DECLARE @output_dir NVARCHAR(500)
SET @output_dir = 'C:\\output\\'

DECLARE @search_result TABLE
(
  ROWID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  ROUTINE_TYPE nvarchar(60),
  ROUTINE_NAME sysname /* Meaning nvarchar(128) */,
  ROUTINE_DEFINITION nvarchar(max)  /* max indicates that the maximum storage size is 2^31-1 bytes (2 GB) */
)

DECLARE @row_count INT
DECLARE @current_row INT

/*Note: Don't use INFORMATION_SCHEMA.ROUTINES because ROUTINE_DEFINITION is NVARCHAR(4000). */

INSERT INTO @search_result (ROUTINE_TYPE, ROUTINE_NAME, ROUTINE_DEFINITION)
    SELECT o.type_desc AS ROUTINE_TYPE
          ,o.[name] AS ROUTINE_NAME
          ,m.definition AS ROUTINE_DEFINITION
    FROM sys.sql_modules AS m
    INNER JOIN sys.objects AS o
        ON m.object_id = o.object_id
    WHERE 
		(o.type_desc = 'SQL_STORED_PROCEDURE' OR o.type_desc = 'SQL_SCALAR_FUNCTION') AND
		 m.definition LIKE '%' + @old_value + '%'
    
SET @row_count = @@ROWCOUNT /*Note: Make sure the count is not more than 2 billion. */  
SET @current_row = 1 

PRINT @row_count

WHILE @current_row <= @row_count
BEGIN

   DECLARE @new_routine_definition nvarchar(max)   
   DECLARE @routine_type nvarchar(60)
   DECLARE @routine_name sysname
   
   
   SELECT 
		@routine_name = ROUTINE_NAME,
		@routine_type =
		(CASE 
			WHEN ROUTINE_TYPE = 'SQL_STORED_PROCEDURE' THEN 
				 'PROCEDURE'	
			WHEN ROUTINE_TYPE = 'SQL_SCALAR_FUNCTION' THEN 
				 'FUNCTION'
		END),
		@new_routine_definition = replace(ROUTINE_DEFINITION,@old_value,@new_value)
        FROM @search_result
        WHERE ROWID = @current_row
    
	
    SET @new_routine_definition = replace(@new_routine_definition, 'CREATE ' + @routine_type, 'ALTER ' + @routine_type)
   
    DECLARE @file_system_object INT,
		@text_stream INT,
		@error_object INT,
		@hr INT,
		@path VARCHAR(1000),
		@error_message VARCHAR(500)
	
	SET @path = @output_dir + @routine_name + '.sql'
	PRINT @path
	
	SET @error_message = 'creating a file system object.'
	EXECUTE @hr = sp_OACreate 'Scripting.FileSystemObject' , @file_system_object OUT
	
	IF @hr = 0
	BEGIN
		SET @error_message = 'creating ' + @routine_name + '.'		
		EXECUTE @hr = sp_OAMethod @file_system_object, 'CreateTextFile' , @text_stream OUT, @path, 2, True
	END
	
	IF @hr = 0
	BEGIN
	    SET @error_message = 'writing ' + @routine_name + '.'	    
		EXECUTE @hr = sp_OAMethod @text_stream, 'Write', Null, @new_routine_definition
	END
		
	IF @hr = 0
	BEGIN
		SET @error_message = 'closing ' + @routine_name + '.'
		PRINT @error_message
		EXECUTE @hr = sp_OAMethod @text_stream, 'Close'
	END


	IF @hr <> 0
	BEGIN
	    /*
		DECLARE 
			@source VARCHAR(255),
			@description VARCHAR(255),
			@help_file VARCHAR(255),
			@help_id INT
		
		EXECUTE sp_OAGetErrorInfo  @error_object, 
			@source output,@description output,@help_file output,@help_id output
			
		Select @strErrorMessage='Error whilst '
				+coalesce(@strErrorMessage,'doing something')
				+', '+coalesce(@Description,'')
		raiserror (@strErrorMessage,16,1)
		*/
		SET @error_message = 'There is an error in ' + @error_message
		print @error_message
		raiserror (@error_message,16,1)
	END

	EXECUTE  sp_OADestroy @text_stream
	EXECUTE sp_OADestroy @text_stream
   
   SET @current_row = @current_row + 1
   
END

