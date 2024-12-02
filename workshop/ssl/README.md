# Workshop Secure Kafka with SSL
* Kafka broker server
* Zookeeper
* Schema registry server
* Producer and consumer

## 1. Generate certs
```
$sh generate-certs.sh    
```

## 2. Create Kafka cluster with Zookeeper
```
$docker compose up -d kafka
$docker compose ps
```
## 3. Create Schema registry server
```
$export SSL_SECRET=datahub
$docker compose up -d schema-registry
$docker compose ps
```

## 4. Create Kafka UI
```
$export SSL_SECRET=datahub
$docker compose up -d kafka-ui
$docker compose ps
```
Kafka UI
* http://localhost:8080/