# Metabase

The network of databases [buildingenvelopedata.org](https://www.buildingenvelopedata.org/) is based on [databases](https://github.com/building-envelope-data/database) and one metabase. This repository presents the source code of the metabase. Before deploying this repository, [machine](https://github.com/building-envelope-data/machine) can be used to set up the machine. Databases and metabase use this [API specification](https://github.com/building-envelope-data/api).

If you have a question for which you don't find the answer in this repository, please raise a [new issue](https://github.com/building-envelope-data/metabase/issues/new) and add the tag `question`! All ways to contribute are presented by [CONTRIBUTING.md](https://github.com/building-envelope-data/metabase/blob/develop/CONTRIBUTING.md). The basis for our collaboration is decribed by our [Code of Conduct](https://github.com/building-envelope-data/metabase/blob/develop/CODE_OF_CONDUCT.md).

## Getting started

### On your Linux machine

1. Open your favorite shell, for example, good old
   [Bourne Again SHell, aka, `bash`](https://www.gnu.org/software/bash/),
   the somewhat newer
   [Z shell, aka, `zsh`](https://www.zsh.org/),
   or shiny new
   [`fish`](https://fishshell.com/).
1. Install [Git](https://git-scm.com/) by running
   `sudo apt install git-all` on [Debian](https://www.debian.org/)-based
   distributions like [Ubuntu](https://ubuntu.com/), or
   `sudo dnf install git` on [Fedora](https://getfedora.org/) and closely-related
   [RPM-Package-Manager](https://rpm.org/)-based distributions like
   [CentOS](https://www.centos.org/). For further information see
   [Installing Git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git).
1. Clone the source code by running
   `git clone git@github.com:building-envelope-data/metabase.git` and navigate
   into the new directory `metabase` by running `cd metabase`.
1. Initialize, fetch, and checkout possibly-nested submodules by running
   `git submodule update --init --recursive`. An alternative would have been
   passing `--recurse-submodules` to `git clone` above.
1. Prepare your environment by running `cp .env.sample .env`,
   `cp frontend/.env.local.sample frontend/.env.local`, and adding the line
   `127.0.0.1 local.buildingenvelopedata.org` to your `/etc/hosts` file.
1. Install [Docker Desktop](https://www.docker.com/products/docker-desktop), and
   [GNU Make](https://www.gnu.org/software/make/).
1. List all GNU Make targets by running `make help`.
1. Generate and trust a self-signed certificate authority and SSL certificates
   by running `make ssl`.
1. Generate JSON Web Token (JWT) encryption and signing certificates by running
   `make jwt-certificates`.
1. Start all services and follow their logs by running `make up logs`.
1. To see the web frontend navigate to
   `https://local.buildingenvelopedata.org:4041` in your web browser, to see
   the GraphQL API navigate to
   `https://local.buildingenvelopedata.org:4041/graphql/`, and to see sent
   emails navigate to
   `https://local.buildingenvelopedata.org:4041/email/`.

In another shell

1. Drop into `bash` with the working directory `/app`, which is mounted to the
   host's `./backend` directory, inside a fresh Docker container based on
   `./backend/Dockerfile` by running `make shellb`. If necessary, the Docker
   image is (re)built automatically, which takes a while the first time.
1. List all backend GNU Make targets by running `make help`.
1. For example, update packages and tools by running `make update`.
1. Drop out of the container by running `exit` or pressing `Ctrl-D`.

The same works for frontend containers by running `make shellf`.

## Deployment

For information on using Docker in production see
[Configure and troubleshoot the Docker daemon](https://docs.docker.com/config/daemon/)
and the pages following it.

### Setting up a Debian production machine

1. Use the sibling project [machine](https://github.com/building-envelope-data/machine) and its
   instructions for the first stage of the set-up.
1. Enter a shell on the production machine using `ssh`.
1. Change into the directory `/app` by running `cd /app`.
1. Clone the repository twice by running
   ```
   for environment in staging production ; do
     git clone git@github.com:building-envelope-data/metabase.git ./${environment}
   done
   ```
1. For each of the two environments staging and production referred to by
   `${environment}` below:
   1. Change into the clone `${environment}` by running `cd /app/${environment}`.
   1. Prepare the environment by running `cp .env.${environment}.sample .env`,
      `cp frontend/.env.local.sample frontend/.env.local`, and by replacing
      dummy passwords in the copies by newly generated ones, where random
      passwords may be generated running `openssl rand -base64 32`.
   1. Prepare PostgreSQL by generating new password files by running
      `make --file Makefile.production postgres_passwords`
      and creating the database by running
      `make --file Makefile.production createdb`.

### Creating a release

1. [Draft a new release](https://github.com/building-envelope-data/metabase/actions/workflows/draft-new-release.yml)
   with a new version according to [Semantic Versioning](https://semver.org) by
   running the GitHub action which, in particular, creates a new branch named
   `release/v*.*.*`, where `*.*.*` is the version, and a corresponding pull
   request.
1. Fetch the release branch by running `git fetch` and check it out by running
   `git checkout release/v*.*.*`, where `*.*.*` is the version.
1. Prepare the release by running `make prepare-release` in your shell, review,
   add, commit, and push the changes. In particular, migration and rollback SQL
   files are created in `./backend/src/Migrations/` which need to be reviewed
   --- see
   [Migrations Overview](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
   and following pages for details.
1. [Publish the new release](https://github.com/building-envelope-data/metabase/actions/workflows/publish-new-release.yml)
   by merging the release branch into `main` whereby a new pull request from
   `main` into `develop` is created that you need to merge to finish off.

### Deploying a release

1. Enter a shell on the production machine using `ssh`.
1. Change to the staging envrionment by running `cd /app/staging`.
1. Deploy the new release in the staging environment by running
   `make --file Makefile.production deploy`.
1. If it fails _after_ the database backup was made, rollback to the previous
   state by running
   `make --file Makefile.production rollback`,
   figure out what went wrong, apply the necessary fixes to the codebase,
   create a new release, and try to deploy that release instead.
1. If it succeeds, deploy the new reverse proxy that handles sub-domains by
   running `cd /app/machine && make deploy` and test whether everything works
   as expected and if that is the case, repeat all stages but this one in the
   directory `/app/production` (instead of `/app/staging`). Note that in the
   staging environment sent emails can be viewed in the web browser under
   `https://staging.buildingenvelopedata.org/email/` and emails to addresses in
   the variable `RELAY_ALLOWED_EMAILS` in the `.env` file are delivered to the
   respective inboxes (the variable's value is a comma separated list of email
   addresses).

### Troubleshooting

The file `Makefile.production` contains GNU Make targets to manage Docker
containers like `up` and `down`, to follow Docker container logs with `logs`,
to drop into shells inside running Docker containers like `shellb` for the
backend service and `shellf` for the frontend service and `psql` for the
databse service, and to list information about Docker like `list` and
`list-services`.

And the file contains GNU Make targets to deploy a new release or rollback it
back as mentioned above. These targets depend on several smaller targets like
`begin-maintenance` and `end-maintenance` to begin or end displaying
maintenance information to end users that try to interact with the website, and
`backup` to backup all data before deploying a new version, `migrate` to
migrate the database, and `run-tests` to run tests.

If for some reason the website displays the maintenance page without
maintenance happening at the moment, then drop into a shell on the production
machine, check all logs for information on what happened, fix issues if
necessary, and end maintenance. It could for example happen that a cron job
set-up by [machine](https://github.com/building-envelope-data/machine) begins
maintenance, fails to do its actual job, and does not end maintenance
afterwards. Whether failing to do its job is a problem for the inner workings
of the website needs to be decided by some developer. If it for example backing
up the database fails because the machine is out of memory at the time of doing
the backup, the website itself should still working.

## Original Idea

The product identifier service should provide the following endpoints:

- Obtain a new product identifier possibly associating internal meta information with it, like a custom string or a JSON blob
- Update the meta information of one of your product identifiers
- Get meta information of one of your product identifiers
- Get the owner of a product identifier (needed, for example, by the IGSDB to check that the user adding product data with a product identifier owns the identifier)
- List all your product identifiers
- Request the transferal of the ownership of one or all of your product identifiers to another (once the receiving user agrees, the transferal is made)
- Respond to a transferal request

How to obtain a unique product identifier and add product data to some database:

1. Create an account at a central authentication service, that is, a domain specific and lightweight service like [Auth0](https://auth0.com) managed by us (the details of how users prove to be a certain manufacturer are still open)
2. Authenticate yourself at the authentication service receiving a [JSON web token](https://jwt.io) (this could be a simple username/password authentication scheme)
3. Obtain a new product identifier from the respective service passing your JSON web token as means of authentication
4. Add product data to some database like IGSDB passing the product identifier and your JSON web token

JSON web tokens are used for authentication across different requests, services, and domains.

Product identifiers are randomly chosen and verified to be unique 32, 48, or 64 bit numbers, which can be communicated for easy human use as [proquints](https://arxiv.org/html/0901.4016) [there are implementations in various languages](https://github.com/dsw/proquint). We could alternatively use [version 4 universally-unique identifiers](https://tools.ietf.org/html/rfc4122); I consider this to be overkill as it comes with a performance penalty and our identifiers do not need to be universally unique. Either way, [those identifiers do _not_ replace primary keys](https://tomharrisonjr.com/uuid-or-guid-as-primary-keys-be-careful-7b2aa3dcb439).

Randomness of identifiers ensures that

- the product identifier does not encode any information regarding the product, like its manufacturer, which would, for example, be problematic when one manufacturer is bought by another
- a user cannot run out of product identifiers, because there is no fixed range of possible identifiers per user
- it's unlikely that flipping one bit or replacing one letter in the proquint representation by another results in a valid identifier owned by the same user

We may add some error detection and correction capabilities by, for example, generating all but the last 4 bits randomly and using the last 4 bits as [some sort of checksum](https://en.wikipedia.org/wiki/Checksum).

## ...

- [Designing GraphQL Mutations](https://www.apollographql.com/blog/graphql/basics/designing-graphql-mutations/)
- [Updating Enum Values in PostgreSQL - The Safe and Easy Way](https://blog.yo1.dog/updating-enum-values-in-postgresql-the-safe-and-easy-way/)
- [C# Coding Standards](https://www.dofactory.com/reference/csharp-coding-standards)
- [Should You Use The Same Dockerfile For Dev, Staging And Production Builds?](https://vsupalov.com/same-dockerfile-dev-staging-production/)
- [Dockerizing a React App](https://mherman.org/blog/dockerizing-a-react-app/)
- [A starting point for Clean Architecture with ASP.NET Core](https://github.com/ardalis/CleanArchitecture)
  [Clean Architecture Manga](https://github.com/ivanpaulovich/clean-architecture-manga)
  [Northwind Traders](https://github.com/JasonGT/NorthwindTraders)
  [Building ASP.NET Core Web APIs with Clean Architecture](https://fullstackmark.com/post/18/building-aspnet-core-web-apis-with-clean-architecture)
  [The Equinox Project](https://github.com/EduardoPires/EquinoxProject)
- [Use a third-party dependency injection (DI) container or pure DI](https://stackoverflow.com/questions/30681477/why-would-one-use-a-third-party-di-container-over-the-built-in-asp-net-core-di-c/30682214#30682214), maybe use [Autofac](https://autofac.org/), for others see [ultimate list](Shttps://www.claudiobernasconi.ch/2019/01/24/the-ultimate-list-of-net-dependency-injection-frameworks/)
- [API design best practices](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)
  [API implementation best practices](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-implementation)
  [Shallow Nesting](https://guides.rubyonrails.org/routing.html#shallow-nesting)
- [Implicit Flow](https://auth0.com/docs/flows/concepts/implicit)
- Our domain model is inspired by
  [AnemicDomainModels](https://github.com/vkhorikov/AnemicDomainModel/tree/master/After/src/Logic/Customers)
  and by
  [Parse, don't validate](https://lexi-lambda.github.io/blog/2019/11/05/parse-don-t-validate/)
  and by
  [Type Safety Back and Forth](https://www.parsonsmatt.org/2017/10/11/type_safety_back_and_forth.html)
- A hands-on introduction to regular expressions is [Everything you need to know about Regular Expressions](https://towardsdatascience.com/everything-you-need-to-know-about-regular-expressions-8f622fe10b03)
- [Unit testing best practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
  and
  [Naming standards for unit tests](https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html)
- [OAuth 2.0 Simplified](https://www.oauth.com)
- [Setting up an Authorization Server with OpenIddict](https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-iv-authorization-code-flow-3eh8)
- [Bearer Token Authentication in ASP.NET Core](https://devblogs.microsoft.com/aspnet/bearer-token-authentication-in-asp-net-core/)
