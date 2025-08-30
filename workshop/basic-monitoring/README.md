# Monitoring Apache Kafka
* [Prometheus](https://prometheus.io/)
* [Kafka Exporter](https://github.com/danielqsj/kafka_exporter)
* [Prometheus JMX Expoter](https://github.com/prometheus/jmx_exporter)
* [Prometheus Alertmanager](https://prometheus.io/docs/alerting/latest/alertmanager/)
* [Grafana](https://grafana.com/)


## 1. Download softwares
* [Prometheus](https://prometheus.io/download/)
* [Prometheus Alertmanager](https://prometheus.io/download/)
* [Kafka Exporter](https://github.com/danielqsj/kafka_exporter/releases)
* [Prometheus JMX Expoter](https://github.com/prometheus/jmx_exporter/releases)
* [Grafana OSS](https://grafana.com/grafana/download?pg=oss-graf&plcmt=hero-btn-1&edition=oss)
  * [List of dashboard](https://github.com/strimzi/strimzi-kafka-operator/tree/main/examples/metrics)

## 2. Install and Start Alert manager
```
$cd alertmanager
$./alertmanager --web.listen-address=:9095
```

Go to Alert manager UI
* http://localhost:9095/

## 3. Install and Start prometheus
```
$cd prometheus
```

### 3.1 Edit config file `prometheus.yml`
```
global :
  scrape_interval: 5s
  evaluation_interval: 5s
alerting:
  alertmanagers:
  - static_configs:
    - targets: ["localhost:9093"]

rule_files:
- "rules.yml"
scrape_configs:
- job_name: "kafka"
  static_configs:
  - targets: ["localhost:9101"]
- job_name: "prometheus"
  static_configs:
  - targets: ["localhost:9090"]
- job_name: "kafka-exporter"
  static_configs:
  - targets: ["localhost:9308"]
```

### 3.2 Create file `rules.yml`
```
groups:
- name: Kafka
  rules:
  - alert: Kafka Metrics Not Reachable
    expr: up{job="kafka"} == 0
    for: 1m
```

### 3.3 Start prometheus server
```
$./prometheus
```

Go to prometheus UI
* http://localhost:9090/
* http://localhost:9090/targets


## 4. Start Apache Kafka with Prometheus JMX exporter
* [Download file kafka-kraft-3_0_0.yml](https://github.com/prometheus/jmx_exporter/blob/main/examples/kafka-kraft-3_0_0.yml) to $DEMO_KAFKA_HOME folder


### 4.1 Confog KAFKA_OPTS environment variable
For MAC OS and Linux
```
$export DEMO_KAFKA_HOME=/Users/somkiatpuisungnoen/data/slide/kafka/kafka-2024/workshop/basic-monitoring/software
$export KAFKA_OPTS="-javaagent:$DEMO_KAFKA_HOME/jmx_prometheus_javaagent-1.4.0.jar=9101:$DEMO_KAFKA_HOME/kafka-kraft-3_0_0.yml"
```

For Windows
```
$set DEMO_KAFKA_HOME=your-path-to-jarfile
$set KAFKA_OPTS="-javaagent:%DEMO_KAFKA_HOME%/jmx_prometheus_javaagent-1.4.0.jar=9101:%DEMO_KAFKA_HOMEHOME%/kafka-kraft-3_0_0.yml"
```

### 4.2 Start Kafka server
```
$cd kafka_2.13-4.0.0
$bin/kafka-server-start.sh config/server.properties
```

Access to Kafka metric
* http://localhost:9101

Check status of Kafka at Promethus UI
* http://localhost:9090/targets

## 4.3 Start Kafka Exporter
```
$cd kafka-exporter
$./kafka_exporter --kafka.server=localhost:9092
```

Access to kafka exporter metric
* http://localhost:9308/


## 5. Install and Start Grafana server

### 5.1 Start grafana server
```
$cd grafana
$./bin/grafana-server
```

Access to Grafana UI
* http://localhost:3000
  * user=admin
  * password=admin

### 5.2 Add datasource
* Prometheus
  * http://localhost:9090


### 5.3 Create your dashboard
* [Kafka export overview](https://grafana.com/grafana/dashboards/7589-kafka-exporter-overview/)
* [Copy kafka dashboard](https://github.com/strimzi/strimzi-kafka-operator/tree/main/examples/metrics/grafana-dashboards)
  * strimzi-kafka.json
  * strimzi-kraft.json
