CREATE PROCEDURE [dbo].[SP_DeleteUserById]
	@UserId Nvarchar(128)
AS
BEGIN
	Delete From TagUsers Where UserId = @UserId;
	Delete From GroupMembers Where UserId = @UserId;
	Delete From Groups Where CreatorId = @UserId;
	Delete From Deeds Where CreatorId = @UserId;
	Delete From Challenges Where CreatorId = @UserId;
	Delete From AspNetUserClaims Where UserId = @UserId;
	Delete From AspNetUserLogins Where UserId = @UserId;
	Delete From AspNetUserRoles Where UserId = @UserId;
	Delete From AspNetUsers Where Id = @UserId;
End

GO


