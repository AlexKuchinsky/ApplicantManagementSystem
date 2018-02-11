SELECT 'User' as Creature,id,name AS 'Имя сущности' FROM Users
UNION
Select 'Department',id, name FROM Departments

SELECT * FROM (
	SELECT 'User' as Creature,id,name AS 'Имя сущности' FROM Users
	UNION
	SELECT 'Department',id, name FROM Departments
	)
AS temp_table WHERE id between 1 and 3;

CREATE TABLE new_table AS (
	SELECT 'User' as Creature,id,name AS 'Имя сущности' FROM Users
	UNION
	SELECT 'Department',id, name FROM Departments
);