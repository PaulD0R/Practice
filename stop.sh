#!/bin/bash

docker compose -f Kafka/compose.yaml down --remove-orphans
docker compose -f NotificationService/compose.yaml down --remove-orphans