
/*------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------*/

-- <Tables> --------------------------------------------------------------------

CREATE TABLE [dbo].[GlobalSetting] (
	 [global_setting_id] uniqueidentifier NOT NULL
    ,[name] nvarchar(100) NOT NULL
    ,[value] nvarchar(max) NULL
    
  ,CONSTRAINT [PK_GlobalSetting] PRIMARY KEY CLUSTERED 
  (
	  [global_setting_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Account] (
	 [account_id] uniqueidentifier NOT NULL
    ,[email] nvarchar(250) NOT NULL
    ,[password] nvarchar(250) NOT NULL
    ,[password_salt] nvarchar(50) NOT NULL
    ,[disabled] bit NOT NULL
    ,[api_key] nvarchar(50) NOT NULL
    ,[api_secret] nvarchar(50) NOT NULL
    ,[first_name] nvarchar(50) NULL
    ,[last_name] nvarchar(50) NULL
    ,[entitlements] nvarchar(250) NULL
    ,[password_reset_token] nvarchar(50) NULL
    ,[password_reset_utc] datetimeoffset(0) NULL
    ,[push_ios] nvarchar(100) NULL
    ,[push_google] nvarchar(100) NULL
    ,[push_microsoft] nvarchar(100) NULL
    ,[last_login_utc] datetimeoffset(0) NULL
    ,[last_login_platform] nvarchar(250) NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
  (
	  [account_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Product] (
	 [product_id] uniqueidentifier NOT NULL
    ,[product_name] nvarchar(250) NOT NULL
    ,[product_owner_id] uniqueidentifier NOT NULL
    ,[product_description] nvarchar(max) NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
  (
	  [product_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Platform] (
	 [platform_id] uniqueidentifier NOT NULL
    ,[platform_name] nvarchar(250) NOT NULL
    ,[bitness] int NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_Platform] PRIMARY KEY CLUSTERED 
  (
	  [platform_id] ASC
  )
)

GO


CREATE TABLE [dbo].[ProductVersion] (
	 [product_version_id] uniqueidentifier NOT NULL
    ,[product_id] uniqueidentifier NOT NULL
    ,[version] nvarchar(250) NOT NULL
    ,[release_date_utc] datetimeoffset(0) NULL
    ,[end_of_life_date_utc] datetimeoffset(0) NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_ProductVersion] PRIMARY KEY CLUSTERED 
  (
	  [product_version_id] ASC
  )
)

GO


CREATE TABLE [dbo].[ProductVersionPlatform] (
	 [product_version_platform_id] uniqueidentifier NOT NULL
    ,[product_version_id] uniqueidentifier NOT NULL
    ,[platform_id] uniqueidentifier NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_ProductVersionPlatform] PRIMARY KEY CLUSTERED 
  (
	  [product_version_platform_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Ticket] (
	 [ticket_id] uniqueidentifier NOT NULL
    ,[reported_by_id] uniqueidentifier NOT NULL
    ,[assigned_to_id] uniqueidentifier NULL
    ,[ticket_type] int NOT NULL
    ,[ticket_status] int NOT NULL
    ,[opened_on_utc] datetimeoffset(0) NOT NULL
    ,[closed_on_utc] datetimeoffset(0) NULL
    ,[ticket_title] nvarchar(250) NOT NULL
    ,[ticket_description] nvarchar(max) NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED 
  (
	  [ticket_id] ASC
  )
)

GO


CREATE TABLE [dbo].[AffectedProduct] (
	 [affected_product_id] uniqueidentifier NOT NULL
    ,[ticket_id] uniqueidentifier NOT NULL
    ,[product_id] uniqueidentifier NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_AffectedProduct] PRIMARY KEY CLUSTERED 
  (
	  [affected_product_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Commit] (
	 [commit_id] uniqueidentifier NOT NULL
    ,[commit_ref] nvarchar(64) NOT NULL
    ,[commit_author_name] nvarchar(256) NOT NULL
    ,[commit_author_email] nvarchar(256) NOT NULL
    ,[commit_message_decoded_utc] datetimeoffset(0) NULL
    ,[commit_message] nvarchar(max) NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
  ,CONSTRAINT [PK_Commit] PRIMARY KEY CLUSTERED 
  (
	  [commit_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Asset] (
	 [asset_id] uniqueidentifier NOT NULL
    ,[type] int NOT NULL
    ,[available] bit NOT NULL
    ,[resize_required] bit NOT NULL
    ,[encode_required] bit NOT NULL
    ,[resize_processing] bit NOT NULL
    ,[encode_processing] bit NOT NULL
    ,[thumb_small_dimensions] nvarchar(10) NULL
    ,[thumb_medium_dimensions] nvarchar(10) NULL
    ,[thumb_large_dimensions] nvarchar(10) NULL
    ,[resize_status] nvarchar(50) NULL
    ,[resize_attempts] int NOT NULL
    ,[resize_attempt_utc] datetimeoffset(0) NULL
    ,[encode_identifier] nvarchar(50) NULL
    ,[encode_status] nvarchar(50) NULL
    ,[raw_url] nvarchar(512) NULL
    ,[public_url] nvarchar(512) NULL
    ,[thumb_small_url] nvarchar(512) NULL
    ,[thumb_medium_url] nvarchar(512) NULL
    ,[thumb_large_url] nvarchar(512) NULL
    ,[encode_log] nvarchar(max) NULL
    ,[resize_log] nvarchar(max) NULL
    ,[dependencies] int NOT NULL
    ,[encode_attempts] int NOT NULL
    ,[encode_attempt_utc] datetimeoffset(0) NULL
    ,[resize_mode] nvarchar(20) NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
  ,CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED 
  (
	  [asset_id] ASC
  )
)

GO


-- </Tables> --------------------------------------------------------------------


-- <Procedures> --------------------------------------------------------------------

CREATE PROCEDURE [dbo].[spIndex_InvalidateAll]
AS

   UPDATE [dbo].[Account] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[Product] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[Platform] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[ProductVersion] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[ProductVersionPlatform] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[Ticket] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[AffectedProduct] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'


GO

CREATE PROCEDURE [dbo].[spIndexHydrate_InvalidateAll]
AS

   UPDATE [dbo].[Account] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[Product] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[Platform] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[ProductVersion] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[ProductVersionPlatform] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[Ticket] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[AffectedProduct] SET [sync_hydrate_utc] = NULL


GO


CREATE PROCEDURE [dbo].[spIndex_InvalidateAggregates]
AS

	UPDATE [dbo].[Product] SET [sync_success_utc] = NULL

GO


CREATE PROCEDURE [dbo].[spIndexHydrate_InvalidateAggregates]
AS

	UPDATE [dbo].[Product] SET [sync_hydrate_utc] = NULL

GO

CREATE PROCEDURE [dbo].[spIndex_Status]
AS

   SELECT 'Pending Items' as [Pending Items]

      ,(select count(1) from [dbo].[Account] where  [sync_success_utc] IS NULL) as [Account - 10]

      ,(select count(1) from [dbo].[Product] where  [sync_success_utc] IS NULL) as [Product - 20]

      ,(select count(1) from [dbo].[Platform] where  [sync_success_utc] IS NULL) as [Platform - 30]

      ,(select count(1) from [dbo].[ProductVersion] where  [sync_success_utc] IS NULL) as [ProductVersion - 40]

      ,(select count(1) from [dbo].[Ticket] where  [sync_success_utc] IS NULL) as [Ticket - 50]

      ,(select count(1) from [dbo].[ProductVersionPlatform] where  [sync_success_utc] IS NULL) as [ProductVersionPlatform - 60]

      ,(select count(1) from [dbo].[AffectedProduct] where  [sync_success_utc] IS NULL) as [AffectedProduct - 60]

         

GO

CREATE PROCEDURE [dbo].[spIndexHydrate_Status]
AS

   SELECT 'Pending Items' as [Pending Items]

      ,(select count(1) from [dbo].[Account] where  [sync_hydrate_utc] IS NULL) as [Account - 10]

      ,(select count(1) from [dbo].[Product] where  [sync_hydrate_utc] IS NULL) as [Product - 20]

      ,(select count(1) from [dbo].[Platform] where  [sync_hydrate_utc] IS NULL) as [Platform - 30]

      ,(select count(1) from [dbo].[ProductVersion] where  [sync_hydrate_utc] IS NULL) as [ProductVersion - 40]

      ,(select count(1) from [dbo].[Ticket] where  [sync_hydrate_utc] IS NULL) as [Ticket - 50]

      ,(select count(1) from [dbo].[ProductVersionPlatform] where  [sync_hydrate_utc] IS NULL) as [ProductVersionPlatform - 60]

      ,(select count(1) from [dbo].[AffectedProduct] where  [sync_hydrate_utc] IS NULL) as [AffectedProduct - 60]

         

GO




CREATE PROCEDURE [dbo].[spAccount_SyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50)
AS
  SELECT [account_id]
  FROM [dbo].[Account]
  WHERE [sync_success_utc] IS NULL OR [deleted_utc] > [sync_success_utc]  OR [updated_utc] > [sync_success_utc]
  AND ISNULL([sync_agent],'') = ISNULL(@sync_agent,'')
  ORDER BY  -- oldest attempt, not attempted, failed -> then by change date  
	CASE WHEN NOT [sync_attempt_utc] IS NULL AND DATEDIFF(second,[sync_attempt_utc], GETUTCDATE()) > @allowableSecondsToProcessIndex  
			THEN 0 -- oldest in queue
		WHEN [sync_attempt_utc] IS NULL 
			THEN 1  -- synch is null , freshly invalidated 
		ELSE  2-- recently failed
	END asc
	,[sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spAccount_SyncUpdate]  
	 @account_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_success_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)  
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNCH DATE
		UPDATE [dbo].[Account]
		SET [sync_success_utc] = @sync_success_utc
			,[sync_attempt_utc] = NULL
			,[sync_invalid_utc] = NULL
			,[sync_log] = @sync_log
		WHERE [account_id] = @account_id
		AND [sync_success_utc] IS NULL
		AND (([sync_invalid_utc] IS NULL) OR ([sync_invalid_utc] <= @sync_success_utc))
	END
	ELSE
	BEGIN
		-- ON FAILED, SET SYNCH "ATTEMPT" DATE
		UPDATE [dbo].[Account]
		SET [sync_attempt_utc] = GETUTCDATE()
			,[sync_log] = @sync_log
		WHERE [account_id] = @account_id
		AND [sync_success_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spAccount_HydrateSyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50) -- not used yet
AS
  SELECT [account_id]
  FROM [dbo].[Account]
  WHERE [sync_hydrate_utc] IS NULL
  ORDER BY [sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spAccount_HydrateSyncUpdate]  
	 @account_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_hydrate_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)   -- not used yet
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNC DATE
		UPDATE [dbo].[Account]
		SET [sync_hydrate_utc] = @sync_hydrate_utc
		WHERE [account_id] = @account_id
		AND [sync_hydrate_utc] IS NULL
	END
	ELSE
	BEGIN
		-- ON FAILED, ADD TO LOG
		UPDATE [dbo].[Account]
		SET [sync_log] = @sync_log
		WHERE [account_id] = @account_id
		AND [sync_hydrate_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spProduct_SyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50)
AS
  SELECT [product_id]
  FROM [dbo].[Product]
  WHERE [sync_success_utc] IS NULL OR [deleted_utc] > [sync_success_utc]  OR [updated_utc] > [sync_success_utc]
  AND ISNULL([sync_agent],'') = ISNULL(@sync_agent,'')
  ORDER BY  -- oldest attempt, not attempted, failed -> then by change date  
	CASE WHEN NOT [sync_attempt_utc] IS NULL AND DATEDIFF(second,[sync_attempt_utc], GETUTCDATE()) > @allowableSecondsToProcessIndex  
			THEN 0 -- oldest in queue
		WHEN [sync_attempt_utc] IS NULL 
			THEN 1  -- synch is null , freshly invalidated 
		ELSE  2-- recently failed
	END asc
	,[sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spProduct_SyncUpdate]  
	 @product_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_success_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)  
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNCH DATE
		UPDATE [dbo].[Product]
		SET [sync_success_utc] = @sync_success_utc
			,[sync_attempt_utc] = NULL
			,[sync_invalid_utc] = NULL
			,[sync_log] = @sync_log
		WHERE [product_id] = @product_id
		AND [sync_success_utc] IS NULL
		AND (([sync_invalid_utc] IS NULL) OR ([sync_invalid_utc] <= @sync_success_utc))
	END
	ELSE
	BEGIN
		-- ON FAILED, SET SYNCH "ATTEMPT" DATE
		UPDATE [dbo].[Product]
		SET [sync_attempt_utc] = GETUTCDATE()
			,[sync_log] = @sync_log
		WHERE [product_id] = @product_id
		AND [sync_success_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spProduct_HydrateSyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50) -- not used yet
AS
  SELECT [product_id]
  FROM [dbo].[Product]
  WHERE [sync_hydrate_utc] IS NULL
  ORDER BY [sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spProduct_HydrateSyncUpdate]  
	 @product_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_hydrate_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)   -- not used yet
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNC DATE
		UPDATE [dbo].[Product]
		SET [sync_hydrate_utc] = @sync_hydrate_utc
		WHERE [product_id] = @product_id
		AND [sync_hydrate_utc] IS NULL
	END
	ELSE
	BEGIN
		-- ON FAILED, ADD TO LOG
		UPDATE [dbo].[Product]
		SET [sync_log] = @sync_log
		WHERE [product_id] = @product_id
		AND [sync_hydrate_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spPlatform_SyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50)
AS
  SELECT [platform_id]
  FROM [dbo].[Platform]
  WHERE [sync_success_utc] IS NULL OR [deleted_utc] > [sync_success_utc]  OR [updated_utc] > [sync_success_utc]
  AND ISNULL([sync_agent],'') = ISNULL(@sync_agent,'')
  ORDER BY  -- oldest attempt, not attempted, failed -> then by change date  
	CASE WHEN NOT [sync_attempt_utc] IS NULL AND DATEDIFF(second,[sync_attempt_utc], GETUTCDATE()) > @allowableSecondsToProcessIndex  
			THEN 0 -- oldest in queue
		WHEN [sync_attempt_utc] IS NULL 
			THEN 1  -- synch is null , freshly invalidated 
		ELSE  2-- recently failed
	END asc
	,[sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spPlatform_SyncUpdate]  
	 @platform_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_success_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)  
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNCH DATE
		UPDATE [dbo].[Platform]
		SET [sync_success_utc] = @sync_success_utc
			,[sync_attempt_utc] = NULL
			,[sync_invalid_utc] = NULL
			,[sync_log] = @sync_log
		WHERE [platform_id] = @platform_id
		AND [sync_success_utc] IS NULL
		AND (([sync_invalid_utc] IS NULL) OR ([sync_invalid_utc] <= @sync_success_utc))
	END
	ELSE
	BEGIN
		-- ON FAILED, SET SYNCH "ATTEMPT" DATE
		UPDATE [dbo].[Platform]
		SET [sync_attempt_utc] = GETUTCDATE()
			,[sync_log] = @sync_log
		WHERE [platform_id] = @platform_id
		AND [sync_success_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spPlatform_HydrateSyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50) -- not used yet
AS
  SELECT [platform_id]
  FROM [dbo].[Platform]
  WHERE [sync_hydrate_utc] IS NULL
  ORDER BY [sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spPlatform_HydrateSyncUpdate]  
	 @platform_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_hydrate_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)   -- not used yet
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNC DATE
		UPDATE [dbo].[Platform]
		SET [sync_hydrate_utc] = @sync_hydrate_utc
		WHERE [platform_id] = @platform_id
		AND [sync_hydrate_utc] IS NULL
	END
	ELSE
	BEGIN
		-- ON FAILED, ADD TO LOG
		UPDATE [dbo].[Platform]
		SET [sync_log] = @sync_log
		WHERE [platform_id] = @platform_id
		AND [sync_hydrate_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spProductVersion_SyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50)
AS
  SELECT [product_version_id]
  FROM [dbo].[ProductVersion]
  WHERE [sync_success_utc] IS NULL OR [deleted_utc] > [sync_success_utc]  OR [updated_utc] > [sync_success_utc]
  AND ISNULL([sync_agent],'') = ISNULL(@sync_agent,'')
  ORDER BY  -- oldest attempt, not attempted, failed -> then by change date  
	CASE WHEN NOT [sync_attempt_utc] IS NULL AND DATEDIFF(second,[sync_attempt_utc], GETUTCDATE()) > @allowableSecondsToProcessIndex  
			THEN 0 -- oldest in queue
		WHEN [sync_attempt_utc] IS NULL 
			THEN 1  -- synch is null , freshly invalidated 
		ELSE  2-- recently failed
	END asc
	,[sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spProductVersion_SyncUpdate]  
	 @product_version_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_success_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)  
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNCH DATE
		UPDATE [dbo].[ProductVersion]
		SET [sync_success_utc] = @sync_success_utc
			,[sync_attempt_utc] = NULL
			,[sync_invalid_utc] = NULL
			,[sync_log] = @sync_log
		WHERE [product_version_id] = @product_version_id
		AND [sync_success_utc] IS NULL
		AND (([sync_invalid_utc] IS NULL) OR ([sync_invalid_utc] <= @sync_success_utc))
	END
	ELSE
	BEGIN
		-- ON FAILED, SET SYNCH "ATTEMPT" DATE
		UPDATE [dbo].[ProductVersion]
		SET [sync_attempt_utc] = GETUTCDATE()
			,[sync_log] = @sync_log
		WHERE [product_version_id] = @product_version_id
		AND [sync_success_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spProductVersion_HydrateSyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50) -- not used yet
AS
  SELECT [product_version_id]
  FROM [dbo].[ProductVersion]
  WHERE [sync_hydrate_utc] IS NULL
  ORDER BY [sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spProductVersion_HydrateSyncUpdate]  
	 @product_version_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_hydrate_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)   -- not used yet
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNC DATE
		UPDATE [dbo].[ProductVersion]
		SET [sync_hydrate_utc] = @sync_hydrate_utc
		WHERE [product_version_id] = @product_version_id
		AND [sync_hydrate_utc] IS NULL
	END
	ELSE
	BEGIN
		-- ON FAILED, ADD TO LOG
		UPDATE [dbo].[ProductVersion]
		SET [sync_log] = @sync_log
		WHERE [product_version_id] = @product_version_id
		AND [sync_hydrate_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spProductVersionPlatform_SyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50)
