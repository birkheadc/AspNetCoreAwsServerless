using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Utils.Id;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Books;

[ApiController]
[Route("books")]
public class BooksController(IBooksService service, IBooksConverter converter) : ControllerBase
{
  private readonly IBooksService _service = service;
  private readonly IBooksConverter _converter = converter;

  [HttpGet]
  public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
  {
    var books = await _service.GetAll();
    return Ok(books.Select(_converter.ToDto));
  }

  [HttpGet]
  [Route("/{id}")]
  public async Task<ActionResult<BookDto>> Get([FromRoute] Id<Book> id)
  {
    Book book = await _service.Get(id);
    return Ok(_converter.ToDto(book));
  }

  [HttpPost]
  public async Task<ActionResult<BookDto>> Create([FromBody] BookCreateDto createDto)
  {
    Book book = await _service.Create(createDto);
    return Ok(_converter.ToDto(book));
  }

  [HttpPut]
  public async Task<ActionResult<BookDto>> Put([FromBody] BookPutDto putDto)
  {
    Book book = await _service.Put(putDto);
    return Ok(_converter.ToDto(book));
  }

  [HttpPatch]
  [Route("/{id}")]
  public async Task<ActionResult<BookDto>> Patch(
    [FromRoute] Id<Book> id,
    [FromBody] BookPatchDto patchDto
  )
  {
    Book book = await _service.Patch(id, patchDto);
    return Ok(_converter.ToDto(book));
  }

  [HttpDelete]
  [Route("/{id}")]
  public async Task<ActionResult> Delete([FromRoute] Id<Book> id)
  {
    await _service.Delete(id);
    return Ok();
  }
}
