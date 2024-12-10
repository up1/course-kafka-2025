# Run Kafka's producer and consumer

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