AS
  SELECT [product_version_platform_id]
  FROM [dbo].[ProductVersionPlatform]
  WHERE [sync_success_utc] IS NULL OR [deleted_utc] > [sync_success_utc]  OR [updated_utc] > [sync_success_utc]
  AND ISNULL([sync_agent],'') = ISNULL(@sync_agent,'')
  ORDER BY  -- oldest attempt, not attempted, failed -> then by change date  
	CASE WHEN NOT [sync_attempt_utc] IS NULL AND DATEDIFF(second,[sync_attempt_utc], GETUTCDATE()) > @allowableSecondsToProcessIndex  
			THEN 0 -- oldest in queue
		WHEN [sync_attempt_utc] IS NULL 
			THEN 1  -- synch is null , freshly invalidated 
		ELSE  2-- recently failed
	END asc
	,[sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spProductVersionPlatform_SyncUpdate]  
	 @product_version_platform_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_success_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)  
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNCH DATE
		UPDATE [dbo].[ProductVersionPlatform]
		SET [sync_success_utc] = @sync_success_utc
			,[sync_attempt_utc] = NULL
			,[sync_invalid_utc] = NULL
			,[sync_log] = @sync_log
		WHERE [product_version_platform_id] = @product_version_platform_id
		AND [sync_success_utc] IS NULL
		AND (([sync_invalid_utc] IS NULL) OR ([sync_invalid_utc] <= @sync_success_utc))
	END
	ELSE
	BEGIN
		-- ON FAILED, SET SYNCH "ATTEMPT" DATE
		UPDATE [dbo].[ProductVersionPlatform]
		SET [sync_attempt_utc] = GETUTCDATE()
			,[sync_log] = @sync_log
		WHERE [product_version_platform_id] = @product_version_platform_id
		AND [sync_success_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spProductVersionPlatform_HydrateSyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50) -- not used yet
