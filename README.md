# Сервис отправки уведомлений (Notification Service)
Микросервис для асинхронной отправки сообщений.
Реализован основной оркестратор отправки сообщений, email-service, sms-service, push-service и необходимые mock-services.

# 1 Запуск проекта через docker
### Ссылки доступа
+ Swagger: http://localhost:8080/swagger/index.html

### Запуск
- `bash start.sh`

### Инициализация БД
Для того чтобы сервис мог отправлять письма, добавьте конфигурацию SMTP-сервера в БД:
- `docker exec -i email-postgres psql -U postgres -d email_db -c "INSERT INTO \"SmtpSettings\" (\"Id\", \"Name\", \"Host\", \"Port\", \"RealEmail\", \"Password\") VALUES (gen_random_uuid(), 'ExampleProvider', 'smtp.example.com', 465, 'sender@example.com', 'password');"`

# 3 Настройка аутентификации (JWT)
Для проверки токенов авторизации сервис использует асимметричное шифрование (RSA). Для локальной разработки используйте ключ:

-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDbREyHcyvOlCHY
iItSQDwiZOf3fgM0kwz03SrrlCjd10MVT6/sQI3Ke/hK2mCEWOuSso81dLoVcqx9
VokvSFipP+28JIZNm1cCL1Je38337b1ItIf4BXXAf7coXevPzBFFgO7eEgPRHTUF
rB/MrxTqyfABZpACQPAxuRheT5wjITfyThsga7M1EgB60zCEIOtjkQI8Gq2L7sNQ
aj6HWzKHRjB3X1ksVEMdsFFGQ3ZqmDz6wn0lgr+kPM8cQ8gYCXPSki2fsGSsRwCE
3z54gn/8cAIMR3ABlbkfzAafPgSRTwC1Zqp81Ab6vuNC+wJ5NXVx87C8C8sAY6SS
+eGCdQOLAgMBAAECggEABt+swIBJSsgVCYs6RuFRoUhlhzoxkofbm5+FVW5HG7FA
Xp4pZzAc4dCiF4KiDPQWqnojiCr5HGwEYUeBtHRBp0ikRMYmWa95ek2OeP/uHowa
53kVXB6bsuWuEbtkBZchoXqpwldxPDB8tYlQU+PXNYcQGZMkB5YOkZh6XZaEUAuP
MCp1cBXXjAx5VcahOXC94ZWHlGdyDzxICi9UNb4/A5T0w7Ep/Wc/N1iH/wP21G7/
/vngTeUUozV7gxPckMY9bOKKCWmjS0jUsH/PVPP6vd9UQcV36fqHd4zJxgIuuunb
IpsrnfKbJAQ56dFxZvialuh2u+5O279M6Fie5zzt9QKBgQDyWWg3VHqF6o/Pip3e
NOmPmO3lCBoaHy2ikyDcjsbEeRpDhhBPiW8vpxARf6RzaUfGxjmgO7NPHEmvZ2ul
agwUmHE749hKrC4ZpY7ek0ObGDe9YHSm1b9aAt1uQUdEeoQ8OcLXeo5YPAbtwg9R
YdKjwgl/iAd1LyiOzlQtf2QVDQKBgQDnng0Aswxy5XOgfZOVtjTDm9v5wuulmDLr
eaSLRLf48tMaB+7x5AiSzDLNwcQ5uCFeuGHTqKgPfYe/4Mh60gZRkTtO0cC2Y4Qg
ldwTyqqHNRRNwTovOpwwIJN4DriK/tt1XSLUrSO3g6wxTujsSgl4WB/RFXvLf9gh
NqgqB7KE9wKBgQDJ99rTabLmUdHh25qvKZeQFZoEqljedidY+paDWPWrnWVW6q5D
Kr0fkHHis2aAzDeGH4K816QahO+fn2flpdhFfbm4eKyzpoSQ2RmUwlOwOGGTkIQG
2dGrmQGitgJHvnbfnh+T5k4kmHoJwRV09DpQQRAbWWklrtR1FtyuQiFNLQKBgAsP
11sjMgMCxmTx2myaCScWeIkTMCH2hOgOJTepjofLQR3LJzRSSb6JFhwRlweSrbVS
ZQmw0mX6/tLBL5H+GeMnZoe7KNMNKbBMt/gSA9b1SAT2p4q959u8ko53VYT96wlN
623w3vXSyhSIykMOvikLPGnF9uWfM3lSnPF08Ke9AoGAdDbGp0oeRnqwJUUQGrD5
wdKJVUGCmecDt8hTMPGQ2NdmVT1jcbI8AT2o6RkYLo0Y9h/KUOh0DHmPb7DQJPl/
jJF4v4uDgu6DtCFRkGqH+dozW9MoTuq77D5IdU6e9MbWeTLmtv+0WXegdnyCIC4r
l/8jTCt92GXInVeyHNmA8YI=
-----END PRIVATE KEY-----