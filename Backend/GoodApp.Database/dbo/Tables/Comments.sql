CREATE TABLE [dbo].[Comments] (
    [CommentId]   UNIQUEIDENTIFIER NOT NULL,
    [ReferenceId] NVARCHAR (MAX)   NOT NULL,
    [UserId]      NVARCHAR (128)   NOT NULL,
    [Caption]     NVARCHAR (MAX)   NOT NULL,
    [CreateDate]  DATETIME         NOT NULL,
    [Type]        INT              NOT NULL,
    CONSTRAINT [PK_dbo.Comments] PRIMARY KEY CLUSTERED ([CommentId] ASC),
    CONSTRAINT [FK_dbo.Comments_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[Comments]([UserId] ASC);

