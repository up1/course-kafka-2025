# Install 3 nodes with combined mode
* Broker + Controller
  * kafka-1 => port=29092
  * kafka-2 => port=39092
  * kafka-3 => port=39092

## 1. Start Kafka nodes
```
$docker compose up -d kafka-1
$docker compose up -d kafka-2
$docker compose up -d kafka-3
$docker compose ps
```

## 2. Install [Kafka UI](https://github.com/provectus/kafka-ui)
```
$docker compose up -d kafka-ui
$docker compose ps
```

Access to Kafka UI
* http://localhost:8080

## 3. Try to connect to Kafka with consumer_group
* [Download Kafka](https://kafka.apache.org/downloads)
```
$bin/kafka-console-consumer.sh --topic test --from-beginning --bootstrap-server localhost:29092 --group test-conusmer-group
```