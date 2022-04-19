# Architecture

This application is split into 5 projects:
- CelebrationBoard.Api: The interface for the web api. 
- CelebrationBoard.Application: Interface for services that want to handle the domain logic. This project works with any external dependencies and the domain logic to return a business result.
- CelebrationBoard.Domain: Logic that is specific to this application.
- CelebrationBoard.Infrastructure: Contains logic related to any external dependencies of this application such as an email provider that I plan on using to handle email subscriptions. (`CelebrationBoard.Application` does not depend on this.)
- CelebrationBoard.Persistence: Contains logic related to the DB that we manage. (`CelebrationBoard.Application` depends on this.)

Each project is responsible for a specific task and should depend only on the projects that it needs. This project architecture is modelled after [Clean Architecture](https://jasontaylor.dev/clean-architecture-getting-started/), with slight modifications.

Benefits:
- We can structure our application to have its domain logic insulated from changes in any other project by ensuring it has no external dependencies.
- We can prevent circular dependencies between projects from occurring as they are not allowed. Circular dependencies are undesirable as complect code by requiring the developer to store all dependencies in their head to understand the code they are looking at.
- We can easily change the Api project (e.g. by adding Razor pages) without affecting any other projects as no other projects depend on it.

Cons
- Work is required to introduce these projects and create references between them (the alternative is to work with one project and use folders inside to separate concerns.)

## CelebrationBoard.Api

Controllers

The controllers here are responsible solely for handling the endpoint url, defining the request/response objects to be mapped to/from their domain equivalent objects and also applying validation rules. 

When creating endpoints, instead of building CRUD endpoints, I have built task based endpoints. This makes the code easier to reason with as it reflects how the user would use the application and helps reduce complexity in the `Create` part of `CRUD`.

The reason I map request/response objects to their domain equivalent objects is to decouple the domain object from the interface of the application. This allows the domain object to be changed without affecting the interface of the application, which is beneficial in a public API as it lets you preserve backwards compatibility. In the case where your API is non-public and it is simultaneously deployed with API consumer(s) you control, this backwards compatibility may not be a requirement and you may be able to avoid creating these intermediary request/response objects. As an aside, C# records are a great way to represent these objects as they are immutable and concise.

Normally, it is ideal if the validation was performed in `CelebrationBoard.Application`, but I have moved the logic here to allow for appending the request field inside the error message. The most important thing however is that the validation is encapsulated in the domain project, which it is. This is in contrast to the [commonly done model (request) validation](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/getting-started), which would result in the splitting of domain knowledge between the domain project and the request, increasing the maintenance burden. That said, in this application, this was an OK way to go as it is quite simple.

### Repository Pattern

At the time of writing, I am not implementing the repository pattern as I don't see code that can be abstracted by the repository pattern as my code is relatively simple. However, as my DB queries grow in complexity, it may make sense to abstract the logic away from the `CelebrationBoard.Application` layer.

I have also not written any repository interfaces as I don't see myself changing database providers. Interfaces are also useful for testing, but I'll cover this in the testing section.

## Testing

### Unit tests
Unit tests are written against `CelebrationBoard.Domain`. Unit tests are great for testing complicated logic, which would mostly reside in this project, and because this project does not have any external dependencies, mocks are not required when unit testing, making these tests easier to write.

### Integration tests

Certain integration tests require using a real database to make confident assertions. For example, testing a `Create` operation followed by a `Get` operation in `CelebrationBoard.Api` simply cannot be achieved when mocking the database.

However, in `CelebrationBoard.Application`, it may make more sense to mock the database as it is possible to do so and doing so will improve test performance (spinning up a database is much slower than creating an in-memory mock). For now, I think it is OK to run these tests against a DB instance and suffer a slight performance hit.

#### CI

An issue I encountered when deciding to go down the 'use a DB instance for testing' route is that I would need to be able to do this while in CI as well. 

My considerations:
- Mock the database: my last resort given my reasoning above. If I can't get a DB instance working it CI then this is a valid reason to work with mocks instead of a DB instance.
- Point the test suite to a live DB hosted on the cloud: the main issue with this is that the db tests become non deterministic when there are multiple CI pipelines running, which is not an issue for me, but an issue for teams that decide to use this. It also costs money to host the DB on the cloud.
- Run the test suite in a docker container with a DB instance spun up locally to the container: This handles the nondeterminism issue specified above and was a good opportunity for me to learn Docker in more detail.
