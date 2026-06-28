#!/bin/bash

docker compose -f Kafka/compose.yaml down --remove-orphans
docker compose -f NotificationService/compose.yaml down --remove-orphans
docker compose -f EmailService/compose.yaml down --remove-orphans