AS
  SELECT [product_version_platform_id]
  FROM [dbo].[ProductVersionPlatform]
  WHERE [sync_hydrate_utc] IS NULL
  ORDER BY [sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spProductVersionPlatform_HydrateSyncUpdate]  
	 @product_version_platform_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_hydrate_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)   -- not used yet
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNC DATE
		UPDATE [dbo].[ProductVersionPlatform]
		SET [sync_hydrate_utc] = @sync_hydrate_utc
		WHERE [product_version_platform_id] = @product_version_platform_id
		AND [sync_hydrate_utc] IS NULL
	END
	ELSE
	BEGIN
		-- ON FAILED, ADD TO LOG
		UPDATE [dbo].[ProductVersionPlatform]
		SET [sync_log] = @sync_log
		WHERE [product_version_platform_id] = @product_version_platform_id
		AND [sync_hydrate_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spTicket_SyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50)
AS
  SELECT [ticket_id]
  FROM [dbo].[Ticket]
  WHERE [sync_success_utc] IS NULL OR [deleted_utc] > [sync_success_utc]  OR [updated_utc] > [sync_success_utc]
  AND ISNULL([sync_agent],'') = ISNULL(@sync_agent,'')
  ORDER BY  -- oldest attempt, not attempted, failed -> then by change date  
	CASE WHEN NOT [sync_attempt_utc] IS NULL AND DATEDIFF(second,[sync_attempt_utc], GETUTCDATE()) > @allowableSecondsToProcessIndex  
			THEN 0 -- oldest in queue
		WHEN [sync_attempt_utc] IS NULL 
			THEN 1  -- synch is null , freshly invalidated 
		ELSE  2-- recently failed
	END asc
	,[sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spTicket_SyncUpdate]  
	 @ticket_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_success_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)  
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNCH DATE
		UPDATE [dbo].[Ticket]
		SET [sync_success_utc] = @sync_success_utc
			,[sync_attempt_utc] = NULL
			,[sync_invalid_utc] = NULL
			,[sync_log] = @sync_log
		WHERE [ticket_id] = @ticket_id
		AND [sync_success_utc] IS NULL
		AND (([sync_invalid_utc] IS NULL) OR ([sync_invalid_utc] <= @sync_success_utc))
	END
	ELSE
	BEGIN
		-- ON FAILED, SET SYNCH "ATTEMPT" DATE
		UPDATE [dbo].[Ticket]
		SET [sync_attempt_utc] = GETUTCDATE()
			,[sync_log] = @sync_log
		WHERE [ticket_id] = @ticket_id
		AND [sync_success_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spTicket_HydrateSyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50) -- not used yet
