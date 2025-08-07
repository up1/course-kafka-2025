# Workshop with Apache Kafka
* Kafka broker server
  * 3 Brokers
* Zookeeper
* Schema registry server
* Develop producer and consumer
  * Go
  * [Confluent's Apache Kafka Golang client](https://github.com/confluentinc/confluent-kafka-go)


## 1. Create Kafka cluster with Zookeeper
```
$docker compose up -d zookeeper
$docker compose up -d kafka-1
$docker compose up -d kafka-2
$docker compose ps
```

Kafka bootstrap server
* http://localhost:19092

Run client with Go
```
$cd demo
$go mod tidy

# Broker 1
$export KAFKA_URL=localhost:19092
# or
# Broker 2
$export KAFKA_URL=localhost:29092

# Producer
$go run cmd/hello_producer.go

# Consumer with groups
$go run cmd/hello_consumer.go group1
$go run cmd/hello_consumer.go group1
```


## 2. Create Schema registry server
```
$docker compose up -d schema-registry
$docker compose ps
```

Schema registry
* http://localhost:8081
* http://localhost:8081/schemas

## 3. Generate schema with Protocol Buffer
* [protoc](https://grpc.io/docs/protoc-installation/)
* [proto-gen-go](https://grpc.io/docs/languages/go/quickstart/)

```
$cd demo
$protoc --go_out=. --go_opt=paths=source_relative --go-grpc_out=. --go-grpc_opt=paths=source_relative protobuf/demo-v1.proto

$mv protobuf/demo*.go pkg/message.v1/
```

## 4. Start producer
```
$cd demo
$go mod tidy

$export KAFKA_URL=localhost:19092
$export SCHEMA_REGISTRY_URL=http://localhost:8081
$go run cmd/producer/main.go
```

Check schema in Schema registry server
```
$curl http://localhost:8081/schemas

[
  {
    "subject": "topic.v1-value",
    "version": 1,
    "id": 1,
    "schemaType": "PROTOBUF",
    "schema": "syntax = \"proto3\";\npackage message.v1;\n\noption go_package = \"./pkg/message.v1\";\n\nmessage TestMessage {\n  int32 Value = 1;\n}\n"
  }
]
```


## 5. Start consumer
```
$cd demo
$go mod tidy

$export KAFKA_URL=localhost:19092
$export SCHEMA_REGISTRY_URL=http://localhost:8081
$go run cmd/consumer/main.go
```

## 6. Create Kafka UI
```
$export SSL_SECRET=datahub
$docker compose up -d kafka-ui
$docker compose ps
```
Kafka UI
* http://localhost:8080/
