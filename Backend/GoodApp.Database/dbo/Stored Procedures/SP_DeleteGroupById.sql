CREATE PROCEDURE [dbo].[SP_DeleteGroupById]
	@GroupId uniqueidentifier
AS
BEGIN
	Delete From GroupMembers Where GroupId = @GroupId;
	Delete From ChallengeGroups Where GroupId = @GroupId;
	Delete From Groups Where GroupId = @GroupId;
End