using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesBackend.Exceptions
{
  public class EmailException : Exception
  {
    public EmailException(string msg)
      : base(msg)
    { }
  }
}
