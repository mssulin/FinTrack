using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Contexts
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dataFolder = Path.Combine(appData, "FinTrackExam");
            Directory.CreateDirectory(dataFolder);

            var dbPath = Path.Combine(dataFolder, "exam.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}