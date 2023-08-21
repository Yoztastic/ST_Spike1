## Application.Core

This project should have the complete set of domain logic. I.e All of the code that orchestrates and achieves the main
purpose of the solution.

Wherever it needs to call external services or databases this should be abstracted where the interface is declared in
this project but the code that actually calls these services is implemented in **Infrastructure**

It should be possible to test all of the acceptance criteria of the solution by testing just this project and Mocking
all the abstractions.

This project should never have to include any dependencies that references anything to do with Entity Framework or Http,
please make sure not to add these dependencies!
