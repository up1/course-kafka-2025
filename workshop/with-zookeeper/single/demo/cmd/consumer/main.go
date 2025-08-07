package main

import (
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
	consumer, err := kafka.NewConsumer(kafkaURL, schemaRegistryURL)
	defer consumer.Close()

	if err != nil {
		log.Fatal(err)
	}
	messageType := (&test.TestMessage{}).ProtoReflect().Type()
	if err := consumer.Run(messageType, topic); err != nil {
		log.Fatal(err)
	}
}
