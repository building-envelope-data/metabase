# Inspired by https://github.com/brainsam/pgbouncer/blob/master/Dockerfile

#--------------------------------------------------
FROM alpine:3.13.3 AS build
#--------------------------------------------------

WORKDIR /

RUN \
  apk add --no-cache \
    autoconf \
    automake \
    build-base \
    c-ares-dev \
    libevent-dev \
    libtool \
    m4 \
    openssl-dev \
    py3-pip \
    python3

RUN \
  pip3 install --no-cache-dir \
    docutils && \
  wget \
    -O - \
    https://www.pgbouncer.org/downloads/files/1.15.0/pgbouncer-1.15.0.tar.gz \
    | tar xzf - && \
  cd /pgbouncer-1.15.0 && \
  ./configure --prefix=/pgbouncer && \
  make && \
  make install

WORKDIR /bin

RUN \
  ln -s \
    ../usr/bin/rst2man.py \
    rst2man

#--------------------------------------------------
FROM alpine:3.13.3
#--------------------------------------------------

RUN \
  apk add --no-cache \
    c-ares \
    libevent \
    openssl

WORKDIR /

COPY \
  --from=build \
  /pgbouncer \
  /pgbouncer

ADD entrypoint.sh ./

ENTRYPOINT ["./entrypoint.sh"]
