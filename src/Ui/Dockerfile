FROM docker.io/node:23.10.0-alpine AS build
RUN apk update && apk upgrade libxml2
WORKDIR /app
COPY ["src/Ui/", "/app/"]
ENV NODE_ENV=production
RUN npm install npm -g \
    && npm install -g @angular/cli

##RUN npm install  @angular-devkit/build-angular --force
##RUN ng update @angular/cli @angular/core --allow-dirty --force 1>/dev/null 2>&1
RUN npm uninstall @angular-devkit/build-angular --legacy-peer-deps
RUN npm install  @angular-devkit/build-angular@13.3.11 --force
RUN npm run build
RUN ng build --configuration=test   --project=dms --base-href /
### STAGE 2: Run ###
FROM docker.io/nginx:1.27.4-alpine
RUN apk update && apk upgrade libxml2
COPY src/Ui/nginx-common.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist/dms /usr/share/nginx/html
