CREATE PROCEDURE [dbo].[SP_ReOrderChallenge]
	@ChallengeId uniqueidentifier,
	@Diff int,
	@IsAsc bit
AS
BEGIN
	-- Diff = OriginalIndex - CurrentIndex
	Declare @Order Int
	Declare @NewOrder Int
	Select @Order = [Order] From AllChallengeViews Where ChallengeId = @ChallengeId
	IF @IsAsc =0 
	BEGIN
		Set @Diff = 0-@Diff
	END
	Set @NewOrder = @Order - @Diff	
	IF @NewOrder > @Order
	BEGIN
		Update Challenges Set [Order]= [Order]-1 Where ([Order] Between @Order And @NewOrder)
	END
	ELSE
	BEGIN
		Update Challenges Set [Order]= [Order]+1 Where ([Order] Between @NewOrder And @Order)
	END
	Update Challenges Set [Order] = @NewOrder Where ChallengeId = @ChallengeId
End