# Metabase

## Getting started

### On your Linux machine
1. Open your favorite shell, for example, good old
   [Bourne Again SHell, aka, `bash`](https://www.gnu.org/software/bash/),
   the somewhat newer
   [Z shell, aka, `zsh`](https://www.zsh.org/),
   or shiny new
   [`fish`](https://fishshell.com/).
2. Install [Git](https://git-scm.com/) by running
   `sudo apt install git-all` on [Debian](https://www.debian.org/)-based
   distributions like [Ubuntu](https://ubuntu.com/), or
   `sudo dnf install git` on [Fedora](https://getfedora.org/) and closely-related
   [RPM-Package-Manager](https://rpm.org/)-based distributions like
   [CentOS](https://www.centos.org/). For further information see
   [Installing Git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git).
3. Clone the source code by running
   `git clone git@github.com:ise621/metabase.git` and navigate
   into the new directory `metabase` by running `cd metabase`.
4. Prepare your environment by running `cp .env.sample .env`,
   `cp frontend/.env.local.sample frontend/.env.local`, and adding the line
   `127.0.0.1 local.buildingenvelopedata.org` to your `/etc/hosts` file.
5. Install [Docker Desktop](https://www.docker.com/products/docker-desktop), and
   [GNU Make](https://www.gnu.org/software/make/).
6. List all GNU Make targets by running `make help`.
7. Generate and trust a self-signed certificate authority and SSL certificates
   by running `make ssl`.
8. Generate JSON Web Token (JWT) encryption and signing certificates by running
   `make jwt-certificates`.
9. Start all services and follow their logs by running `make up logs`.
10. To see the web frontend navigate to
   `https://local.buildingenvelopedata.org:4041` in your web browser and to see
   the GraphQL API navigate to
   `https://local.buildingenvelopedata.org:4041/graphql/`.

In another shell
1. Drop into `ash` with the working directory `/app`, which is mounted to the
   host's `./backend` directory, inside a fresh Docker container based on
   `./backend/Dockerfile` by running `make shellb`.  If necessary, the Docker
   image is (re)built automatically, which takes a while the first time.
2. List all backend GNU Make targets by running `make help`.
3. For example, update packages and tools by running `make update`.
4. Drop out of the container by running `exit` or pressing `Ctrl-D`.

The same works for frontend containers by running `make shellf`.

## Setting up a Debian production machine
1. Install [Ansible](https://www.ansible.com) as explained on
   [Installing Ansible on Debian](https://docs.ansible.com/ansible/latest/installation_guide/intro_installation.html#installing-ansible-on-debian).
1. Change into the home directory by running `cd ~`.
1. Clone the repository twice by running
   ```
   for environment in staging production ; do
     git clone git@github.com:ise621/metabase.git ./${environment}
   done
   ```
1. Change into one clone by running `cd ./staging`
1. Set-up the machine by running `ansible-playbook ./machine/local.yml`.
1. Format and mount hard disk for data to the directory `/data` as follows:
   1. Figure out its name by running `lsblk` to figure out its name, for
      example, `sdb` and use this name instead of `sdx` below.
   1. Partition the hard disk `/dev/sdx` by running
      `sudo parted --align=opt /dev/sdx mklabel gpt`
      and
      `sudo parted --align=opt /dev/sdx mkpart primary 0 50G`
      or, if the command warns you that resulting partition is not properly
      aligned for best performance: 1s % 2048s != 0s,
      `sudo parted --align=opt /dev/sdx mkpart primary 2048s 50G`.
      If the number of sectors, 2048 above, is not resported, consult
      https://rainbow.chard.org/2013/01/30/how-to-align-partitions-for-best-performance-using-parted/
      for details on how to compute that number.
   1. Format the partition `/dev/sdx1` of hard disk `/dev/sdx` by running
      `sudo mkfs.ext4 -L data /dev/sdx1`
      and mount it permanently by adding
      `UUID=XXXX-XXXX-XXXX-XXXX-XXXX /data ext4 errors=remount-ro 0 1`
      to the file `/etc/fstab` and running
      `sudo mount --all`,
      where the UUID is the one reported by
      `sudo blkid | grep /dev/sdx1`.
      Note that to list block devices and whether and where they are
      mounted run `lsblk` and you could mount partitions temporarily by running
      `sudo mount /dev/sdx1 /data`.
1. For each of the two environments
   1. Prepare the environment in both clones more or less as detailed above
      replacing dummy passwords by newly generated ones, for example, by running
      `openssl rand -base64 32`,
      and adding the variable `NAME` to `.env` with the value
      `metabase_staging` or `metabase_production`.
   1. Prepare PostgreSQL by generating new password files by running
      `make --file Makefile.production postgres_passwords`
      and creating the database by running
      `make --file Makefile.production createdb`.
   1. Start all services by running
      `make --file Makefile.production up`.
   1. Restart changed services in a changed environment by running
      `make --file Makefile.production down build up`.

## Original Idea

The product identifier service should provide the following endpoints:
* Obtain a new product identifier possibly associating internal meta information with it, like a custom string or a JSON blob
* Update the meta information of one of your product identifiers
* Get meta information of one of your product identifiers
* Get the owner of a product identifier (needed, for example, by the IGSDB to check that the user adding product data with a product identifier owns the identifier)
* List all your product identifiers
* Request the transferal of the ownership of one or all of your product identifiers to another (once the receiving user agrees, the transferal is made)
* Respond to a transferal request

How to obtain a unique product identifier and add product data to some database:
1. Create an account at a central authentication service, that is, a domain specific and lightweight service like [Auth0](https://auth0.com) managed by us (the details of how users prove to be a certain manufacturer are still open)
2. Authenticate yourself at the authentication service receiving a [JSON web token](https://jwt.io) (this could be a simple username/password authentication scheme)
3. Obtain a new product identifier from the respective service passing your JSON web token as means of authentication
4. Add product data to some database like IGSDB passing the product identifier and your JSON web token

JSON web tokens are used for authentication across different requests, services, and domains.

Product identifiers are randomly chosen and verified to be unique 32, 48, or 64 bit numbers, which can be communicated for easy human use as [proquints](https://arxiv.org/html/0901.4016) [there are implementations in various languages](https://github.com/dsw/proquint). We could alternatively use [version 4 universally-unique identifiers](https://tools.ietf.org/html/rfc4122); I consider this to be overkill as it comes with a performance penalty and our identifiers do not need to be universally unique. Either way, [those identifiers do _not_ replace primary keys](https://tomharrisonjr.com/uuid-or-guid-as-primary-keys-be-careful-7b2aa3dcb439).

Randomness of identifiers ensures that
* the product identifier does not encode any information regarding the product, like its manufacturer, which would, for example, be problematic when one manufacturer is bought by another
* a user cannot run out of product identifiers, because there is no fixed range of possible identifiers per user
* it's unlikely that flipping one bit or replacing one letter in the proquint representation by another results in a valid identifier owned by the same user

We may add some error detection and correction capabilities by, for example, generating all but the last 4 bits randomly and using the last 4 bits as [some sort of checksum](https://en.wikipedia.org/wiki/Checksum).

## ...

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
