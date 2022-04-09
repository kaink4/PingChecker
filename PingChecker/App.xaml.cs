using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingChecker.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PingChecker.Infrastructure;
using PingChecker.ViewModels;

namespace PingChecker;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();




        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = ConfigurationFactory.GetConfiguration(); 

        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));


        services.Scan(s => s.FromCallingAssembly()
            .AddClasses(c => c.AssignableToAny(typeof(Window), typeof(UserControl), typeof(ViewModelBase)))
            .AsSelf()
            .WithTransientLifetime());

        services.AddTransient<IWindowFactory, WindowFactory>();
        services.AddTransient<ISampleService, SampleService>();
    }
}
