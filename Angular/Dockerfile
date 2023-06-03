# Stage 1
FROM node:16.10-alpine as node

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build  --prod

# Stage 2
FROM nginx:alpine

COPY --from=node /app/dist/demo-app /usr/share/nginx/html
