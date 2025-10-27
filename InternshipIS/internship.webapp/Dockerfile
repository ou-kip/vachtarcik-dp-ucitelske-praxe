FROM node:18-alpine AS build

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

ARG VITE_INTERNSHIPS_API_BASE_URL
ARG VITE_AUTH_API_BASE_URL
ENV VITE_INTERNSHIPS_API_BASE_URL=$VITE_INTERNSHIPS_API_BASE_URL
ENV VITE_AUTH_API_BASE_URL=$VITE_AUTH_API_BASE_URL

RUN npm run build

FROM nginx:1.19-alpine

RUN apk add --no-cache gettext

COPY --from=build /app/dist /usr/share/nginx/html

COPY nginx.conf.template /etc/nginx/templates/nginx.conf.template

COPY ssl/praxeosucz.crt /etc/nginx/ssl/praxeosucz.crt
COPY ssl/praxeosucz.key /etc/nginx/ssl/private/praxeosucz.key
COPY ssl/praxeosu.czCA.crt /etc/nginx/ssl/praxeosuczCA.crt

RUN chmod 644 /etc/nginx/ssl/praxeosucz.crt 
RUN chmod 644 /etc/nginx/ssl/praxeosuczCA.crt 
RUN chmod 600 /etc/nginx/ssl/private/praxeosucz.key

CMD envsubst '$SERVER_NAME' < /etc/nginx/templates/nginx.conf.template > /etc/nginx/conf.d/default.conf \
    && nginx -g 'daemon off;'

EXPOSE 443