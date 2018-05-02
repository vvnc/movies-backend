﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

using MoviesBackend.Utils;

namespace MoviesBackend
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var host = BuildWebHost(args);

      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        try
        {
          var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
          var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
          await RoleInitializer.InitializeAsync(userManager, rolesManager);
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred while seeding the database with roles.");
        }
      }

      host.Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
  }
}
