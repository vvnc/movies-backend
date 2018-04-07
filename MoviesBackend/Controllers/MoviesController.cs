using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoviesBackend.Models;

namespace MoviesBackend.Controllers
{
  [Route("api/[controller]")]
  public class MoviesController : Controller
  {
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
      _context = context;

      if (_context.Movies.Count() == 0)
      {
        _context.Movies.Add(new Movie { Title = "Item1" });
        _context.SaveChanges();
      }
    }

    [HttpGet]
    public IEnumerable<Movie> GetAll()
    {
      return _context.Movies.ToList();
    }

    [HttpGet("{id}", Name = "GetMovies")]
    public IActionResult GetById(long id)
    {
      var item = _context.Movies.FirstOrDefault(t => t.Id == id);
      if (item == null)
      {
        return NotFound();
      }
      return new ObjectResult(item);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Movie item)
    {
      if (item == null)
      {
        return BadRequest();
      }

      _context.Movies.Add(item);
      _context.SaveChanges();

      return CreatedAtRoute("GetMovies", new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] Movie item)
    {
      if (item == null || item.Id != id)
      {
        return BadRequest();
      }

      var movie = _context.Movies.FirstOrDefault(t => t.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      movie.Title = item.Title;

      _context.Movies.Update(movie);
      _context.SaveChanges();
      return new NoContentResult();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
      var movie = _context.Movies.FirstOrDefault(t => t.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      _context.Movies.Remove(movie);
      _context.SaveChanges();
      return new NoContentResult();
    }
  }
}