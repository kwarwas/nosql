MERGE (theMatrix:Movie {title:'The Matrix', released:1999, tagline:'Welcome to the Real World'})
MERGE (theMatrixReloaded:Movie {title:'The Matrix Reloaded', released:2003, tagline:'Free your mind'})
MERGE (theMatrixRevolutions:Movie {title:'The Matrix Revolutions', released:2003, tagline:'Everything that has a beginning has an end'})
MERGE (theDevilsAdvocate:Movie {title:"The Devil's Advocate", released:1997, tagline:'Evil has its winning ways'})
MERGE (aFewGoodMen:Movie {title:"A Few Good Men", released:1992, tagline:"In the heart of the nation's capital, in a courthouse of the U.S. government, one man will stop at nothing to keep his honor, and one will stop at nothing to find the truth."})
MERGE (topGun:Movie {title:"Top Gun", released:1986, tagline:'I feel the need, the need for speed.'})

MERGE (action:Genre {name:'Action'})
MERGE (drama:Genre {name:'Drama'})
MERGE (fantasy:Genre {name:'Fantasy'})
MERGE (mystery:Genre {name:'Mystery'})
MERGE (thriller:Genre {name:'Thriller'})
MERGE (romance:Genre {name:'Romance'})

MERGE (theDevilsAdvocate)-[:IN_GENRE]->(fantasy)
MERGE (theDevilsAdvocate)-[:IN_GENRE]->(mystery)
MERGE (theDevilsAdvocate)-[:IN_GENRE]->(triller)

MERGE (aFewGoodMen)-[:IN_GENRE]->(mystery)
MERGE (aFewGoodMen)-[:IN_GENRE]->(triller)

MERGE (topGun)-[:IN_GENRE]->(action)
MERGE (topGun)-[:IN_GENRE]->(drama)
MERGE (topGun)-[:IN_GENRE]->(romance);