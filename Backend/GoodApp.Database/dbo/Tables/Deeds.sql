CREATE TABLE [dbo].[Deeds] (
    [DeedId]      UNIQUEIDENTIFIER NOT NULL,
    [DeedDate]    DATETIME         NOT NULL,
    [Location]    NVARCHAR (MAX)   NOT NULL,
    [Lat]         FLOAT (53)       NOT NULL,
    [Lon]         FLOAT (53)       NOT NULL,
    [Rating]      INT              NULL,
    [Comment]     NVARCHAR (MAX)   NULL,
    [ChallengeId] UNIQUEIDENTIFIER NOT NULL,
    [CreateDate]  DATETIME         NOT NULL,
    [CreatorId]   NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_dbo.Deeds] PRIMARY KEY CLUSTERED ([DeedId] ASC),
    CONSTRAINT [FK_dbo.Deeds_dbo.AspNetUsers_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_dbo.Deeds_dbo.Challenges_ChallengeId] FOREIGN KEY ([ChallengeId]) REFERENCES [dbo].[Challenges] ([ChallengeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ChallengeId]
    ON [dbo].[Deeds]([ChallengeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatorId]
    ON [dbo].[Deeds]([CreatorId] ASC);

