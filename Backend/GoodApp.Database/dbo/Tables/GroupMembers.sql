CREATE TABLE [dbo].[GroupMembers] (
    [GroupId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]  NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_dbo.GroupMembers] PRIMARY KEY CLUSTERED ([GroupId] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.GroupMembers_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.GroupMembers_dbo.Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([GroupId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_GroupId]
    ON [dbo].[GroupMembers]([GroupId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[GroupMembers]([UserId] ASC);

