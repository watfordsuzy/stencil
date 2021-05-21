-- <Tables> --------------------------------------------------------------------


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

-- <Tables> --------------------------------------------------------------------


-- <Procedures> --------------------------------------------------------------------

ALTER PROCEDURE [dbo].[spIndex_InvalidateAll]
AS

   UPDATE [dbo].[Account] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[Product] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[Platform] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[ProductVersion] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[ProductVersionPlatform] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[Ticket] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

   UPDATE [dbo].[AffectedProduct] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'


GO

ALTER PROCEDURE [dbo].[spIndexHydrate_InvalidateAll]
AS

   UPDATE [dbo].[Account] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[Product] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[Platform] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[ProductVersion] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[ProductVersionPlatform] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[Ticket] SET [sync_hydrate_utc] = NULL

   UPDATE [dbo].[AffectedProduct] SET [sync_hydrate_utc] = NULL


GO


ALTER PROCEDURE [dbo].[spIndex_Status]
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

ALTER PROCEDURE [dbo].[spIndexHydrate_Status]
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

-- <Foreign Keys> --------------------------------------------------------------------
