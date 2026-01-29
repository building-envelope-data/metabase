# syntax=docker/dockerfile:1.21
# check=error=true;skip=SecretsUsedInArgOrEnv
# The first line fixes the Dockerfile frontend used by BuildKit. For details
# see https://github.com/moby/buildkit/blob/master/frontend/dockerfile/docs/reference.md
# Available versions are listed on https://hub.docker.com/r/docker/dockerfile

# Inspired by https://github.com/dotnet/dotnet-docker/tree/master/samples/aspnetapp

# https://hub.docker.com/_/microsoft-dotnet-sdk
# https://mcr.microsoft.com/artifact/mar/dotnet/sdk/tags
FROM mcr.microsoft.com/dotnet/sdk:10.0-noble-amd64
SHELL ["/bin/bash", "-o", "errexit", "-o", "nounset", "-o", "pipefail", "-c"]

ARG GROUP_ID
ARG USER_ID
ARG CERTIFICATE_AUTHORITY_BASE_FILE_NAME
ARG CERTIFICATE_AUTHORITY_PASSWORD

RUN \
  if [ -z "$GROUP_ID" ]; then echo "required 'GROUP_ID'"; exit 1; fi && \
  if [ -z "$USER_ID" ]; then echo "required 'USER_ID'"; exit 1; fi && \
  if [ -z "$CERTIFICATE_AUTHORITY_BASE_FILE_NAME" ]; then echo "required 'CERTIFICATE_AUTHORITY_BASE_FILE_NAME'"; exit 1; fi && \
  if [ -z "$CERTIFICATE_AUTHORITY_PASSWORD" ]; then echo "required 'CERTIFICATE_AUTHORITY_PASSWORD'"; exit 1; fi

