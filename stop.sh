#!/bin/bash

docker compose -f Brokker/compose.yaml down --remove-orphans
docker compose -f Wiremock/compose.yaml down --remove-orphans
docker compose -f NotificationService/compose.yaml down --remove-orphans
docker compose -f EmailService/compose.yaml down --remove-orphans
docker compose -f SmsService/compose.yaml down --remove-orphans
docker compose -f PushService/compose.yaml down --remove-orphans