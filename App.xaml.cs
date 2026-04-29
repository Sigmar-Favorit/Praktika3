using Praktika3.Data;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Praktika3
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            db.Seed();
            db.AutoCancelExpiredRequests();
        }
    }
}