using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesBackend.Exceptions
{
  public class JwtException : Exception
  {
    public JwtException(string msg)
      : base(msg)
    { }
  }
}
