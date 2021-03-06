/*
    This script was generated by SQL Change Automation to help provide object-level history. This script should never be edited manually.
    For more information see: https://www.red-gate.com/sca/dev/offline-schema-model
*/

CREATE TABLE [dbo].[series]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[title] [varchar] (200) NOT NULL,
[description] [varchar] (2000) NOT NULL,
[release_date] [datetime] NOT NULL,
[added_date] [datetime] NOT NULL,
[ended] [bit] NOT NULL CONSTRAINT [DF_series_ended] DEFAULT ((0)),
[owned] [bit] NOT NULL CONSTRAINT [DF_series_owned] DEFAULT ((0)),
[anime] [bit] NOT NULL CONSTRAINT [DF_series_anime] DEFAULT ((0))
)
GO
ALTER TABLE [dbo].[series] ADD CONSTRAINT [PK_series_id] PRIMARY KEY CLUSTERED ([id])
GO
CREATE NONCLUSTERED INDEX [IX_series_release_date] ON [dbo].[series] ([release_date])
GO
