# Workshop Monitoring with Kafka
* Broker => 2 nodes
* Zookeeper => 3 nodes
* Producer
* Consumer

## Tools
* Docker
* JMX
* Prometheus
* Grafana


## Download [JMX Exporter Agent](https://github.com/prometheus/jmx_exporter)
```
$wget https://github.com/prometheus/jmx_exporter/releases/download/1.1.0/jmx_prometheus_javaagent-1.1.0.jar -O jmx-exporter/jmx_prometheus_javaagent.jar
```

## Start Zookeeper 3 nodes
```
$docker compose up -d zookeeper-1
$docker compose up -d zookeeper-2
$docker compose up -d zookeeper-3
$docker compose ps
```  

Access to url of metric
- http://localhost:17071/metrics
- http://localhost:17072/metrics
- http://localhost:17073/metrics

## Start Kafka broker 2 nodes
```
$docker compose up -d kafka-1
$docker compose up -d kafka-2
$docker compose ps
```  

Access to url of metric
- - http://localhost:17074/metrics

## Start Prometheus

```
$docker compose up -d prometheus
$docker compose ps
```
Access to urls
- http://localhost:9090
  * zookeeper_quorumsize
  * zookeeper_numaliveconnections
- http://localhost:9090/targets

## Start Grafana
```
$docker compose up -d grafana
$docker compose ps
```
Access to urls
- http://localhost:3000
- http://localhost:3000/dashboards

## Start Producer
```
$docker compose up -d producer
$docker compose ps
```

Access to urls
- http://localhost:19091/metrics

## Start Consumer
```
$docker compose up -d consumer
$docker compose ps
```

Access metric url
* http://localhost:19094/metrics



