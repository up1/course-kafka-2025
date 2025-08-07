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

Check SSL Connectivity
```
$openssl s_client -connect localhost:9092
```

## 3. Create Schema registry server
```
$export SSL_SECRET=datahub
$docker compose up -d schema-registry
$docker compose ps
```

Verify schema registry server
```
$curl -k https://localhost:8081/subjects
```

## 4. Create Kafka UI
```
$export SSL_SECRET=datahub
$docker compose up -d kafka-ui
$docker compose ps
```
Kafka UI
* http://localhost:8080/

## 5. Start producer
```
$cd demo
$go mod tidy

$export KAFKA_URL=ssl://localhost:19092
$export SCHEMA_REGISTRY_URL=http://localhost:8081
$go run cmd/producer/main.go
```