
/*------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------*/

-- <Foreign Keys> --------------------------------------------------------------------

GO

ALTER TABLE [dbo].[AffectedProduct] DROP  [FK_AffectedProduct_Ticket_ticket_id]

ALTER TABLE [dbo].[AffectedProduct] DROP  [FK_AffectedProduct_Product_product_id]

GO

ALTER TABLE [dbo].[Ticket] DROP  [FK_Ticket_Account_reported_by_id]

ALTER TABLE [dbo].[Ticket] DROP  [FK_Ticket_Account_assigned_to_id]

GO

ALTER TABLE [dbo].[ProductVersionPlatform] DROP  [FK_ProductVersionPlatform_ProductVersion_product_version_id]

ALTER TABLE [dbo].[ProductVersionPlatform] DROP  [FK_ProductVersionPlatform_Platform_platform_id]

GO

ALTER TABLE [dbo].[ProductVersion] DROP  [FK_ProductVersion_Product_product_id]

GO

GO

ALTER TABLE [dbo].[Product] DROP  [FK_Product_Account_product_owner_id]

GO

GO

GO

-- </Foreign Keys> --------------------------------------------------------------------



-- <Unique Keys> --------------------------------------------------------------------

IF OBJECT_ID('dbo.UK_account_key', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[Account] 
		DROP CONSTRAINT UK_account_key
END
GO


-- </Unique Keys> --------------------------------------------------------------------


-- <Tables> --------------------------------------------------------------------

DROP TABLE [dbo].[Asset]
GO

DROP TABLE [dbo].[AffectedProduct]
GO

DROP TABLE [dbo].[Ticket]
GO

DROP TABLE [dbo].[ProductVersionPlatform]
GO

DROP TABLE [dbo].[ProductVersion]
GO

DROP TABLE [dbo].[Platform]
GO

DROP TABLE [dbo].[Product]
GO

DROP TABLE [dbo].[Account]
GO

DROP TABLE [dbo].[GlobalSetting]
GO

-- </Tables> --------------------------------------------------------------------

-- <Procedures> --------------------------------------------------------------------


DROP PROCEDURE [dbo].[spAccount_SyncUpdate]
GO

DROP PROCEDURE [dbo].[spAccount_SyncGetInvalid]
GO

DROP PROCEDURE [dbo].[spAccount_HydrateSyncUpdate]
GO

DROP PROCEDURE [dbo].[spAccount_HydrateSyncGetInvalid]
GO




DROP PROCEDURE [dbo].[spProduct_SyncUpdate]
GO

DROP PROCEDURE [dbo].[spProduct_SyncGetInvalid]
GO

DROP PROCEDURE [dbo].[spProduct_HydrateSyncUpdate]
GO

DROP PROCEDURE [dbo].[spProduct_HydrateSyncGetInvalid]
GO




DROP PROCEDURE [dbo].[spPlatform_SyncUpdate]
GO

DROP PROCEDURE [dbo].[spPlatform_SyncGetInvalid]
GO

DROP PROCEDURE [dbo].[spPlatform_HydrateSyncUpdate]
GO

DROP PROCEDURE [dbo].[spPlatform_HydrateSyncGetInvalid]
GO




DROP PROCEDURE [dbo].[spProductVersion_SyncUpdate]
GO

DROP PROCEDURE [dbo].[spProductVersion_SyncGetInvalid]
GO

DROP PROCEDURE [dbo].[spProductVersion_HydrateSyncUpdate]
GO

DROP PROCEDURE [dbo].[spProductVersion_HydrateSyncGetInvalid]
GO




DROP PROCEDURE [dbo].[spProductVersionPlatform_SyncUpdate]
GO

DROP PROCEDURE [dbo].[spProductVersionPlatform_SyncGetInvalid]
GO

DROP PROCEDURE [dbo].[spProductVersionPlatform_HydrateSyncUpdate]
GO

DROP PROCEDURE [dbo].[spProductVersionPlatform_HydrateSyncGetInvalid]
GO




DROP PROCEDURE [dbo].[spTicket_SyncUpdate]
GO

DROP PROCEDURE [dbo].[spTicket_SyncGetInvalid]
GO

DROP PROCEDURE [dbo].[spTicket_HydrateSyncUpdate]
GO

DROP PROCEDURE [dbo].[spTicket_HydrateSyncGetInvalid]
GO




DROP PROCEDURE [dbo].[spAffectedProduct_SyncUpdate]
GO

DROP PROCEDURE [dbo].[spAffectedProduct_SyncGetInvalid]
GO

DROP PROCEDURE [dbo].[spAffectedProduct_HydrateSyncUpdate]
GO

DROP PROCEDURE [dbo].[spAffectedProduct_HydrateSyncGetInvalid]
GO




DROP PROCEDURE [dbo].[spIndex_InvalidateAll]
GO
DROP PROCEDURE [dbo].[spIndex_InvalidateAggregates]
GO

DROP PROCEDURE [dbo].[spIndex_Status]
GO

DROP PROCEDURE [dbo].[spIndexHydrate_InvalidateAll]
GO

DROP PROCEDURE [dbo].[spIndexHydrate_InvalidateAggregates]
GO


DROP PROCEDURE [dbo].[spIndexHydrate_Status]
GO


-- <Procedures> --------------------------------------------------------------------
