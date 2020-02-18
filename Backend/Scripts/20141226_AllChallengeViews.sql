CREATE VIEW [dbo].[AllChallengeViews]
AS
SELECT	c.ChallengeId, 
		c.Name, 
		c.Type, 
		c.Description, 
		c.Count, 
		c.DueDate, 
		c.[Order], 
		c.FrequencyCount, 
		c.FrequencyValue, 
		c.FrequencyType, 
		c.Status, 
		c.CreateDate, 
		c.CreatorId,
		u.FirstName As CreatorFirstName,
		u.LastName As CreatorLastName,
		u.UserName As CreatorUserName,
		(Select Count(*) From ChallengeGroups Where ChallengeId = c.ChallengeId) ChallengeGroupCount,
		(Select Count(*) From Deeds Where ChallengeId = c.ChallengeId) DeedCount,
		(Select Count(*) From Deeds Where ChallengeId = c.ChallengeId And DeedDate<=c.DueDate) ValidDeedCount,
		(Select Count(Distinct UserId) From GroupMembers Where GroupId In (Select GroupId From ChallengeGroups Where ChallengeId = c.ChallengeId)) As ChallengeUserCount
FROM	dbo.Challenges c Inner Join AspNetUsers u On c.CreatorId = u.Id
GO