AS
  SELECT [ticket_id]
  FROM [dbo].[Ticket]
  WHERE [sync_hydrate_utc] IS NULL
  ORDER BY [sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spTicket_HydrateSyncUpdate]  
	 @ticket_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_hydrate_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)   -- not used yet
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNC DATE
		UPDATE [dbo].[Ticket]
		SET [sync_hydrate_utc] = @sync_hydrate_utc
		WHERE [ticket_id] = @ticket_id
		AND [sync_hydrate_utc] IS NULL
	END
	ELSE
	BEGIN
		-- ON FAILED, ADD TO LOG
		UPDATE [dbo].[Ticket]
		SET [sync_log] = @sync_log
		WHERE [ticket_id] = @ticket_id
		AND [sync_hydrate_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spAffectedProduct_SyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50)
AS
  SELECT [affected_product_id]
  FROM [dbo].[AffectedProduct]
  WHERE [sync_success_utc] IS NULL OR [deleted_utc] > [sync_success_utc]  OR [updated_utc] > [sync_success_utc]
  AND ISNULL([sync_agent],'') = ISNULL(@sync_agent,'')
  ORDER BY  -- oldest attempt, not attempted, failed -> then by change date  
	CASE WHEN NOT [sync_attempt_utc] IS NULL AND DATEDIFF(second,[sync_attempt_utc], GETUTCDATE()) > @allowableSecondsToProcessIndex  
			THEN 0 -- oldest in queue
		WHEN [sync_attempt_utc] IS NULL 
			THEN 1  -- synch is null , freshly invalidated 
		ELSE  2-- recently failed
	END asc
	,[sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spAffectedProduct_SyncUpdate]  
	 @affected_product_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_success_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)  
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNCH DATE
		UPDATE [dbo].[AffectedProduct]
		SET [sync_success_utc] = @sync_success_utc
			,[sync_attempt_utc] = NULL
			,[sync_invalid_utc] = NULL
			,[sync_log] = @sync_log
		WHERE [affected_product_id] = @affected_product_id
		AND [sync_success_utc] IS NULL
		AND (([sync_invalid_utc] IS NULL) OR ([sync_invalid_utc] <= @sync_success_utc))
	END
	ELSE
	BEGIN
		-- ON FAILED, SET SYNCH "ATTEMPT" DATE
		UPDATE [dbo].[AffectedProduct]
		SET [sync_attempt_utc] = GETUTCDATE()
			,[sync_log] = @sync_log
		WHERE [affected_product_id] = @affected_product_id
		AND [sync_success_utc] IS NULL
	END  
