CREATE TABLE [dbo].[Challenges] (
    [ChallengeId]    UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (MAX)   NOT NULL,
    [Type]           NVARCHAR (MAX)   NOT NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [Count]          INT              NOT NULL,
    [DueDate]        DATETIME         NOT NULL,
    [Order]          INT              NOT NULL,
    [FrequencyCount] INT              NOT NULL,
    [FrequencyValue] INT              NOT NULL,
    [FrequencyType]  NVARCHAR (MAX)   NOT NULL,
    [Status]         NVARCHAR (MAX)   NOT NULL,
    [CreateDate]     DATETIME         NOT NULL,
    [CreatorId]      NVARCHAR (128)   NOT NULL,
    [Picture]        NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.Challenges] PRIMARY KEY CLUSTERED ([ChallengeId] ASC),
    CONSTRAINT [FK_dbo.Challenges_dbo.AspNetUsers_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CreatorId]
    ON [dbo].[Challenges]([CreatorId] ASC);

