# Install Kafka with single node
* For development and testing

## 1. Install Kafka
```
$docker compose up -d broker
$docker compose ps
```

## 2. Install Kafka UI
```
$docker compose up -d kafka-ui
$docker compose ps
```

Access to Kafka UI
* http://localhost:8080

## 3. Monitoring with Prometheus and Grafana
* [Kafka exporter for Prometheus](https://github.com/danielqsj/kafka_exporter)
  * Brokers
  * Topics
  * Consumer Groups

### 3.1 Start Kafka exporter
```
$docker compose up -d kafka-exporter
$docker compose ps
```

Access to Kafka Metric
* http://localhost:9308