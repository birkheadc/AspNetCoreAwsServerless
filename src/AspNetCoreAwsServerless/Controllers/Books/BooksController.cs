using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Utils.Id;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Books;

[ApiController]
[Route("books")]
public class BooksController(IBooksService service, IBooksConverter converter)
{
  private readonly IBooksService _service = service;
  private readonly IBooksConverter _converter = converter;

  [HttpGet]
  public async Task<IEnumerable<BookDto>> GetAll()
  {
    var books = await _service.GetAll();
    return books.Select(_converter.ToDto);
  }

  [HttpGet]
  [Route("/{id}")]
  public async Task<BookDto> Get([FromRoute] Id<Book> id)
  {
    Book book = await _service.Get(id);
    return _converter.ToDto(book);
  }

  [HttpPost]
  public async Task<BookDto> Create([FromBody] BookCreateDto createDto)
  {
    Book book = await _service.Create(createDto);
    return _converter.ToDto(book);
  }

  [HttpPut]
  public async Task<BookDto> Put([FromBody] BookPutDto putDto)
  {
    Book book = await _service.Put(putDto);
    return _converter.ToDto(book);
  }

  [HttpPatch]
  [Route("/{id}")]
  public async Task<BookDto> Patch([FromRoute] Id<Book> id, [FromBody] BookPatchDto patchDto)
  {
    Book book = await _service.Patch(id, patchDto);
    return _converter.ToDto(book);
  }

  [HttpDelete]
  [Route("/{id}")]
  public async Task Delete([FromRoute] Id<Book> id)
  {
    await _service.Delete(id);
  }
}
