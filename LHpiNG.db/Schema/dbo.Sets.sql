CREATE TABLE [dbo].[Sets] (
    [TLA]                 NCHAR (3)      NOT NULL,
    [Name]                NVARCHAR (255) NOT NULL,
    [MagicAlbumId]        INT            NULL,
    [CardCount]           INT            NULL,
    [TokenCount]          INT            NULL,
    [NontraditionalCount] INT            NULL,
    [InsertCount]         INT            NULL,
    [ReplicaCount]        INT            NULL,
    [HasFoil]             BIT            NULL,
    [HasNonfoil]          BIT            NULL,
    PRIMARY KEY CLUSTERED ([TLA] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [MagicAlbumId]
    ON [dbo].[Sets]([MagicAlbumId] ASC);

