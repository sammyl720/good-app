CREATE TABLE [dbo].[TagUsers] (
    [DeedId] UNIQUEIDENTIFIER NOT NULL,
    [UserId] NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_dbo.TagUsers] PRIMARY KEY CLUSTERED ([DeedId] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.TagUsers_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TagUsers_dbo.Deeds_DeedId] FOREIGN KEY ([DeedId]) REFERENCES [dbo].[Deeds] ([DeedId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_DeedId]
    ON [dbo].[TagUsers]([DeedId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[TagUsers]([UserId] ASC);

