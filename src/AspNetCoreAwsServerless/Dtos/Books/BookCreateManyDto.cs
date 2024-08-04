using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAwsServerless.Dtos.Books;

public class BookCreateManyDto
{
  public required List<BookCreateDto> Books { get; init; }
}
