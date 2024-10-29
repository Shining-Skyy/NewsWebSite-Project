using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace UnitTest.Builders
{
    public class DatabaseBuilder
    {
        // This method creates and returns a new instance of DataBaseContext.
        internal DataBaseContext GetDbContext()
        {
            // Create a new options builder for the DataBaseContext.
            var options = new DbContextOptionsBuilder<DataBaseContext>()
              .UseInMemoryDatabase(Guid.NewGuid()
              .ToString())
              .Options;

            // Return a new instance of DataBaseContext with the specified options.
            return new DataBaseContext(options);
        }
    }
}
