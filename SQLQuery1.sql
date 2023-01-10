USE Polyclinic1
GO

CREATE PROCEDURE NumberRceptionsPerDayByEmployees
	@Recept_count int
AS
	SELECT A.DateOfReception, E.Surname, A.COUNT_RECEPTION
	FROM 
	(SELECT R.DateOfReception, R.EmployeeID, COUNT(R.EmployeeID) COUNT_RECEPTION
	FROM Reception R
	GROUP BY R.DateOfReception, R.EmployeeID
	HAVING COUNT(R.EmployeeID) > @Recept_count) A
	JOIN Employee E
	ON E.EmployeeID = A.EmployeeID
GO

CREATE PROCEDURE NumberRceptionsPerDayByEmployees2
	@Recept_count int
AS
	SELECT A.DateOfReception, E.Surname, A.COUNT_RECEPTION
	FROM 
	(SELECT R.DateOfReception, R.EmployeeID, COUNT(R.EmployeeID) COUNT_RECEPTION
	FROM Reception R
	GROUP BY R.DateOfReception, R.EmployeeID
	HAVING COUNT(R.EmployeeID) = @Recept_count) A
	JOIN Employee E
	ON E.EmployeeID = A.EmployeeID
GO

