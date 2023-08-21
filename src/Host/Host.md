## Host

This project is effectively the glue that binds everything together. Nothing should depend on this project and it is free to depend on all other services in the solution (although technically it does not need to actually reference **Infrastructure** and if **Host.Application** is broken out then all concrete registrations could be done without project dependencies on anything but **Host.Application**)

This is the only place that should be aware of the actual protocol i.e. Http or SQS that is used to invoke the **Core** logic.

In here is where the container is configured, and the configuration is set up. It also defines all the Controllers|Routes|Event Handlers and sets up the middleware pipeline.

We have split the project down with the **Application** folder this may move to its own project in the future but for now it is responsible for declaring and validating the DTO's and mapping DTO's to domain types. ~~~~
