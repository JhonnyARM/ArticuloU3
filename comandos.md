docker exec -it kafka-1 bash
kafka-console-consumer --bootstrap-server localhost:9092 --topic inventory-.inventory.products --from-beginning

## mysql permisos

-- Otorgar permisos para la replicación y lectura en todas las bases de datos
GRANT SELECT, REPLICATION SLAVE, REPLICATION CLIENT ON _._ TO 'debezium'@'%';

-- Permitir que Debezium vea las bases de datos disponibles
GRANT SHOW DATABASES ON _._ TO 'debezium'@'%';

-- Permitir que Debezium ejecute SHOW TABLES en bases de datos relevantes
GRANT SHOW TABLES ON inventory.\* TO 'debezium'@'%';

-- Permitir que Debezium lea información del binlog
GRANT PROCESS ON _._ TO 'debezium'@'%';

-- Conceder acceso al binlog
GRANT SUPER ON _._ TO 'debezium'@'%';

-- Aplicar los cambios
FLUSH PRIVILEGES;

GRANT SELECT, REPLICATION SLAVE, REPLICATION CLIENT ON _._ TO 'debezium'@'%';
GRANT SHOW DATABASES ON _._ TO 'debezium'@'%';
GRANT SHOW TABLES ON inventory._ TO 'debezium'@'%';
GRANT PROCESS ON _._ TO 'debezium'@'%';
GRANT SUPER ON _.\* TO 'debezium'@'%';
FLUSH PRIVILEGES;
