using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Books;

[ApiController]
[Route("books")]
public class BooksController(IBooksService service, IBooksConverter converter) : ControllerBase
{
  private readonly IBooksService _service = service;
  private readonly IBooksConverter _converter = converter;

  [HttpGet]
  public async Task<ActionResult<List<BookDto>>> GetAll()
  {
    ApiResult<List<Book>> result = await _service.GetAll();
    return result.GetObjectResult(_converter.ToDto);
  }

  [HttpGet]
  [Route("/{id}")]
  public async Task<ActionResult<BookDto>> Get([FromRoute] Id<Book> id)
  {
    ApiResult<Book> result = await _service.Get(id);
    return result.GetObjectResult(_converter.ToDto);
  }

  [HttpPost]
  public async Task<ActionResult<BookDto>> Create([FromBody] BookCreateDto createDto)
  {
    ApiResult<Book> result = await _service.Create(createDto);
    return result.GetObjectResult(_converter.ToDto);
  }

  [HttpPut]
  public async Task<ActionResult<BookDto>> Put([FromBody] BookPutDto putDto)
  {
    ApiResult<Book> result = await _service.Put(putDto);
    return result.GetObjectResult(_converter.ToDto);
  }

  [HttpPatch]
  [Route("/{id}")]
  public async Task<ActionResult<BookDto>> Patch(
    [FromRoute] Id<Book> id,
    [FromBody] BookPatchDto patchDto
  )
  {
    ApiResult<Book> result = await _service.Patch(id, patchDto);
    return result.GetObjectResult(_converter.ToDto);
  }

  [HttpDelete]
  [Route("/{id}")]
  public async Task<ActionResult> Delete([FromRoute] Id<Book> id)
  {
    ApiResult result = await _service.Delete(id);
    return result.GetObjectResult();
  }
}
