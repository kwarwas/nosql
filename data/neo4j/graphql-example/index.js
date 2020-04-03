import { makeAugmentedSchema } from 'neo4j-graphql-js';
import neo4j from 'neo4j-driver';
import { ApolloServer } from 'apollo-server';

const typeDefs = `
type Movie {
    title: String
    released: Int
    genres: [Genre] @relation(name: "IN_GENRE", direction: "OUT")
}
type Genre {
    name: String
    movies: [Movie] @relation(name: "IN_GENRE", direction: "IN")
}
`;

const schema = makeAugmentedSchema({ typeDefs });

const driver = neo4j.driver(
  'bolt://localhost:7687',
  neo4j.auth.basic('neo4j', 'password')
);

const server = new ApolloServer({ 
  schema, 
  context: { driver } 
});

server.listen(3003, '0.0.0.0').then(({ url }) => {
  console.log(`GraphQL API ready at ${url}`);
});