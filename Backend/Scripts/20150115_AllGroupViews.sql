ALTER VIEW [dbo].[AllGroupViews]
AS
SELECT        g.GroupId, g.Name, g.Description, g.Code, g.CreateDate, g.CreatorId, u.FirstName AS CreatorFirstName, u.LastName AS CreatorLastName, u.UserName AS CreatorUserName,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.GroupMembers
                               WHERE        (GroupId = g.GroupId)) AS MembersCount,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.ChallengeGroups
                               WHERE        (GroupId = g.GroupId)) AS ChallengesCount, g.Picture
FROM            dbo.Groups AS g INNER JOIN
                         dbo.AspNetUsers AS u ON g.CreatorId = u.Id

GO


