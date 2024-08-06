using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAwsServerless.Config.Books;

public class BooksOptions
{
  public int PageSize { get; init; } = 10;
}