END

GO

CREATE PROCEDURE [dbo].[spAffectedProduct_HydrateSyncGetInvalid]
	@allowableSecondsToProcessIndex int
    ,@sync_agent nvarchar(50) -- not used yet
AS
  SELECT [affected_product_id]
  FROM [dbo].[AffectedProduct]
  WHERE [sync_hydrate_utc] IS NULL
  ORDER BY [sync_invalid_utc] asc

GO

CREATE PROCEDURE [dbo].[spAffectedProduct_HydrateSyncUpdate]  
	 @affected_product_id uniqueidentifier,  
	 @sync_success bit,  
	 @sync_hydrate_utc datetimeoffset(0),  
	 @sync_log nvarchar(MAX)   -- not used yet
AS  
BEGIN 
	IF (@sync_success = 1)   
	BEGIN  
		-- ON SUCCESSFUL, SET SYNC DATE
		UPDATE [dbo].[AffectedProduct]
		SET [sync_hydrate_utc] = @sync_hydrate_utc
		WHERE [affected_product_id] = @affected_product_id
		AND [sync_hydrate_utc] IS NULL
	END
	ELSE
	BEGIN
		-- ON FAILED, ADD TO LOG
		UPDATE [dbo].[AffectedProduct]
		SET [sync_log] = @sync_log
		WHERE [affected_product_id] = @affected_product_id
		AND [sync_hydrate_utc] IS NULL
	END  
