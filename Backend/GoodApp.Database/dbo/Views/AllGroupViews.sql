

CREATE VIEW [dbo].[AllGroupViews]
AS
SELECT	g.GroupId, 
		g.Name, 
		g.Description, 
		g.Code, 
		g.CreateDate, 
		g.CreatorId,
		u.FirstName As CreatorFirstName,
		u.LastName As CreatorLastName,
		u.UserName As CreatorUserName,
		(Select Count(*) From GroupMembers Where GroupId = g.GroupId) As MembersCount,
		(Select Count(*) From ChallengeGroups Where GroupId = g.GroupId) As ChallengesCount
FROM    dbo.Groups g Inner Join AspNetUsers u On g.CreatorId = u.Id
GO
