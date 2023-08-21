## Infrastructure

This project is to implement services that physically depend on Upstream service, SQS, Databases etc.

This project can have dependencies on *HttpClient* and *EF* and *AWS* services in order to realise the behaviour required from the interfaces declared in **Application.Core**

This project depends on **Application.Core**. The **Host** project depends on this only to achieve mapping of abstractions to concrete classes and we may even lose this direct dependency if we perform such mappings using assembly level mappings.

This project should not know anything about the HttpContext or the context in which the **Application.Core** abstractions were called.
