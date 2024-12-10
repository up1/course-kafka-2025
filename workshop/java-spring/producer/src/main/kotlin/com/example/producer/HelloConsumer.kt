package com.example.producer

import EXAMPLE_TOPIC_NAME
import GROUP_ID
import org.slf4j.LoggerFactory
import org.springframework.kafka.annotation.KafkaListener
import org.springframework.kafka.support.Acknowledgment
import org.springframework.kafka.support.KafkaHeaders
import org.springframework.messaging.handler.annotation.Header
import org.springframework.messaging.handler.annotation.Payload
import org.springframework.stereotype.Component

@Component
class ExampleConsumer {
    private val logger = LoggerFactory.getLogger(this.javaClass)

    @KafkaListener(topics = [EXAMPLE_TOPIC_NAME], groupId = GROUP_ID)
    fun firstListener(
        @Payload message: String,
        @Header(KafkaHeaders.RECEIVED_PARTITION) partition: Int,
        @Header(KafkaHeaders.OFFSET) offsets: Int,
        acknowledgment: Acknowledgment) {
        logger.info("Message received: [$message] in partition=[$partition] with offset=[$offsets]")
        // Disable commit
//        acknowledgment.acknowledge()
    }
}