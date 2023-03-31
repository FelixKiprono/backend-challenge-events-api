using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace myTests;
public class DataFixture : IDisposable
{
    private readonly SqliteConnection connection;
    public DataFixture()
    {
        this.connection = new SqliteConnection("DataSource=:events.db:");
        this.connection.Open();
    }
    public void Dispose() => this.connection.Dispose();
    public EventDBContenxt CreateContext()
    {
        var result = new EventDBContenxt(new DbContextOptionsBuilder<EventDBContenxt>()
            .UseSqlite(this.connection)
            .Options);
        result.Database.EnsureCreated();
        return result;
    }
}