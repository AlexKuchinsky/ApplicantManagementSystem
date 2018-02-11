SELECT U.id AS 'User ID', U.name AS 'User Name',D.id AS 'Department ID', D.name AS 'Department Name'
FROM Users U INNER JOIN Departments D ON U.d_id = D.id

SELECT U.id AS 'User ID', U.name AS 'User Name',D.id AS 'Department ID', D.name AS 'Department Name'
FROM Users U INNER JOIN Departments D ON U.d_id = D.id
WHERE D.id between 1 and 2
