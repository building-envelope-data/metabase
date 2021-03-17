# Inspired by https://github.com/dotnet/dotnet-docker/tree/master/samples/aspnetapp
# See https://hub.docker.com/_/microsoft-dotnet-core-sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0.100-alpine3.12-amd64

ARG GROUP_ID
ARG USER_ID
ARG CERTIFICATE_AUTHORITY_BASE_FILE_NAME
ARG CERTIFICATE_AUTHORITY_PASSWORD

# Create non-root user to run commands in (see https://medium.com/@mccode/processes-in-containers-should-not-run-as-root-2feae3f0df3b)
RUN \
  addgroup -S -g ${GROUP_ID} us && \
  adduser -S -D -u ${USER_ID} -G us me

#############
# As `root` #
#############

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
RUN \
  apk add --no-cache \
    curl \
    dumb-init \
    gawk \
    lldb \
    make \
    # npm \
    openssl

# Add dependencies for interactive examples in ./examples
# `jq` can be used to format and filter JSON data
RUN \
  apk add --no-cache \
    bash \
    jq \
    neovim

# Inspired by https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust/59702094#59702094
# See also https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
# and https://github.com/dotnet/runtime/issues/31237#issuecomment-544929504
COPY \
  ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
  /usr/local/share/ca-certificates
RUN \
  update-ca-certificates

ENV HOME=/home/me
RUN \
  mkdir --parents ${HOME}/app && \
  chown \
    me:us \
    ${HOME}/app && \
  ln -s ${HOME}/app /app

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
RUN \
  dotnet tool restore \
    --tool-manifest ./dotnet-tools.json

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
RUN \
  # dotnet dev-certs https --export-path ./.secrets/https/icon.pfx --password crypticpassword && \
  # dotnet user-secrets --project ./src/Metabase.csproj set "Kestrel:Certificates:Development:Password" "crypticpassword" && \
  dotnet user-secrets --project ./src/Metabase.csproj set SendGridUser myuser && \
  dotnet user-secrets --project ./src/Metabase.csproj set SendGridKey mykey

COPY \
  --chown=me:us \
  ./Metabase.sln \
  ./
RUN \
  dotnet clean && \
  dotnet restore --runtime linux-musl-x64 && \
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
#    "https://metabase.org:4041/graphql/",
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

# COPY \
#   --chown=me:us \
#   ./package.json ./package-lock.json ./
# RUN npm install

# Create empty directories `/app/*/bin` and `/app/*/obj` for user `me` and
# group `us` to make the respective mounted volumes be owned by the user `me`
# and the group `us`.
RUN \
  mkdir -p ./src/bin && \
  mkdir -p ./src/obj && \
  mkdir -p ./test/bin && \
  mkdir -p ./test/obj

ENTRYPOINT ["/usr/bin/dumb-init", "--"]
