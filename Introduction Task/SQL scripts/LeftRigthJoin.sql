SELECT U.id AS 'User ID', U.name AS 'User Name',D.id AS 'Department ID', D.name AS 'Department Name'
FROM Users U LEFT OUTER JOIN Departments D ON U.d_id = D.id

SELECT U.id AS 'User ID', U.name AS 'User Name',D.id AS 'Department ID', D.name AS 'Department Name'
FROM Users U RIGHT OUTER JOIN Departments D ON U.d_id = D.id