CREATE PROCEDURE SP_GetUserViews
	@PageIndex Int =1,
	@PageSize Int =20,
	@Status Nvarchar(Max) = 'All',
	@Search Nvarchar(Max) = '',
	@OrderBy Nvarchar(Max)= 'CreateDate',
	@Count Int OUTPUT
AS
BEGIN
	Declare @Start INT
    Declare @End INT
	SET @Start = (@PageSize * @PageIndex) - (@PageSize - 1);
	SET @End = (@PageSize * @PageIndex)
	Select @Count = Count(*) From AllUserViews
	;WITH oUserViews AS
	(
		SELECT ROW_NUMBER() OVER (ORDER BY @OrderBy) AS RowNumber, *
		FROM AllUserViews
	)
	SELECT * FROM oUserViews WHERE RowNumber BETWEEN @Start AND @End
End