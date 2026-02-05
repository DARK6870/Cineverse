# build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

# declare build arguments
ARG SERVICE_NAME="NotificationService"
ARG SOLUTION_NAME="${SERVICE_NAME}.sln"
ARG APP_NAME="notificationservice"
ARG APP_PATH="/opt/${APP_NAME}"

# copy source files
COPY ${SERVICE_NAME} ${APP_PATH}/
WORKDIR ${APP_PATH}

# restore and build the app
RUN dotnet restore ./${SOLUTION_NAME}
RUN dotnet build -c Release ./${SOLUTION_NAME} --no-restore
RUN dotnet publish ./${SOLUTION_NAME} --no-restore -c Release -o /app/out

# build a runtime application
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime-app

ARG SERVICE_NAME="NotificationService"
ARG PROJECT_NAME="${SERVICE_NAME}.Api"
ARG APP_NAME="notificationservice"
ARG APP_PATH="/opt/${APP_NAME}"

WORKDIR ${APP_PATH}

# copy files
COPY --from=build /app/out .
COPY cicd/${APP_NAME}/docker/run.sh ${APP_PATH}/run.sh
COPY ${SERVICE_NAME}/NotificationTemplates ${APP_PATH}/NotificationTemplates

# chmod
RUN chmod +x ${APP_PATH}/${PROJECT_NAME}
RUN chmod +x ${APP_PATH}/run.sh

# install system packages
ENV PACKAGES="ca-certificates icu icu-libs librdkafka-dev tzdata gettext openssl jq curl"
RUN apk update && apk upgrade && apk add --no-cache $PACKAGES

# create symlink for librdkafka
RUN ln -s /usr/lib/librdkafka.so /usr/lib/alpine-librdkafka.so

# application files 
RUN chgrp -R 0 ${APP_PATH}
RUN chmod -R g+rwX ${APP_PATH}

# ssl certificates
RUN chgrp -R 0 /etc/ssl/certs
RUN chmod -R g+rwX /etc/ssl/certs

# custom CA certificates
RUN chgrp -R 0 /usr/local/share/ca-certificates
RUN chmod -R g+rwX /usr/local/share/ca-certificates

# application entry point
EXPOSE 7401
ENTRYPOINT ["/bin/sh", "/opt/notificationservice/run.sh"]