CREATE PROCEDURE [dbo].[SP_DeleteChallengeById]
	@ChallengeId uniqueidentifier
AS
BEGIN
	Delete From ChallengeGroups Where ChallengeId = @ChallengeId;
	Delete From TagUsers Where DeedId In (Select DeedId From Deeds Where ChallengeId = @ChallengeId);
	Delete From Deeds Where ChallengeId = @ChallengeId;
 	Delete From Challenges Where ChallengeId = @ChallengeId;
End



GO


