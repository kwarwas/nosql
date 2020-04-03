FROM neo4j:3.5.16

ENV NEO4J_HOME="/var/lib/neo4j"

RUN wget https://s3-eu-west-1.amazonaws.com/com.neo4j.graphalgorithms.dist/neo4j-graph-algorithms-3.5.8.0-standalone.jar -P "${NEO4J_HOME}"/plugins/

RUN echo "dbms.security.procedures.unrestricted=algo.*" >> "${NEO4J_HOME}"/conf/neo4j.conf