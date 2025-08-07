package com.example.producer

import EXAMPLE_TOPIC_NAME
import org.apache.kafka.clients.admin.NewTopic
import org.apache.kafka.clients.producer.ProducerConfig
import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.context.annotation.Bean
import org.springframework.kafka.annotation.KafkaListener

@SpringBootApplication
class ProducerApplication {

	// Create a new topic with 5 partitions and rf = 2
	@Bean
	fun topic() = NewTopic(EXAMPLE_TOPIC_NAME, 5, 2)
}

fun main(args: Array<String>) {
	runApplication<ProducerApplication>(*args)
}





