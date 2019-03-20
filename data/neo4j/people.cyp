CREATE (anders:Person {name: "Anders"}),
(dilshad:Person {name: "Dilshad"}),
(cesar:Person {name: "Cesar"}),
(becky:Person {name: "Becky"}),
(filipa:Person {name: "Filipa"}),
(george:Person {name: "George"}),
(anders)-[:KNOWS]->(dilshad),
(anders)-[:KNOWS]->(cesar),
(anders)-[:KNOWS]->(becky),
(dilshad)-[:KNOWS]->(filipa),
(cesar)-[:KNOWS]->(george),
(becky)-[:KNOWS]->(george)