using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using BusinessLogicLayer.Services;
using BusinessObjects;

namespace Pham_Hong_Quan_SE18D05_Assignment_01
{
    public partial class App : Application
    {
        public static IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Load cấu hình từ appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Mở màn hình đăng nhập
            var loginWindow = new Views.LoginWindow();
            loginWindow.Show();

            base.OnStartup(e);
        }
    }
}
