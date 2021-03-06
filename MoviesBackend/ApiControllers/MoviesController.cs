﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoviesBackend.Models;
using MoviesBackend.Utils;

namespace MoviesBackend.ApiControllers
{
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = "Jwt", Roles = Roles.ADMIN)]
  public class MoviesController : Controller
  {
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
      _context = context;

      if (_context.Movies.Count() == 0)
      {
        _context.Movies.Add(new Movie
        {
          Title = "Test Movie",
          Overview = "This is the default movie that is created automatically when the database is first initialized",
          RuntimeMinutes = 90,
          ImdbId = 42,
          UpdatedTime = DateTime.Now,
          ReleaseDate = DateTime.Now.AddDays(1.0),
          UsDigitalReleaseDate = DateTime.Now.AddDays(10.0),
          UsPhysicalReleaseDate = DateTime.Now.AddDays(20.0)
        });
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
      var movie = _context.Movies.FirstOrDefault(t => t.Id == id);
      if (movie == null)
      {
        return NotFound();
      }
      return new ObjectResult(movie);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Movie movie)
    {
      if (movie == null)
      {
        return BadRequest();
      }

      _context.Movies.Add(movie);
      _context.SaveChanges();

      return CreatedAtRoute("GetMovies", new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] Movie newMovie)
    {
      if (newMovie == null || newMovie.Id != id)
      {
        return BadRequest();
      }

      var movie = _context.Movies.FirstOrDefault(t => t.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      movie.Title = newMovie.Title;
      movie.Overview = newMovie.Overview;
      movie.RuntimeMinutes = newMovie.RuntimeMinutes;
      movie.ImdbId = newMovie.ImdbId;
      movie.UpdatedTime = newMovie.UpdatedTime;
      movie.ReleaseDate = newMovie.ReleaseDate;
      movie.UsDigitalReleaseDate = newMovie.UsDigitalReleaseDate;
      movie.UsPhysicalReleaseDate = newMovie.UsPhysicalReleaseDate;

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