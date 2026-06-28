#!/bin/bash

NETWORK_NAME="notification-network"
KAFKA_FILE="Kafka/compose.yaml"
NOTIFICATION_SERVICE_FILE="NotificationService/compose.yaml"
EMAIL_FILE="EmailService/compose.yaml"

if [ ! "$(docker network ls | grep -w $NETWORK_NAME)" ]; then
  docker network create $NETWORK_NAME
fi

docker compose -f $KAFKA_FILE up -d
docker compose -f $NOTIFICATION_SERVICE_FILE up -d
docker compose -f $EMAIL_FILE up -d
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
