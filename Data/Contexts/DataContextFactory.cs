
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer("Data Source=silicon-sqlserver-hamhol.database.windows.net;Initial Catalog=silicon-database;Persist Security Info=True;User ID=sqladmin;Password=vetinte123!;Encrypt=True;Trust Server Certificate=True");

        return new DataContext(optionsBuilder.Options);
    }
}