CREATE TABLE [dbo].[Groups] (
    [GroupId]     UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (MAX)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [Code]        NVARCHAR (MAX)   NOT NULL,
    [CreateDate]  DATETIME         NOT NULL,
    [CreatorId]   NVARCHAR (128)   NOT NULL,
    [Picture]     NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.Groups] PRIMARY KEY CLUSTERED ([GroupId] ASC),
    CONSTRAINT [FK_dbo.Groups_dbo.AspNetUsers_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CreatorId]
    ON [dbo].[Groups]([CreatorId] ASC);

