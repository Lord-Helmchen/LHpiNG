CREATE TABLE [dbo].[Expansion] (
    [Id]           INT        NOT NULL,
    [Name]         NCHAR (10) NOT NULL,
    [Abbreviation] NCHAR (3)  NULL,
    [ReleaseDate]  DATETIME   NULL,
    [IsReleased]   BIT        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

