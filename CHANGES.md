# All features, improviments and refactors

- I added some projects to left just presentation code in the presentation layer in `VacationRental.Api` project.  These projects have been created following `Domain Drive Design Pattern`. `Domain` project is used to share all interfaces and models that will be used by entire applications. `Infra.TransactionalDb` implements everything that is necessary to access the transactional database to handle rental´s data. The `CrossCutting` project is handling the just dependency injection concern but it can handle every shared concern along with the other projects.
- I added Entities to be handled by EntityFramework
- I removed all models from `VacationRental.Api` project but the binding models  
- Added EntityFramework as my ORM creating the context and migration in  `Infra.TransactionalDb`. Using the unit of work to handle database transactions. 
- I created repositories and services following DDD principles with Base Repository<TEntity> in order to avoid repeated code on repositories. 
- I set up the application to use SqlServer to run as development environment and InMemory to run as test.
- I created a profile to auto-mapping the entities to view models (Dtos) and back to make easier transformations among then. 
- I created a filter as middleware to handle exceptions on response messages creating a pattern and getting the right code status adjusting by either broke rule or just an exception.
- I changed every controller to use my services and repositories.
- I created `AvailabilityCheckService` following the `SOLID` principles.
- I refactored the tests creating a `SharedRequests` class to store all endpoint calls.
- I created a new 4 tests for the new endpoint put in `RentalsController` and make the optional feature on the challenge.
