# Install 6 nodes with isolated mode
* Controller 3 nodes
  * controller-1 => port=9093
  * controller-2 => port=9093
  * controller-3 => port=9093
* Broker 3 nodes
  * kafka-1 => port=29092
  * kafka-2 => port=39092
  * kafka-3 => port=39092

## 1. Start Controller nodes
```
$docker compose up -d controller-1
$docker compose up -d controller-2
$docker compose up -d controller-3
$docker compose ps

## 2. Start Kafka nodes
```
$docker compose up -d kafka-1
$docker compose up -d kafka-2
$docker compose up -d kafka-3
$docker compose ps
```

## 3. Install [Kafka UI](https://github.com/provectus/kafka-ui)
```
$docker compose up -d kafka-ui
$docker compose ps
```

Access to Kafka UI
* http://localhost:8080

## 4. Try to connect to Kafka with consumer_group
* [Download Kafka](https://kafka.apache.org/downloads)
```
$bin/kafka-console-consumer.sh --topic test --from-beginning --bootstrap-server localhost:29092 --group test-conusmer-group
```