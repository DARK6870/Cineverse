#!/bin/sh

update-ca-certificates

envsubst '${LB_FQDN} ${BASE_HREF}' < /etc/nginx/conf.d/nginx.conf > /etc/nginx/conf.d/default.conf
rm /etc/nginx/conf.d/nginx.conf

nginx -g 'daemon off;'