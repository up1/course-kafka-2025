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

Try to connect to Kafka with consumer_group
* [Download Kafka](https://kafka.apache.org/downloads)
```
$bin/kafka-console-consumer.sh --topic test --from-beginning --bootstrap-server localhost:9092 --group test-conusmer-group
```

### 3.2 Start Prometheus
```
$docker compose up -d prometheus
$docker compose ps
```

Access to Prometheus UI
* http://localhost:9090

### 3.2 Start Grafana
* [Kafka Exporter Overview](https://grafana.com/grafana/dashboards/7589-kafka-exporter-overview/)

```
$docker compose up -d grafana
$docker compose ps
```

Access to Grafana dashboard
* http://localhost:3000
  * Login with user=admin, password=admin
  * Create a new dashboard
    * Import id = 7589