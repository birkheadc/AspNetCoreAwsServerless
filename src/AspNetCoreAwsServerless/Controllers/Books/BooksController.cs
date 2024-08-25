using AspNetCoreAwsServerless.Attributes.IsEnvironment;
using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Utils.Paginated;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Books;

[ApiController]
[Route("books")]
public class BooksController(
  IBooksService service,
  IBooksConverter converter,
  ILogger<BooksController> logger
) : ControllerBase
{
  private readonly IBooksService _service = service;
  private readonly IBooksConverter _converter = converter;
  private readonly ILogger<BooksController> _logger = logger;

  [HttpGet]
  [Route("page")]
  public async Task<ActionResult<Paginated<BookDto>>> GetFirstPage()
  {
    _logger.LogInformation("GetFirstPage");
    ApiResult<Paginated<Book>> result = await _service.GetPage();
    return result.GetActionResult(_converter.ToDto);
  }

  [HttpGet]
  [Route("page/{paginationToken}")]
  public async Task<ActionResult<Paginated<BookDto>>> GetPage([FromRoute] string paginationToken)
  {
    _logger.LogInformation("GetPage");
    ApiResult<Paginated<Book>> result = await _service.GetPage(paginationToken);
    return result.GetActionResult(_converter.ToDto);
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<ActionResult<BookDto>> Get([FromRoute] Guid id)
  {
    _logger.LogInformation("Get");
    ApiResult<Book> result = await _service.Get(id);
    return result.GetActionResult(_converter.ToDto);
  }

  [HttpPost]
  public async Task<ActionResult<BookDto>> Create([FromBody] BookCreateDto createDto)
  {
    _logger.LogInformation("Create");
    ApiResult<Book> result = await _service.Create(createDto);
    return result.GetActionResult(_converter.ToDto);
  }

  [HttpPost]
  [Route("many")]
  public async Task<ActionResult> CreateMany([FromBody] BookCreateManyDto createManyDto)
  {
    _logger.LogInformation("CreateMany");
    ApiResult result = await _service.CreateMany(createManyDto);
    return result.GetActionResult();
  }

  [HttpPut]
  public async Task<ActionResult<BookDto>> Put([FromBody] BookPutDto putDto)
  {
    _logger.LogInformation("Put");
    ApiResult<Book> result = await _service.Put(putDto);
    return result.GetActionResult(_converter.ToDto);
  }

  [HttpPatch]
  [Route("{id}")]
  public async Task<ActionResult<BookDto>> Patch(
    [FromRoute] Guid id,
    [FromBody] BookPatchDto patchDto
  )
  {
    _logger.LogInformation("Patch");
    ApiResult<Book> result = await _service.Patch(id, patchDto);
    return result.GetActionResult(_converter.ToDto);
  }

  [HttpDelete]
  [Route("{id}")]
  public async Task<ActionResult> Delete([FromRoute] Guid id)
  {
    _logger.LogInformation("Delete");
    ApiResult result = await _service.Delete(id);
    return result.GetActionResult();
  }

  [HttpPost]
  [Route("seed/{num}")]
  [IsEnvironment(["Development", "Staging"])]
  public async Task<ActionResult> SeedMany([FromRoute] int num)
  {
    _logger.LogInformation("SeedMany");
    if (num > 999 || num < 1)
    {
      return ApiResult.Failure(ApiResultErrors.BadRequest).GetActionResult();
    }

    List<BookCreateDto> books = [];
    for (int i = 0; i < num; i++)
    {
      books.Add(
        new()
        {
          Title = $"Test Book {i.ToString().PadLeft(3, '0')}",
          Author = $"Test Author {i.ToString().PadLeft(3, '0')}",
          Pages = i * 10
        }
      );
    }

    return await CreateMany(new() { Books = books });
  }
}
