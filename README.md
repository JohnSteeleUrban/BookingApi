# BookingAPI

## Required Software/Setup

- .Net 5 SDK: https://dotnet.microsoft.com/download/visual-studio-sdks
- Visual Studio 2019: https://visualstudio.microsoft.com/downloads/
- PostgresSql:  https://www.postgresql.org/download/windows/
    -  Version 13.2 or higher
    - Be sure to install PgAdmin 4
    - For the database superuser (postgres) give any password.
    - Once installed, Create a test account.
        - Launch pgAdmin 4
        - Navigate to Servers => Login/Group Roles => right clieck and select create
            - Name: test
            - Password: test
            - All privilages enabled 
- Download and open the BookingService: https://github.com/JohnSteeleUrban/BookingApi
- Open the solution with Visual Studio 2019.
- Right click the solution in the Solution Explorer and click *Restore Nuget Packages*
- Run the *BookingService* Profile.


## Notes
The application will automatically create the database in your local Postgres db so long as the above instructions have been followed and the difault server port has not been changed from 5432.

The Browser will open on a Swaggar page so that you can test.  The enums can be strings or ints. The Search does not return canceled Appointments by default.

I added limited date validation, as I would expect the front end to only allow certain hours and this is only a small demo app.

The Serilog Sink for Postgress seems to have issues playing nice with EntityFramework or .Net 5.  Hence, the db logging is commented out and File logging to your C:\bookingservice\logs\ drive is enabled.

The idea behind the app was to sepatate any current and potential business logic or validation from the DAL.  All business logic is in the AppointmentService and all data is manipulated in the context.  I also went with a service because in my opinion adding the repository pattern is overkill is small CRUD apps.  Also, EF implements the repository and unit of work pattern under the hood already, or at least something similar.  If this were the start of a monolith or larger api, I'd implement a more DDD design with IAggregate/Root notation where appropriate as well as lock down entities and keep behavoir in each entity.

With that, enjoy!
