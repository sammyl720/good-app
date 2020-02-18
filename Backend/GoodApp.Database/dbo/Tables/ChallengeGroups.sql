CREATE TABLE [dbo].[ChallengeGroups] (
    [ChallengeId] UNIQUEIDENTIFIER NOT NULL,
    [GroupId]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.ChallengeGroups] PRIMARY KEY CLUSTERED ([ChallengeId] ASC, [GroupId] ASC),
    CONSTRAINT [FK_dbo.ChallengeGroups_dbo.Challenges_ChallengeId] FOREIGN KEY ([ChallengeId]) REFERENCES [dbo].[Challenges] ([ChallengeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ChallengeGroups_dbo.Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([GroupId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ChallengeId]
    ON [dbo].[ChallengeGroups]([ChallengeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_GroupId]
    ON [dbo].[ChallengeGroups]([GroupId] ASC);

