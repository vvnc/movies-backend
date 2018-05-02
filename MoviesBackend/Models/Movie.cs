using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesBackend.Models
{
  public class Movie
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Overview { get; set; }
    public int RuntimeMinutes { get; set; }
    public long ImdbId { get; set; }
    public DateTime UpdatedTime { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime UsDigitalReleaseDate { get; set; }
    public DateTime UsPhysicalReleaseDate { get; set; }
  }
}
