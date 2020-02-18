CREATE VIEW [dbo].[AllDeedViews]
AS
SELECT	d.DeedId, 
		d.DeedDate, 
		d.Location, 
		d.Lat, 
		d.Lon, 
		d.Rating, 
		d.ChallengeId, 
		d.Comment, 
		d.CreateDate, 
		d.CreatorId,
		u.FirstName As CreatorFirstName,
		u.LastName As CreatorLastName,
		u.UserName As CreatorUserName,
		g.Name As ChallengeName,
		g.DueDate,
		(Select Count(Distinct UserId) From TagUsers Where DeedId = d.DeedId) As TagUserCount,
		case when d.DeedDate<=g.DueDate then cast(1 as bit) else cast(0 as bit) end As IsValid,
		(Select FirstName From AspNetUsers Where Id = t.UserId) As TagUserFirstName,
		(Select LastName From AspNetUsers Where Id = t.UserId) As TagUserLastName,
		(Select UserName From AspNetUsers Where Id = t.UserId) As TagUserName,
		t.UserId As TagUserId
FROM	dbo.Deeds d Inner Join AspNetUsers u On d.CreatorId = u.Id
		Inner Join Challenges g On g.ChallengeId = d.ChallengeId
		Left Join TagUsers t On t.DeedId = d.DeedId
GO
