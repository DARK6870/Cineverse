# Cineverse

Cineverse is a movie booking platform built as a set of microservices with a web UI, an admin UI, and supporting infrastructure for local development and Kubernetes deployment.

## What is here
Main services and modules:
- `Cineverse` service uses GraphQL for the core domain: movies, bookings, and user-facing flows.
- `IdentityService` exposes both GraphQL and REST for auth and user identity.
- `NotificationService` consumes Kafka messages and sends email notifications.
- `ApiGateway` is the entry point and routes requests to the services.
- `Infrastructure` contains shared libraries for MongoDB, Kafka, logging, health checks, and web API helpers.
- `AutomationTests` is the automation test suite (work in progress).
- `CineverseUI/AdminUI/IdentityUI` is the Angular frontend.

## Tech stack
This repo is primarily .NET with supporting infrastructure and UI tooling:
- .NET services (Cineverse, IdentityService, NotificationService, ApiGateway)
- GraphQL and REST APIs
- Kafka for async messaging
- MongoDB for persistence and migrations
- Redis (background jobs)
- Seq for centralized logging
- Kubernetes manifests and scripts in `k8s/`
- Angular for the UI

## Kubernetes and ingress
The `k8s/` folder contains all Kubernetes-related files, including deployments, services, config maps, ingress rules, and helper scripts.

Ingress examples (not exhaustive):
- `kafkaui.localhost:8080` -> Kafka UI
- `redisui.localhost:8080` -> Redis Stack browser
- `seq.localhost:8080` -> Seq logs UI
- `dashboard.localhost:8080` -> Dashboard

## Screenshots
![Dashboard](img/screencapture-dashboard-localhost-8080-2026-03-26-22_55_37.png)
![Kafka UI](img/screencapture-kafkaui-localhost-8080-ui-clusters-local-all-topics-2026-03-26-23_19_46.png)
![Redis UI](img/screencapture-redisui-localhost-8080-redis-stack-browser-2026-03-26-23_20_06.png)
![Seq](img/screencapture-seq-localhost-8080-2026-03-26-22_54_53.png)
![Seq Dashboard](img/tg_image_1747892588.png)
![Home](img/screencapture-localhost-8080-2026-03-26-23_22_03.png)
![Movies](img/screencapture-localhost-8080-movies-2026-03-26-23_22_35.png)
![Movie Details](img/screencapture-localhost-8080-movies-details-69b6ba58f54a66d4172bc7a8-2026-03-26-23_22_19.png)
![Booking Flow](img/screencapture-localhost-8080-book-69b6b6e8f54a66d4172bc771-2026-03-26-23_23_02.png)
![Profile](img/tg_image_773377802.png)
![My bookings](img/tg_image_341268862.png)
![Booking](img/screencapture-localhost-8080-booking-69bae083bc4c26b7550f7f8e-2026-03-26-23_07_52.png)
![Contact](img/screencapture-localhost-8080-contact-2026-03-26-23_08_13.png)
![Change Password](img/tg_image_1475662972.png)
![Login](img/tg_image_1611314640.png)
![Email Confirmation Email Message](img/tg_image_2583304727.png)
![Booking Created Email Message](img/tg_image_415801817.png)
![Admin](img/screencapture-localhost-8080-admin-2026-03-26-23_08_31.png)
![Admin Bookings](img/screencapture-localhost-8080-admin-bookings-2026-03-26-23_16_00.png)
![Admin Bookings 2](img/screencapture-localhost-8080-admin-bookings-2026-03-26-23_16_42.png)
![Admin Movies](img/screencapture-localhost-8080-admin-movies-2026-03-26-23_18_45.png)
![Admin Movies 2](img/screencapture-localhost-8080-admin-movies-2026-03-26-23_19_04.png)
![Admin Edit Movie](img/tg_image_2874163440.png)
![Admin Users](img/screencapture-localhost-8080-admin-users-2026-03-26-23_11_46.png)
![MongoDB](img/tg_image_775022100.png)
![Tests](img/tg_image_2961953177.png)