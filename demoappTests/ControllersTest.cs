using demoapp.Controllers;
using demoapp.Models;
using Microsoft.EntityFrameworkCore;


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
        //get the recently saved event
        var saveEvent = await eventController.GetEvent(1);
        //check
        Assert.NotNull(result);
        Assert.NotNull(result.Result);

        Assert.Equal(1, saveEvent.Value.Id);
        Assert.Equal("event", saveEvent.Value.Title);
        Assert.Equal(DateTime.Today, saveEvent.Value.StartDate);
        Assert.Equal(DateTime.Today, saveEvent.Value.EndDate);
        Assert.Equal("UTC", saveEvent.Value.TimeZone);

        //test get all events
        var events = await eventController.GetEvent();
        Assert.NotNull(events);
        Assert.Equal(1, events.Value.Count());

        //deleted Event
        var deleteResult = await eventController.DeleteEvent(1);

        var fetchedEvent = await eventController.GetEvent(1);

        Assert.NotNull(deleteResult);
        Assert.Null(fetchedEvent.Value);
        Assert.Null(fetchedEvent.Value);

    }

  


    [Fact]
    public  async void TestInvitesMethods()
    {

   


    }


}
