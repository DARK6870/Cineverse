# build stage
FROM node:22.12-alpine AS build

# args
ARG APP_NAME="adminui"
ARG PROJECT_NAME="admin-ui"
ARG BASE_HREF="/admin/"

# copy files
COPY ${PROJECT_NAME} /opt/${APP_NAME}/

WORKDIR /opt/${APP_NAME}

# build ui
RUN npm cache verify
RUN npm i --no-package-lock --legacy-peer-deps
RUN npx ng build --base-href=${BASE_HREF} --output-path /app/out

# runtime image
FROM nginx:alpine-slim AS runtime-app

# args
ARG APP_NAME="adminui"
ARG BASE_HREF="/admin/"
ENV BASE_HREF=${BASE_HREF}

WORKDIR /opt/${APP_NAME}/

# clear html folder
RUN rm -rf /usr/share/nginx/html/*

# copy bild files
COPY --from=build /app/out/browser /usr/share/nginx/html${BASE_HREF}
COPY cicd/${APP_NAME}/docker/run.sh /opt/${APP_NAME}/run.sh
COPY cicd/${APP_NAME}/docker/nginx.conf /etc/nginx/conf.d/nginx.conf

# give permissions
RUN chown nginx:nginx /usr/share/nginx/html/*
RUN chmod +x /opt/${APP_NAME}/run.sh

# install packages
ENV PACKAGES="ca-certificates gettext"
RUN apk update && apk upgrade && apk add $PACKAGES

RUN chgrp -R 0 /etc/nginx/conf.d && \
    chmod -R g+rwX /etc/nginx/conf.d
RUN chgrp -R 0 /etc/nginx/conf.d/nginx.conf && \
    chmod -R g+rwX /etc/nginx/conf.d/nginx.conf 
RUN chgrp -R 0 /opt/${APP_NAME} && \
    chmod -R g+rwX /opt/${APP_NAME}
RUN chgrp -R 0 /var/cache/nginx && \
    chmod -R g+rwX /var/cache/nginx
RUN chgrp -R 0 /var/log/nginx && \
    chmod -R g+rwX /var/log/nginx
RUN chgrp -R 0 /run && \
    chmod -R g+rwX /run
RUN chgrp -R 0 /etc/ssl/certs
RUN chmod -R g+rwX /etc/ssl/certs
RUN chgrp -R 0 /usr/local/share/ca-certificates
RUN chmod -R g+rwX /usr/local/share/ca-certificates

# application entry point
EXPOSE 4200
ENTRYPOINT ["/bin/sh", "run.sh"]
