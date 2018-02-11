SELECT Title FROM Citites ORDER BY Title DESC;
SELECT Title FROM Citites Where Number IS NOT NULL;
SELECT * FROM Citites WHERE Title LIKE '_a%a';
SELECT * FROM Citites WHERE (Number IS NOT NULL) AND ID IN (2,3);
SELECT Topic FROM Topics WHERE UserID IN (SELECT UserID FROM Topics WHERE UserID in (1,2)) AND (TopicContent LIKE '%cycling%' OR TopicContent LIKE 'cycling%' OR TopicContent LIKE '%cycling')

