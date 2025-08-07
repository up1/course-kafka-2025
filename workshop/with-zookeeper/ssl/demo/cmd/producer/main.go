package main

import (
	"fmt"
	"log"
	"os"

	"demo/internal/kafka"
	test "demo/pkg/message.v1"
)

const (
	topic = "topic.v1"
)

func main() {
	kafkaURL := os.Getenv("KAFKA_URL")
	schemaRegistryURL := os.Getenv("SCHEMA_REGISTRY_URL")
	producer, err := kafka.NewProducer(kafkaURL, schemaRegistryURL)
	defer producer.Close()

	if err != nil {
		log.Fatal(err)
	}
	testMSG := test.TestMessage{Value: 42}
	offset, err := producer.ProduceMessage(&testMSG, topic)
	if err != nil {
		log.Fatal(err)
	}
	fmt.Println(offset)
}
