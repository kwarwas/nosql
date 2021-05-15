import { Neo4jGraphQL } from '@neo4j/graphql';
import neo4j from 'neo4j-driver';
import { ApolloServer } from 'apollo-server';

const typeDefs = `
type Movie {
    title: String!
    released: Int!
    tagline: String
    genres: [Genre] @relationship(type: "IN_GENRE", direction: OUT)
}
type Genre {
    name: String!
    movies: [Movie] @relationship(type: "IN_GENRE", direction: IN)
}
`;

const driver = neo4j.driver(
  'bolt://localhost:7687',
  neo4j.auth.basic('neo4j', 'password')
);

const neoSchema = new Neo4jGraphQL({ typeDefs, driver });

const server = new ApolloServer({
  schema: neoSchema.schema,
  context: ({ req }) => ({ req })
});

server.listen(3003, '0.0.0.0').then(({ url }) => {
  console.log(`GraphQL API ready at ${url}`);
});