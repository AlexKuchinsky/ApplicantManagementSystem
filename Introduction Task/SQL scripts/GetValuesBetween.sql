SELECT ones.n + 10*tens.n + 100*hundreds.n + 1000*thousands.n
FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) ones(n),
     (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) tens(n),
     (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) hundreds(n),
     (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) thousands(n)
WHERE ones.n + 10*tens.n + 100*hundreds.n + 1000*thousands.n BETWEEN 345 AND 374
ORDER BY 1