using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAwsServerless.Utils.Paginated;

public class Paginated<T>
{
  public required List<T> Values { get; init; }
  public required string? PaginationToken { get; init; }
}
