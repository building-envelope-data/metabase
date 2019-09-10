FROM python:3.7.4-alpine3.10

ARG GROUP_ID=1000
ARG GROUP_NAME=me
ARG USER_ID=1000
ARG USER_NAME=me

# Create non-root user to run commands in (see https://medium.com/@mccode/processes-in-containers-should-not-run-as-root-2feae3f0df3b)
RUN addgroup -S -g ${GROUP_ID} ${GROUP_NAME} && \
    adduser -S -D -u ${USER_ID} -G ${GROUP_NAME} ${USER_NAME}

#############
# As `root` #
#############

ENV PYTHONUNBUFFERED 1

RUN mkdir /app

WORKDIR /app

COPY requirements.txt .

# Inspired by https://stackoverflow.com/questions/46711990/error-pg-config-executable-not-found-when-installing-psycopg2-on-alpine-in-dock/47871121#47871121
RUN apk add --no-cache \
      dumb-init \
      postgresql-libs && \
    apk add --no-cache \
      --virtual .build-deps \
      gcc \
      musl-dev \
      postgresql-dev && \
    pip install --no-cache-dir \
      -r requirements.txt && \
    apk del --purge .build-deps

ENV HOME=/home/${USER_NAME}

RUN ln -fs ${HOME}/app /app

#####################
# As `${USER_NAME}` #
#####################

USER ${USER_NAME}

WORKDIR /app

VOLUME /app

ENTRYPOINT [ "/usr/bin/dumb-init", "--" ]
CMD [ "ash" ]
