# Workshop Monitoring with Kafka
* Broker
* Zookeeper
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

## Start Zookeeper  
```
$docker compose up -d zookeeper
$docker compose ps
```  

Access to url of metric
- http://localhost:7072/metric

## Start Kafka broker
```
$docker compose up -d kafka
$docker compose ps
```  

Access to url of metric
- http://localhost:7071/metric

## Start Prometheus

```
$docker compose up -d prometheus
$docker compose ps
```
Access to urls
- http://localhost:9090
- http://localhost:9090/targets

## Start Grafana
```
$docker compose up -d grafana
$docker compose ps
```
Access to urls
- http://localhost:3000
- http://localhost:3000/dashboards
  * Kafka 
  * Zookeeper
