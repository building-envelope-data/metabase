workspace "Metabase" {

    model {
        user = person "Human"

        group "Clients" {
            window = softwareSystem "??? Berkeley Lab WINDOW ???" "A publicly available computer program for calculating total window thermal performance indices."
        }

        group "Metabase" {
            metabase = softwareSystem "Metabase" "Web Application managing identifiers in the format UUID that are unique across databases and broadcasting data queries to registered databases." {
                # url https://github.com/building-envelope-data/metabase
                # url https://www.buildingenvelopedata.org
                frontend = container "Single-Page Application (SPA)" "" "Language: TypeScript, Framework: Next.js" "Web Browser" {
                    react = component "React" "React lets you build user interfaces (UI) out of individual pieces called components."
                    nextjs = component "Next.js" "Next.js enables you to create full-stack Web applications by extending the latest React features."
                    antdesign = component "Ant Design" "Ant Design is an enterprise-class user interface design language and React library with a set of high-quality React components."
                    apollo = component "Apollo Client" "Apollo Client is a comprehensive state management library for JavaScript that enables you to manage both local and remote data with GraphQL."
                }
                # webApi = container "Web API" "GraphQL is a query language for APIs and a runtime for fulfilling those queries with your existing data." "GraphQL Schema Definition Language (SDL)"
                # openIdConnectApi = container "OpenId Connect API" "OpenId Connect is an identity layer built on top of the OAuth 2.0 framework. It allows third-party applications to verify the identity of the end-user and to obtain basic user profile information."
                middleTier = container "Middle Tier" "" "Language: C#, Framework: ASP.NET Core" {
                    graphQlServer = component "GraphQL Server" "Hot Chocolate takes the complexity away from building a fully-fledged GraphQL server." "Hot Chocolate" "API"
                    authorization = component "Authorization" "Authorization or authorisation is the function of specifying access rights/privileges to resources, which is related to general information security and computer security, and to access control in particular." "Functions"
                    domainModel = component "Domain Model" "A domain model is a representation of meaningful real-world concepts pertinent to the domain that need to be modeled in software." "Plain-Old Instances"
                    objectRelationalMapper = component "Object-Relational Mapper (ORM)" "An object-relational mapper enables .NET developers to work with a database using .NET objects." "Entity Framework Core"
                    openIdConnect = component "Single Sign-On (SSO)" "OpenId Connect is an identity layer built on top of the OAuth 2.0 framework. It allows third-party applications to verify the identity of the end-user and to obtain basic user profile information." "OpenIddict" "API"
                }
                database = container "Database" "PostgreSQL is a relational database management system emphasizing extensibility and SQL compliance." "PostgreSQL" "Database"
            }
        }

        group "Databases" {
            igsdb = softwareSystem "IGSDB"
            testlab = softwareSystem "TestLab Solar Facades" "Deployment of the database reference implementation." {
                # url https://github.com/building-envelope-data/database
                # url https://www.solarbuildingenvelopes.com
            }
        }

        user -> metabase "Uses Web Frontend" "Web Browser (HTTPS, HTML, JavaScript, Cascading Style Sheets)"
        window -> metabase "Could request API" "GraphQL (HTTPS Post, Request/Response Body: JSON)"
        metabase -> igsdb "Requests" "GraphQL"
        metabase -> testlab "Requests" "GraphQL (HTTPS Post, Request/Response Body: JSON)"

        # webApi -> frontend "GraphQL Responds (HTTPS Response)"
        # frontend -> webApi "Requests" "GraphQL (HTTPS Post)"
        # frontend -> openIdConnectApi "Signs-In" "HTTPS Get/Post"
        # middleTier -> webApi "Serves"
        # webApi -> middleTier "Is Served By" "Hot Chocolate (ASP.NET Core)"
        # openIdConnectApi -> middleTier "Is Served By" "OpenIddict (ASP.NET Core)"
        # database -> middleTier "Returns Rows"
        middleTier -> database "Queries" "Structured Query Language (Query: SQL, Result: CSV)"

        frontend -> graphQlServer "Requests" "GraphQL (HTTPS Post, Request/Response Body: JSON)"
        frontend -> openIdConnect "Signs-In" "HTTPS Get/Post (Request: Query Parameters, Response: JSON)"
        graphQlServer -> domainModel "Uses" "Plain-Old Instances (C#)"
        graphQlServer -> authorization "Uses" "Functions (C#)"
        domainModel -> objectRelationalMapper "Is Populated By" "Entity Framework Core (C# Instances)"
        authorization -> objectRelationalMapper "Accesses Database Through" "Entity Framework Core (Functional C#, C# Instances)"
        openIdConnect -> objectRelationalMapper "Accesses Database Through" "Entity Framework Core (Functional C#, C# Instances)"
        objectRelationalMapper -> database "Queries" "Structured Query Language (Query: SQL, Result: CSV)"
    }

    views {
        # systemLandscape "SystemLandscape" {
        #     include *
        #     autoLayout
        # }

        systemcontext metabase "SystemContext" {
            include *
            animation {
                user
                window
                metabase
                igsdb
                testlab
            }
            autoLayout
            description "The system context diagram for the Metabase."
            # properties {
            #     structurizr.groups false
            # }
        }

        container metabase "Containers" {
            include *
            animation {
                frontend
                # webApi
                middleTier
                database
            }
            autoLayout
            description "The container diagram for the Metabase."
        }

        component middleTier "Components" {
            include *
            animation {
            }
            autoLayout
            description "The component diagram for the API Application."
        }

        styles {
            element "Person" {
                color #08427b
                fontSize 22
                shape Person
            }
            element "API" {
                shape Pipe
            }
            element "Web Browser" {
                shape WebBrowser
            }
            element "Database" {
                shape Cylinder
            }
            element "Component" {
                background #85bbf0
                color #000000
            }
            element "Failover" {
                opacity 25
            }
        }
    }

}