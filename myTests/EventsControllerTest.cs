using demoapp.Controllers;
using demoapp.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace myTests;

public class EventsControllerTest
{
    private readonly DataFixture databaseFixture;
    public EventsControllerTest()
    {
        this.databaseFixture = new DataFixture();
       
    }
    public void Dispose()
    {
        this.databaseFixture.Dispose();
    }

    [TestMethod]
    public void GetAllEvents()
    {
        using var context = this.databaseFixture.CreateContext();

        //var events = new EventController(context);
        //var res = await events.GetEvent();
        //Console.WriteLine(res);
        // all tables are empty by default
        //Assert.Equal(res.Result., 0);

        //// add a new item
        //var result = await authors.GetOrCreateAsync("Sample");
        //Assert.True(await context.Authors.AnyAsync());

        //var firstItem = await context.Authors.FirstOrDefaultAsync(a => a.Name == result.Name);
        //Assert.Equal(result.Name, firstItem?.Name);
    }
}
