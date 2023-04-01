using demoapp.Controllers;
using demoapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace demoappTests;

public class ControllersTest
{

    public static EventDBContenxt  getUpDBContext()
    {
        var options = new DbContextOptionsBuilder<EventDBContenxt>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .EnableSensitiveDataLogging()
               .Options;
        var contenxt = new EventDBContenxt(options);
        return contenxt;

    }

    [Fact]
    public async void TestEventsMethods()
    {
        var context = getUpDBContext();
        var eventController = new EventController(context);

        var eventModel = new Event();
        eventModel.Id = 1;
        eventModel.Title = "event";
        eventModel.Description = "event description";
        eventModel.StartDate = DateTime.Today;
        eventModel.EndDate = DateTime.Today;
        eventModel.TimeZone = "UTC";

        //save event
        var result = await eventController.PostEvent(eventModel);
       
        //check
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result);



        //get the recently saved event
        var saveEvent = await eventController.GetEvent(1);
        Assert.IsType<OkObjectResult>(saveEvent.Result);

        ////test get all events
        var events = await eventController.GetEvents(1,1);
        Assert.IsType<OkObjectResult>(events.Result);


    }




    [Fact]
    public  async void TestInvitesMethods()
    {

        var context = getUpDBContext();
        var invitesController = new InvitationController(context);
        var eventController = new EventController(context);

        //mock data for event and user
        await eventController.PostEvent(new Event{
              Id = 1,
              Description="",
              Title="Test",
              StartDate=DateTime.Today,
              EndDate = DateTime.Today,
              TimeZone = "UTC"
        });

        int eventId = 1;
        List<int> usersId = new List<int>{ 1, 2, 3, 4 };

        //test invite with existing event 
        var invites = await invitesController.SendInvitation(eventId,usersId);
        Assert.IsType<OkObjectResult>(invites);

        //Test invite with a wrong event
        var badInvite = await invitesController.SendInvitation(2, usersId);
        Assert.IsType<NotFoundObjectResult>(badInvite);


        var userInvites = await invitesController.AcceptInvitation(1,1,1);
        Assert.IsType<OkObjectResult>(userInvites);




    }


}
