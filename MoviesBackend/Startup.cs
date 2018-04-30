﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using MoviesBackend.Models;
using MoviesBackend.Services;

namespace MoviesBackend
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //services.AddDbContext<MoviesContext>(opt => opt.UseInMemoryDatabase("Movies"));
      services.AddMvc();

      services.AddDbContext<MoviesContext>(options =>
        options.UseSqlite("Data Source=movies.sqlite",
            optionsBuilder => optionsBuilder.MigrationsAssembly("MoviesBackend")));
      services.AddIdentity<IdentityUser, IdentityRole>()
          .AddEntityFrameworkStores<MoviesContext>()
          .AddDefaultTokenProviders();
      services.Configure<IdentityOptions>(options =>
       {
         options.SignIn.RequireConfirmedEmail = true;
       });

      services.AddTransient<IMessageService, SendGridMessageService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseAuthentication();
      app.UseMvc(routes =>
      {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
