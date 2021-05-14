/*CREATE DATABASE incidentdb*/


declare @dbname varchar(100);
set @dbname=DB_NAME();

if (SELECT user_access_desc FROM sys.databases WHERE name = @dbname) = 'SINGLE_USER'
BEGIN
    exec('alter database '+@dbname+' SET MULTI_USER');
	/*ALTER DATABASE CURRENT  SET MULTI_USER*/
	/*ALTER DATABASE CURRENT  SET SINGLE_USER*/
END
GO



IF (NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[incidents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	and (NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[incidents]') and xtype = 'SN')))
BEGIN 
CREATE TABLE [dbo].incidents( 
	[name] [nvarchar](255) NULL, 
	[action] [nvarchar](255) NULL, 
	[objid] [nvarchar](255) NULL, 
	[objtype] [nvarchar](255) NULL, 
	[user_id] [nvarchar](255) NULL, 
	[timestamp] [datetimeoffset] NULL,
	[id] [uniqueidentifier] NOT NULL
 CONSTRAINT [PK_GUID_incidents] PRIMARY KEY CLUSTERED 
( [id] ASC )
) ON [PRIMARY] 
END 

IF (NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[incident_handling]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	and (NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[incident_handling]') and xtype = 'SN')))
BEGIN 
CREATE TABLE [dbo].incident_handling( 
	[line_descr] [nvarchar](max) NULL, 
	[line_action] [nvarchar](max) NULL, 
	[image_path] [nvarchar](max) NULL, 
	[incident_id] [uniqueidentifier] NOT NULL, 
	[line_timestamp] [datetimeoffset] NULL,
	[id] [uniqueidentifier] NOT NULL
 CONSTRAINT [PK_GUID_incident_handling] PRIMARY KEY CLUSTERED 
( [id] ASC )
) ON [PRIMARY] 
END 

IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[incident_handling]') AND name = N'IX_incident_id')
	CREATE NONCLUSTERED INDEX [IX_incident_id] ON [dbo].[incident_handling] 
	 ([incident_id] ASC)




