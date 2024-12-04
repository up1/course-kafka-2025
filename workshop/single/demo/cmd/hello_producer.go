package main

import (
	"fmt"
	"os"
	"time"

	"github.com/confluentinc/confluent-kafka-go/v2/kafka"
)

func main() {
	kafkaURL := os.Getenv("KAFKA_URL")
	topic := "demo-topic"
	totalMsgcnt := 3

	p, err := kafka.NewProducer(&kafka.ConfigMap{
		"bootstrap.servers": kafkaURL,
		// Enable the Idempotent Producer
		// "enable.idempotence": true,

		// acks=1 means the leader will write the record to its local log but will respond without awaiting full acknowledgement from all followers.
		// acks=1 is the default setting.
		// acks=0 means the leader will not wait for any acks from the followers.
		// acks=all means the leader will wait for all in-sync replicas to acknowledge the record.
		// acks=all is the safest setting that will guarantee no data loss.
		// acks=all is the slowest setting.
		"acks": 1,

		// The number of messages allowed on the producer queue.
		// When this number is reached, the producer will either block or throw an error based on queue.buffering.max.messages.
		"queue.buffering.max.messages": 1000000,

		// The maximum size of the message queue in bytes.
		// This property is used to set the maximum amount of memory the producer will use to buffer messages waiting to be sent to the broker.
		"queue.buffering.max.kbytes": 1000000,

		// The maximum time, in milliseconds, for buffering data on the producer queue.
		// If this time is exceeded, librdkafka will return an error.
		"queue.buffering.max.ms": 50,

		// The maximum number of messages batched in one MessageSet.
		// The total MessageSet size is also limited

		// The maximum size of a message in bytes.
		// This is the largest size of a message that the broker will allow.
		"message.max.bytes": 1000000,
	})

	if err != nil {
		fmt.Printf("Failed to create producer: %s\n", err)
		os.Exit(1)
	}

	fmt.Printf("Created Producer %v\n", p)

	// Listen to all the events on the default events channel
	go func() {
		for e := range p.Events() {
			switch ev := e.(type) {
			case *kafka.Message:
				// The message delivery report, indicating success or
				// permanent failure after retries have been exhausted.
				// Application level retries won't help since the client
				// is already configured to do that.
				m := ev
				if m.TopicPartition.Error != nil {
					fmt.Printf("Delivery failed: %v\n", m.TopicPartition.Error)
				} else {
					fmt.Printf("Delivered message to topic %s [%d] at offset %v\n",
						*m.TopicPartition.Topic, m.TopicPartition.Partition, m.TopicPartition.Offset)
				}
			case kafka.Error:
				// Generic client instance-level errors, such as
				// broker connection failures, authentication issues, etc.
				//
				// These errors should generally be considered informational
				// as the underlying client will automatically try to
				// recover from any errors encountered, the application
				// does not need to take action on them.
				fmt.Printf("Error: %v\n", ev)
			default:
				fmt.Printf("Ignored event: %s\n", ev)
			}
		}
	}()

	msgcnt := 0
	for msgcnt < totalMsgcnt {
		value := fmt.Sprintf("Producer example, message #%d", msgcnt)

		err = p.Produce(&kafka.Message{
			TopicPartition: kafka.TopicPartition{Topic: &topic, Partition: kafka.PartitionAny},
			Value:          []byte(value),
			Headers:        []kafka.Header{{Key: "myTestHeader", Value: []byte("header values are binary")}},
		}, nil)

		if err != nil {
			if err.(kafka.Error).Code() == kafka.ErrQueueFull {
				// Producer queue is full, wait 1s for messages
				// to be delivered then try again.
				time.Sleep(time.Second)
				continue
			}
			fmt.Printf("Failed to produce message: %v\n", err)
		}
		msgcnt++
	}

	// Flush and close the producer and the events channel
	for p.Flush(10000) > 0 {
		fmt.Print("Still waiting to flush outstanding messages\n")
	}
	p.Close()
}
