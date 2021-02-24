Clone the project and initialize submodules by running
```shell
git clone git@gitlab.cc-asp.fraunhofer.de:ise621/icon.git
cd icon
git submodule init
git submodule update
```
For an introduction to submodules see
[Git submodules](https://www.atlassian.com/git/tutorials/git-submodule)

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

# ...

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
