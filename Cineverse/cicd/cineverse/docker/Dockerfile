# build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

# copy source files
COPY ./Cineverse/Cineverse /opt/cineverse/

WORKDIR /opt/cineverse/

# restore and build the app
RUN dotnet restore ./Cineverse.sln
RUN dotnet build -c Release ./Cineverse.sln --no-restore
RUN dotnet publish ./Cineverse.sln --no-restore -c Release -o /app/out

# build a runtime application
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime-app

# change workdir
WORKDIR /opt/cineverse/

# copy build files
COPY --from=build /app/out .

# copy bash script
COPY ./cineverse.sh /opt/cineverse/cineverse.sh

# chmod
RUN chmod +x /opt/cineverse/Cineverse.Api
RUN chmod +x /opt/cineverse/cineverse.sh

# install system packages
ENV PACKAGES ca-certificates openssl
RUN apk update && apk upgrade && apk add --no-cache $PACKAGES

# application files - needs to write logs, read config
RUN chgrp -R 0 /opt/cineverse
RUN chmod -R g+rwX /opt/cineverse

# ssl certificates - may need to read or add custom certs
RUN chgrp -R 0 /etc/ssl/certs
RUN chmod -R g+rwX /etc/ssl/certs

# custom CA certificates - may need to add trusted certificates
RUN chgrp -R 0 /usr/local/share/ca-certificates
RUN chmod -R g+rwX /usr/local/share/ca-certificates

# expose port 7400
EXPOSE 7400

# application entry point
ENTRYPOINT ["/bin/sh", "/opt/cineverse/cineverse.sh"]

#RUN apk update && apk upgrade && apk add $PACKAGES
#RUN ln /usr/lib/librdkafka.so /usr/lib/alpine-librdkafka.so