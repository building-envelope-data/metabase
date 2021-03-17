# Inspired by https://mherman.org/blog/dockerizing-a-react-app/
FROM node:14.15.4-alpine3.12

ARG GROUP_ID
ARG USER_ID
ARG CERTIFICATE_AUTHORITY_BASE_FILE_NAME

# TODO directory node_modules is created on host machine when container is run
# and does not belong to current user. Avoid it being created at all or at
# least make it belong to the current user

#############
# As `root` #
#############

# `ca-certificates` is needed to run `update-ca-certificates`
# `openssl` is needed to debug SSL certificates
RUN \
  apk add --no-cache \
    ca-certificates \
    dumb-init \
    make \
    openssl

COPY \
  ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
  /usr/local/share/ca-certificates
RUN \
  update-ca-certificates

ENV HOME=/home/node
RUN \
  mkdir --parents ${HOME}/app && \
  chown \
    node:node \
    ${HOME}/app && \
  ln -s ${HOME}/app /app

#############
# As `node` #
#############
USER node
WORKDIR /app

ENV PATH=/app/node_modules/.bin:$PATH
COPY \
  --chown=node:node \
  ./package.json ./
COPY \
  --chown=node:node \
  ./yarn.lock ./
RUN yarn install

EXPOSE 3000
ENTRYPOINT ["/usr/bin/dumb-init", "--"]
