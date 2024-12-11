# Run Kafka's producer and consumer
* [Spring Kafka](https://docs.spring.io/spring-kafka/reference/tips.html)

## Run server
```
$gradlew bootRun
```

## Testing to send data
```
$curl --location 'http://localhost:8080/test' \
--header 'Content-Type: application/json' \
--data '{
"message": "demo 01"
}'
```

## Build docker image
```
$docker image build -t producer:1.0 .
```


