using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoviesBackend.Models
{
  public class MoviesContext : DbContext
  {
    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
  }
}
