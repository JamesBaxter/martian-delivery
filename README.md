# martian-delivery

## Setup Instructions
The main application is in the MartianDelivery.Api proj.
Run these commands from [src/MartianDelivery.Api](https://github.com/JamesBaxter/martian-delivery/tree/main/src/MartianDelivery.Api)

1. ``dotnet restore``
2. ``dotnet build``
3. ``dotnet run``

The application should start list where it is listening. The default is: <br>
http://localhost:5172 <br>
With the swagger documentation at: <br>
http://localhost:5172/swagger/index.html

## Summary
- Built with Jetbrains Rider
- dotnet 8
- Used Rider WebApi template to get basic project

## Implementation Details

### Packages Used
- [Stateless](https://github.com/dotnet-state-machine/stateless) Used to create basic state machine workflow
- Testing Packages
    - Awesome Assertions - Fluent style assertions
    - AutoFixture - Create realistic test data for test setups, avoids "magic" strings
    - NSubstitute - Used to mock dependencies

## Functionality
- POST, GET, PATCH Parcels
- InMemory Parcel Store
- Parcel Audit Trail
- Barcode Validation - Done through regex

The implementation is split into 3 layers to keep separation of concerns
- API - Handles HTTP Requests, Request Validation
- Domain - Contains business logic
- Persistence - InMemory storage (wrapped Dictionary)

I've used a state machine to handle the bulk of the flow of the business logic. There are a lot of unit tests to cover state transitions. This state machine is injected into the Domain Object and should be reasonably easy to swap out if another approach was required. It would also be easy to add other States or Triggers without stepping on existing behaviour.

Validation of the barcode is handled through a Regex in a DataAnnotation. I would probably prefer to use FluentValidation and split the validation from the model further.

SystemTime is injected using an interface, this would make setting up scenarios involving time transition very easy. If there were requirements to simulate to progress of time in a test environment, a specific endpoint could be used to control system time. I have also had success with injecting a DateTimeProvider that makes HTTP Requests to a shared "time server" on integrated test environments.

## Out of Time
- Unhappy paths:
    - Parcel not found
    - Try to create Parcel with existing Barcode should return 409 Conflict
- Schedule for launches
- ETA Calculation
- Ability to "move time forward", I was also unclear if it was possible to change state if time had not moved forward , IE is it possible for a Parcel to Launch before its date
- Better exception handling, would probably have gone for ProblemDetailsFactory approach
- DateTimeOffset storage and Deserialization/Serialization. I would much prefer to use DateTimeOffset to represent all dates in the system but ran out of time. I prioritised the main business flow.
- DateTimeFormatting on response
- More helpful swagger examples that display enum options correctly
- Barcode Validation Unit tests
- Some warnings on build to do with required properties

## Potential Improvements for Enterprise Scale
- Row version/etag
    - It may be sensible to prevent a "last write win" scenario
    - We should be fairly protected from some issues by the state machine but if the same request was retried in very quick succession and different instances of the API handled each request there may be unintended behaviour
- Split Parcel Creation/Update into Azure Function
    - I have found that splitting the handling of http request and creation/updating of data works well. In this case perhaps the API could create a message and push it onto a ServiceBus which is consumed by an Azure Function. This allows the API Service to scale less and for the Azure Function to process messages at a rate we can control.
    - This has benefits when the the input is "bursty" which it may be in this case, rockets could be loaded and unloaded with 1000s/10000s of packages in a short time rather than over a long period.
    - Depending on requirements of the project the function part could scale to nothing in the intermediate time whilst the API can still handle ongoing GET requests
- Barcode Uniqueness
    - Depending on how the barcodes are generated there may be more safety in not using the barcode as the key for the Parcel
    - Perhaps the Web frontend is already handling authentication and authorisation and confirming the user has the right to get the details for that Barcode
    - Another endpoint could be created to get the Parcels for a Customer first, we would have to change the persistence layer to support this, making sure Parcels were keyed by something that works well for By ID and ByCustomer lookups
- Integration level tests that confirm serialization and dependencies are registered correctly

## AI Usage
Very minimal use of copilot (Office365 version)
- Used to learn how to mock ``out`` parameter of persistence layer
- Checked whether Transitioned event was called on entering Initial state for abandoned Audit History approach