END

GO

-- <Procedures> --------------------------------------------------------------------


-- <Foreign Keys> --------------------------------------------------------------------

ALTER TABLE [dbo].[AffectedProduct] WITH CHECK ADD  CONSTRAINT [FK_AffectedProduct_Ticket_ticket_id] FOREIGN KEY([ticket_id])
REFERENCES [dbo].[Ticket] ([ticket_id])
GO

ALTER TABLE [dbo].[AffectedProduct] WITH CHECK ADD  CONSTRAINT [FK_AffectedProduct_Product_product_id] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO

ALTER TABLE [dbo].[Ticket] WITH CHECK ADD  CONSTRAINT [FK_Ticket_Account_reported_by_id] FOREIGN KEY([reported_by_id])
REFERENCES [dbo].[Account] ([account_id])
GO

ALTER TABLE [dbo].[Ticket] WITH CHECK ADD  CONSTRAINT [FK_Ticket_Account_assigned_to_id] FOREIGN KEY([assigned_to_id])
REFERENCES [dbo].[Account] ([account_id])
GO

ALTER TABLE [dbo].[ProductVersionPlatform] WITH CHECK ADD  CONSTRAINT [FK_ProductVersionPlatform_ProductVersion_product_version_id] FOREIGN KEY([product_version_id])
REFERENCES [dbo].[ProductVersion] ([product_version_id])
GO

ALTER TABLE [dbo].[ProductVersionPlatform] WITH CHECK ADD  CONSTRAINT [FK_ProductVersionPlatform_Platform_platform_id] FOREIGN KEY([platform_id])
REFERENCES [dbo].[Platform] ([platform_id])
GO

ALTER TABLE [dbo].[ProductVersion] WITH CHECK ADD  CONSTRAINT [FK_ProductVersion_Product_product_id] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO

ALTER TABLE [dbo].[Product] WITH CHECK ADD  CONSTRAINT [FK_Product_Account_product_owner_id] FOREIGN KEY([product_owner_id])
REFERENCES [dbo].[Account] ([account_id])
GO

-- </Foreign Keys> --------------------------------------------------------------------


-- <Unique Keys> --------------------------------------------------------------------


IF OBJECT_ID('dbo.UK_account_key', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[Account] 
		DROP CONSTRAINT UK_account_key
END
ALTER TABLE [dbo].[Account] 
   ADD CONSTRAINT UK_account_key UNIQUE ([api_key]); 
GO

-- </Unique Keys> --------------------------------------------------------------------


