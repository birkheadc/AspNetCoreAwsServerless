using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Tests.TestData.Books;

public static class BooksTestData
{
  public static Book GenerateBook()
  {
    Id<Book> id = new();
    Book book =
      new()
      {
        Id = id,
        Title = id.ToString()[..10],
        Author = id.ToString()[10..],
        Pages = new Random().Next(1, 500)
      };
    return book;
  }

  public static List<Book> GenerateBooks(int n)
  {
    List<Book> books = [];
    for (int i = 0; i < n; i++)
    {
      books.Add(GenerateBook());
    }
    return books;
  }
}
