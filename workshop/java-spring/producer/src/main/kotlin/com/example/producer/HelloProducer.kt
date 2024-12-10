package com.example.producer

import EXAMPLE_TOPIC_NAME
import org.springframework.kafka.core.KafkaTemplate
import org.springframework.stereotype.Component

@Component
class HelloProducer(
    private val kafkaTemplate: KafkaTemplate<String, String>
) {
    fun sendStringMessage(message: String) {
        kafkaTemplate.send(EXAMPLE_TOPIC_NAME, message)
    }
}