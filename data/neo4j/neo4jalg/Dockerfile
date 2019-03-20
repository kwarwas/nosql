FROM neo4j

ENV NEO4J_HOME="/var/lib/neo4j"

RUN wget https://github.com/neo4j-contrib/neo4j-graph-algorithms/releases/download/3.5.3.0/graph-algorithms-algo-3.5.3.0.jar -P "${NEO4J_HOME}"/plugins/

RUN echo "dbms.security.procedures.unrestricted=algo.*" >> "${NEO4J_HOME}"/conf/neo4j.conf

ENTRYPOINT ["/sbin/tini", "-g", "--", "/docker-entrypoint.sh"]
CMD ["neo4j"]