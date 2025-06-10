# Krok 1: Build stage
FROM node:18-alpine AS build

# Nastav pracovnÌ adres·¯
WORKDIR /app

# ZkopÌruj package.json a package-lock.json
COPY package*.json ./

# Nainstaluj z·vislosti
RUN npm install

# ZkopÌruj vöechny soubory aplikace
COPY . .

ARG VITE_INTERNSHIPS_API_BASE_URL
ARG VITE_AUTH_API_BASE_URL
ENV VITE_INTERNSHIPS_API_BASE_URL=VITE_INTERNSHIPS_API_BASE_URL
ENV VITE_AUTH_API_BASE_URL=$VITE_AUTH_API_BASE_URL

# Buildni aplikaci
RUN npm run build

# Krok 2: Production stage
FROM nginx:alpine

# ZkopÌruj buildnutou aplikaci do NGINX
COPY --from=build /app/dist /usr/share/nginx/html

# Nastav SSL certifik·ty
COPY ssl/praxeosucz.crt /etc/nginx/ssl/praxeosucz.crt
COPY ssl/praxeosucz.key /etc/nginx/ssl/private/praxeosucz.key
COPY ssl/praxeosu.czCA.crt /etc/nginx/ssl/praxeosuczCA.crt

RUN chmod 644 /etc/nginx/ssl/praxeosucz.crt 
RUN chmod 644 /etc/nginx/ssl/praxeosuczCA.crt 
RUN chmod 600 /etc/nginx/ssl/private/praxeosucz.key

# Konfigurace NGINX pro HTTPS
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Exponuj HTTPS port
EXPOSE 443

# Spusù NGINX
CMD ["nginx", "-g", "daemon off;"]