# Create non-root user to run commands in (see https://medium.com/@mccode/processes-in-containers-should-not-run-as-root-2feae3f0df3b)
# id --user --name ${USER_ID} 2>/dev/null
RUN \
  apt-get update && \
  apt-get install \
    --assume-yes \
    --no-install-recommends \
    adduser && \
  existing_user_name="$( (getent passwd ${USER_ID} 2>/dev/null || true) | cut --delimiter=: --fields=1)" && \
  if test -n "${existing_user_name}"; then \
    deluser "${existing_user_name}"; \
  fi && \
  existing_group_name="$( (getent group ${GROUP_ID} 2>/dev/null || true) | cut --delimiter=: --fields=1)" && \
  if test -n "${existing_group_name}"; then \
    delgroup "${existing_group_name}"; \
  fi && \
  addgroup \
    --system \
    --gid ${GROUP_ID} \
    us && \
  adduser \
    --system \
    --home /home/me \
    --uid ${USER_ID} \
    --ingroup us \
    me && \
  apt-get remove \
    --assume-yes \
    adduser && \
  rm \
    --recursive \
    --force \
    /var/lib/apt/lists/*

#############
# As `root` #
#############

# `editorconfig` can be used to debug ./backend/.editorconfig used for example
# by `dotnet format` and analyzers run by `dotnet build`. See
# https://github.com/editorconfig/editorconfig/issues/358
#
# `lldb` is needed by dotnet tool `dotnet-sos`, see
# https://github.com/dotnet/diagnostics#net-core-diagnostics-repo
# `openssl` is needed to debug SSL certificates
#
# Tracing cannot only be done with `dotnet-trace`, which is connected to
# `dotnet-sos`, but also with LTTng, see
# https://docs.microsoft.com/en-us/dotnet/core/diagnostics/logging-tracing#logging-events
# and
# https://github.com/dotnet/coreclr/blob/master/Documentation/project-docs/linux-performance-tracing.md#collecting-a-trace
# and
# https://stackoverflow.com/questions/55477684/how-to-configure-network-tracing-dotnet-core-for-httpclient-calls/58700657#58700657
# and
# https://lttng.org/docs/#doc-tracing-your-own-user-application
# For best practices on using `apt-get` see
# https://docs.docker.com/develop/develop-images/dockerfile_best-practices/#run
RUN \
  apt-get update && \
  apt-get install \
    --assume-yes \
    --no-install-recommends \
    curl \
    editorconfig \
    gawk \
    gnupg2 \
    lldb \
    make \
    # npm \
    openssl \
    procps \
    tini && \
  rm \
    --recursive \
    --force \
    /var/lib/apt/lists/*

# Add dependencies for interactive examples in ./examples
# `jq` can be used to format and filter JSON data
RUN \
  apt-get update && \
  apt-get install \
    --assume-yes \
    --no-install-recommends \
    bash \
    jq \
    neovim && \
  rm \
    --recursive \
    --force \
    /var/lib/apt/lists/*

# Inspired by https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust/59702094#59702094
# See also https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
# and https://github.com/dotnet/runtime/issues/31237#issuecomment-544929504
COPY \
  ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
  /usr/local/share/ca-certificates
RUN \
  update-ca-certificates

# Install up-to-date GnuPG from the official GnuPG repository.
# For available versions and the signing-key fingerprint see
# https://repos.gnupg.org/deb/gnupg/noble/
# Keep the version in sync with the one in the other Dockerfiles of this
# repository and with the one of the `machine` repository specified in the
# `local.yml` Ansible script.
ENV GNUPG_VERSION=2.5.17-1
ENV GNUPG_FINGERPRINT=32097B719B3745D6E61DDA1B85C45AE3E1A2B355
# hadolint ignore=SC2016 # Ignore "Expressions don't expand in single quotes, use double quotes for that" because we do not want `$VERSION_CODENAME` to expand.
RUN \
  version_codename="$(env --ignore-environment bash -c '. /etc/os-release; echo $VERSION_CODENAME')" && \
  apt-get update && \
  apt-get install \
    --assume-yes \
    --no-install-recommends \
    curl \
    gnupg2 && \
  curl \
    --silent \
    --show-error \
    "https://repos.gnupg.org/deb/gnupg/${version_codename}/gnupg-signing-key.gpg" \
    | gpg --dearmor \
    > /usr/share/keyrings/gnupg-keyring.gpg && \
  fingerprint=$(gpg \
    --batch \
    --with-colons \
    --import-options show-only \
    --import /usr/share/keyrings/gnupg-keyring.gpg \
    | awk -F':' '$1=="fpr"{print $10}') && \
  if [ "${fingerprint}" != "${GNUPG_FINGERPRINT}" ]; then \
      echo "Error: Key fingerprint mismatch, got ${fingerprint} and expected ${GNUPG_FINGERPRINT}"; \
      exit 1; \
  fi && \
  echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/gnupg-keyring.gpg] https://repos.gnupg.org/deb/gnupg/${version_codename}/ ${version_codename} main" \
    > /etc/apt/sources.list.d/gnupg.list && \
  apt-get update && \
  apt-get install \
    --assume-yes \
    --no-install-recommends \
    dirmngr \
    gnupg2=${GNUPG_VERSION} && \
  rm \
    --recursive \
    --force \
    /var/lib/apt/lists/*

ENV HOME=/home/me
RUN \
  mkdir --parents ${HOME}/app && \
  chown \
    me:us \
    ${HOME} && \
  chown \
    me:us \
    ${HOME}/app && \
  ln --symbolic ${HOME}/app /app

###########
# As `me` #
###########
USER me
WORKDIR /app

# Install the tools specified in the manifest file `dotnet-tools.json`.
# For a comprehensive list of tools see https://github.com/natemcmaster/dotnet-tools
# Diagnostics is provided by `counters`, `dump`, and `trace`, see https://devblogs.microsoft.com/dotnet/introducing-diagnostics-improvements-in-net-core-3-0/
# Code formatting is provided by `format`
# Code generation is provided by `aspnet-codegenerator`
# Database migrations are provided by `ef`
# Read-Evaluate-Print-Loop (REPL) is provided by `script`, see https://github.com/filipw/dotnet-script#repl
# Information on the `dotnet tool` command-line interface can be found under
# * https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install
# * https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools#install-a-local-tool
COPY \
  --chown=me:us \
  ./dotnet-tools.json ./
COPY \
  --chown=me:us \
  ./dotnet-certificate-tool ./dotnet-certificate-tool
RUN \
  # `dotnet pack` is run automatically by `dotnet build` because `GeneratePackageOnBuild` is set to `True`.
  dotnet build \
    ./dotnet-certificate-tool/src/GSoft.CertificateTool.csproj && \
  dotnet tool restore \
    --tool-manifest ./dotnet-tools.json \
    --add-source ./dotnet-certificate-tool/src/nupkg && \
  rm --recursive --force \
    ./dotnet-certificate-tool

RUN \
  mkdir ./src && \
  mkdir ./test
COPY \
  --chown=me:us \
  ./src/Metabase.csproj \
  ./src/
COPY \
  --chown=me:us \
  ./test/Metabase.Tests.csproj \
  ./test/

# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.alpine-x64
# https://github.com/dotnet/dotnet-docker/blob/master/samples/run-aspnetcore-https-development.md
# https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=linux
#
# https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541138051
# https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
#
# `dotnet dev-certs` does not generate certificates that work in Linux
# distributions
# RUN \
  # dotnet dev-certs https --export-path ./.secrets/https/icon.pfx --password crypticpassword && \

COPY \
  --chown=me:us \
  ./Metabase.sln \
  ./
RUN \
  dotnet clean && \
  dotnet restore && \
  dotnet tool run dotnet-sos install # https://github.com/dotnet/diagnostics/blob/master/documentation/installing-sos-instructions.md

# Add the certificate authority PKCS#12 file to the X.509 certificate store for
# trusted root certificate authorities named `Root`, see
# https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.storename?view=netcore-3.1#fields
# for the current user, see
# https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.storelocation?view=netcore-3.1#fields
# essentially using `X509Store.Add`, see
# https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509store.add?view=netcore-3.1
#
# In C# code with .NET Core and hard-coded file name and password this reads
# ```
# using (var store = new System.Security.Cryptography.X509Certificates.X509Store(System.Security.Cryptography.X509Certificates.StoreName.Root, System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser, System.Security.Cryptography.X509Certificates.OpenFlags.ReadWrite))
#     {
#         var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2("/app/ssl/ca.pfx", "ca-password", System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.DefaultKeySet);
#         store.Add(cert);
#     }
# ```
# The above code is used in the dotnet tool `certificate-tool`, see
# https://github.com/gsoft-inc/dotnet-certificate-tool
# which we use below to add the certificate.
#
# What I don't know is why the certificate authority
# `${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt` added above using
# `update-ca-certificates` is not contained in the `Root` store of the
# `LocalMachine` used by .NET Core. Where does this store get its certificates
# from if not from `/etc/ssl`? Using `update-ca-certificates` was enough to
# make the command-line tool `cURL` trust
# `${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt` but not .NET Core `HttpClient`.
#
# To debug SSL errors when using `HttpClient` that only say something vague
# like `the remote certificate is invalid`, you can hook into the callback
# `HttpClientHandler#ServerCertificateCustomValidationCallback` to get detailed
# information about the problem:
# ```
# var content = new System.Net.Http.StringContent("{ \"query\": \"\" }");
# content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
# var handler = new System.Net.Http.HttpClientHandler();
# handler.ServerCertificateCustomValidationCallback +=
#   (sender, cert, chain, sslPolicyErrors) => {
#       Console.WriteLine(sender);
#       Console.WriteLine(cert);
#       Console.WriteLine(chain);
#       foreach (var s in chain.ChainStatus)
#         {
#         Console.WriteLine(s.Status);
#         Console.WriteLine(s.StatusInformation);
#         }
#       Console.WriteLine(sslPolicyErrors);
#       return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
#   };
# var client = new System.Net.Http.HttpClient(handler);
# var response =
#    await client
#    .PostAsync(
#    "https://local.buildingenvelopedata.org:4041/graphql/",
#    content
#    );
# ```
#
# To print existing and non-existing stores and their certificates use
# ```
# using System.Security.Cryptography;
# using System.Security.Cryptography.X509Certificates;
# foreach (var storeLocation in (StoreLocation[]) Enum.GetValues(typeof(StoreLocation)))
# {
#     foreach (var storeName in (StoreName[]) Enum.GetValues(typeof(StoreName)))
#     {
#         using (var store = new X509Store(storeName, storeLocation))
#         {
#             try
#             {
#                 store.Open(OpenFlags.OpenExistingOnly);
#                 Console.WriteLine($"# Store {store.Name}, {store.Location}, exists and has {store.Certificates.Count} certificate(s)");
#                 if (store.Certificates.Count >= 1)
#                 {
#                     foreach (var x509 in store.Certificates)
#                     {
#                         Console.WriteLine("----------------------------------------");
#                         Console.WriteLine(x509);
#                         try
#                         {
#                             byte[] rawdata = x509.RawData;
#                             Console.WriteLine("Content Type: {0}", X509Certificate2.GetCertContentType(rawdata));
#                             Console.WriteLine("Friendly Name: {0}", x509.FriendlyName);
#                             Console.WriteLine("Certificate Verified?: {0}", x509.Verify());
#                             Console.WriteLine("Simple Name: {0}", x509.GetNameInfo(X509NameType.SimpleName, true),Environment.NewLine);
#                             Console.WriteLine("Signature Algorithm: {0}", x509.SignatureAlgorithm.FriendlyName);
#                             if (x509.SignatureAlgorithm.FriendlyName.EndsWith("ECDSA"))
#                             {
#                                 Console.WriteLine("Private Key: {0}", x509.GetECDsaPrivateKey()?.ToString());
#                                 Console.WriteLine("Public Key: {0}", x509.GetECDsaPublicKey()?.ToString());
#                             }
#                             else
#                             {
#                                 Console.WriteLine("Private Key: {0}", x509.PrivateKey?.ToXmlString(false));
#                                 Console.WriteLine("Public Key: {0}", x509.PublicKey?.Key?.ToXmlString(false));
#                             }
#                             Console.WriteLine("Certificate Archived?: {0}", x509.Archived);
#                             Console.WriteLine("Length of Raw Data: {0}", x509.RawData.Length);
#                             x509.Reset();
#                         }
#                         catch (CryptographicException)
#                         {
#                             Console.WriteLine("Information could not be written out for this certificate.");
#                         }
#                     }
#                 }
#             }
#             catch (CryptographicException)
#             {
#                 Console.WriteLine($"Store {store.Name}, {store.Location}, does not exist");
#             }
#         }
#         Console.WriteLine("========================================");
#     }
# }
# ```
RUN \
  mkdir ./ssl
COPY \
  --chown=me:us \
  ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.pfx \
  ./ssl/
RUN \
  dotnet tool run \
    certificate-tool add \
      --file ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.pfx \
      --store-name Root \
      --password ${CERTIFICATE_AUTHORITY_PASSWORD}

# bash parameter completion for the dotnet CLI
# see https://learn.microsoft.com/en-us/dotnet/core/tools/enable-tab-autocomplete#bash
# for the here-document syntax used to append to the ~/.bashrc file
# see https://pubs.opengroup.org/onlinepubs/9699919799/utilities/V3_chap02.html#tag_18_07_04
# hadolint ignore=DL3059 # Multiple consecutive `RUN` instructions.
RUN \
  cat <<-"EOT" >> ${HOME}/.bashrc
		function _dotnet_bash_complete()
		{
		  local cur="${COMP_WORDS[COMP_CWORD]}" IFS=$'\n'
		  local candidates
		  read -d '' -ra candidates < <(dotnet complete --position "${COMP_POINT}" "${COMP_LINE}" 2>/dev/null)
		  read -d '' -ra COMPREPLY < <(compgen -W "${candidates[*]:-}" -- "$cur")
		}
		complete -f -F _dotnet_bash_complete dotnet
EOT

# COPY \
#   --chown=me:us \
#   ./package.json ./package-lock.json ./
# RUN npm install

# Create empty directories `/app/*/bin`, `/app/*/obj`, and
# `${HOME}/.vscode-server/*` for user `me` and group `us` to make the
# respective mounted volumes be owned by the user `me` and the group `us`.
# For an explanation for the Visual Studio Code Server directories see
# https://code.visualstudio.com/docs/remote/containers-advanced#_avoiding-extension-reinstalls-on-container-rebuild
RUN \
  mkdir --parents \
    ./src/logs && \
  mkdir --parents \
    ./src/bin \
    ./src/obj && \
  mkdir --parents \
    ./test/bin \
    ./test/obj && \
  mkdir --parents \
    ${HOME}/.vscode-server \
    ${HOME}/.vscode-server-insiders && \
  mkdir --parents \
    ${HOME}/.nuget \
    ${HOME}/.dotnet && \
  mkdir --parents \
    ${HOME}/.gnupg && \
  chmod u=rwx,g=,o= \
    ${HOME}/.gnupg && \
  echo "allow-loopback-pinentry" \
    > ${HOME}/.gnupg/gpg-agent.conf

ENV SHELL=/bin/bash

ENTRYPOINT ["/usr/bin/tini", "--"]
