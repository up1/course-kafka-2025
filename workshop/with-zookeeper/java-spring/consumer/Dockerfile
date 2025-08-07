# Use image as a parent image => https://hub.docker.com/_/eclipse-temurin
FROM eclipse-temurin:21.0.5_11-jdk-jammy

# Set the working directory in the container
WORKDIR /app

# Copy the JMX Exporter jar to the container
COPY ./agent/jmx_prometheus_javaagent.jar /app/jmx_prometheus_javaagent.jar

# Copy the JMX Exporter configuration file to the container
COPY ./agent/kafka-consumer.yml /app/kafka-consumer.yml

# Copy the Spring Boot application's jar to the container
COPY build/libs/consumer-0.0.1-SNAPSHOT.jar /app/demo.jar

# Expose the port that the Spring Boot application runs on
EXPOSE 8080

# Expose the port for JMX
EXPOSE 9091

# Run the Spring Boot application with the JMX Exporter agent
ENTRYPOINT ["java", "-javaagent:/app/jmx_prometheus_javaagent.jar=9091:/app/kafka-consumer.yml", "-jar", "/app/demo.jar"]