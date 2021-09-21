FROM node:14.17.0 as build

WORKDIR /app

COPY /basicbanking.web/package.json /app/package.json

RUN npm install --slient --loglevel=error
RUN npm install -g @angular/cli@12.2.0

COPY basicbanking.web/ /app

RUN ng build

# base image 
FROM nginx:1.21.3-alpine

# copy artifact build from the 'build environment' 
COPY --from=build /app/dist /usr/share/nginx/html 

COPY /basicbanking.web/default.conf /etc/nginx/conf.d/default.conf 

# expose port 80 
EXPOSE 80

# run nginx 
CMD ["nginx", "-g", "daemon off;"] 
