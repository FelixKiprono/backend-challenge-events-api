Events API

Models

 1. Attending
    this is for the invitations it has userId and eventId which we use to fetch Event details
    for the user we are using the provided  (https://jsonplaceholder.typicode.com/users/{id}).
 2. Event
    This model has the events properties for Event CRUD
 3. User
    This is a boiler plate model just to use when perfoming request to (https://jsonplaceholder.typicode.com/users/{id})
    in order to access the response more easily and also assist on attaching the data when you query single event
    `api\Event\{1}`
    the request are made by RestSharp https://restsharp.dev/intro.html#introduction

Routes 
   * [GET] /api/Event/All/{page} {resultsPerPage}
        - query all events but as per the defined pagination
            example payload
            {
              "events": [
                {
                  "id": 1,
                  "title": "string",
                  "startDate": "2023-03-31T14:10:58.583",
                  "endDate": "2023-03-31T14:10:58.583",
                  "timeZone": "string",
                  "description": "string",
                  "user": null
                },
                {
                  "id": 2,
                  "title": "C# event",
                  "startDate": "2023-03-31T21:58:51.251",
                  "endDate": "2023-03-31T21:58:51.251",
                  "timeZone": "UTC",
                  "description": "Meetup for senior developers",
                  "user": null
                }
              ],
              "currentPage": 1,
              "totalPages": 4,
              "message": "Success",
              "error": ""
            }

   * [GET] /api/Event/{2}
      - Fetch specific events based on ID and attaches users data on the response.
        {
            "data": {
            "id": 2,
            "title": "C# event",
            "startDate": "2023-03-31T21:58:51.251",
            "endDate": "2023-03-31T21:58:51.251",
            "timeZone": "UTC",
            "description": "Meetup for senior developers",
            "user": {
                "id": 1,
                "name": "Leanne Graham",
                "userName": "Bret",
                "email": "Sincere@april.biz",
                "phone": "1-770-736-8031 x56442",
                "website": "hildegard.org",
                "address": {
                "id": 0,
                "street": "Kulas Light",
                "suite": "Apt. 556",
                "city": "Gwenborough",
                "zipCode": "92998-3874",
                "geo": {
                    "id": 0,
                    "lat": "-37.3159",
                    "ltd": null
                }
                },
                "company": {
                "id": 0,
                "name": "Romaguera-Crona",
                "catchPhrase": "Multi-layered client-server neural-net",
                "bs": "harness real-time e-markets"
                }
            }
            },
            "message": "Success",
            "error": ""
        }

   * [PUT] /api/Event/{id}
      - Update event

   * [DELETE] /api/Event/{id}
     - Delete event

   * [POST] /api/Event
     - Create event
         payload it expects
         {
          "title": "Russian Federation",
          "startDate": "2023-04-01T01:18:38.181Z",
          "endDate": "2023-04-01T01:18:38.181Z",
          "timeZone": "(GMT+3)",
          "description": "Talks about the autonomous republics"
         }
   * [GET] /api/Invitation/User/{id}
     - Get users Invitations if any

   * [POST] /api/Invitation/Send/{eventId} {userIds}
     - Invite users to an event this expects eventId and list of users Ids

   * [POST] /api/Invitation/Accept/{invitationId} {userId} {eventId}
      - serves as confirmation to the event from the user, the user needs to supply the arguments above and the state of the event will
      change to Confirmed/Attending/Participating
       
       

Tests
 The tests are created using XUnit
 https://xunit.net/#documentation

 Other instructions
  - The sqlite database lives in the root of the demoapp folder

Docker
  - You can read through this https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-6.0

how to build docker image
 - run this command

 1. docker build -t . eventsapi/v1 
 2. docker run -p {PORT}:80 --name eventsapi eventsapi
 3. Open browser and put http://localhost:{port}/api/event/1 to check but you can always use POSTMAN/Insomnia or PostWoman to make requests or
     any that works for you.












