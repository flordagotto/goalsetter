### The provided repo has a REST API written in C# using ASP.NET. 

*The system is the back-end for a car rental application, with the following functionalities: <br />
Add and remove vehicles. <br />
Add and remove clients. <br />
Create and cancel rentals.*

### Business rules:
*A removed vehicle should not be available for new rentals.<br />
Each vehicle must have a price per day that should be used to calculate the rental final charge.<br />
A rental has the following information: client, date range, vehicle, price.<br />
A vehicle cannot have overlapping rental dates.*

### Technical notes:
*The Entity Framework Seed mechanism populates the database with the initial vehicles, clients and rentals. <br />
The connection string must be configured to create the database schema and run the Seed.*

### Known bugs and issues:
	1. The GET Clients endpoint is always returning an empty array of rentals. The array should be populated.
	2. The system is allowing to create a rental that starts the same date another one ends for the same vehicle. This should not be allowed, the rentals should always be at least separated by one day.
	3. Another problem with overlapping rentals:
      The system has two rentals in place for the vehicle with id = 2 (created by the Seed method). If you execute the following request to create a new rental, you will find that it creates one that overlaps with the dates of the first rental:
      POST /Rentals
      {
        "clientId": 1,
        "vehicleId": 2,
        "startDate": "2024-03-25T18:21:00.727Z",
        "endDate": "2024-04-17T18:21:00.727Z"
      }
	4. Some business validations are returning HTTP 500, which is incorrect since this is not an internal server error but a business validation error on the request. Return the proper HTTP status code (for example the rentals dates validation or trying to delete a vehicle with an ID that is not found on the database).
	5. The system is not validating for any reasonable length on strings fields.
	6. A vehicle can be deleted even if it has pending rentals.
	7. A vehicle can be created with a PricePerDay as 0 (zero).

### Assignment:
Make a new private GitHub repository uploading this code without modifying it. Create a new branch and a single pull request to merge all your changes, without merging it. Give reading access to the reviewers.
In your changes, fix every bug/issue from the list, and also look around and apply any other bug fixes or improvements that you may consider necessary. Add any notes in the pull request explaining your changes whenever you want.
