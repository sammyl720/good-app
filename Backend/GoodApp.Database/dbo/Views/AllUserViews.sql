

CREATE VIEW [dbo].[AllUserViews]
AS
SELECT	u.Id, 
		u.FirstName, 
		u.LastName, 
		u.PhotoPath, 
		u.CreateDate, 
		u.LastLoginDate, 
		u.Status, 
		u.Email, 
		u.EmailConfirmed, 
		u.UserName,
		reverse(stuff(reverse((Select Name + ',' From AspNetRoles Where Id In (Select RoleId From AspNetUserRoles Where UserId = u.Id))), 1, 1, '')) As RoleName,
		(Select Count(*) From GroupMembers Where UserId = u.Id) AS JoinedGroupCount,
		(Select Count(*) From Deeds Where CreatorId = u.Id) As DeedCount,
		(Select Count(*) From Deeds Where CreatorId = u.Id And DeedDate<=(Select DueDate From Challenges Where ChallengeId = Deeds.ChallengeId)) As ValidDeedCount,
		(Select Count(Distinct ChallengeId) From ChallengeGroups Where GroupId In (Select Distinct GroupId From GroupMembers Where UserId = u.Id)) As JoinedChallengeCount,
		(Select Count(Distinct UserId) From TagUsers Where DeedId In (Select DeedId From Deeds Where CreatorId = u.Id)) As TagUserCount,
		(Select Count(*) From Deeds) As TotalDeedCount,
		(Select Count(*) From Deeds Where CreatorId In (Select UserId From GroupMembers Where GroupId In(Select GroupId From GroupMembers Where UserId = u.Id))) NetworkDeedCount
FROM	dbo.AspNetUsers u
GO
