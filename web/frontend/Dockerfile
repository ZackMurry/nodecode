FROM node:18-alpine3.15
ENV NODE_OPTIONS=--openssl-legacy-provider
WORKDIR /usr/src/frontend
COPY package.json package-lock.json ./
RUN npm i
COPY . ./
RUN npm run build
ENTRYPOINT ["npm", "run", "start"]

