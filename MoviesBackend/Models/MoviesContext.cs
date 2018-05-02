using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MoviesBackend.Models
{
  public class MoviesContext : IdentityDbContext
  {
    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
  }
}
