using AspNetCoreAwsServerless.Attributes.IsEnvironment;
using AspNetCoreAwsServerless.Authorization;
using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Entities.Permissions;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Paginated;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Books;

/// <summary>
/// Provides endpoints for managing books.
/// </summary>
[ApiController]
[Route("books")]
public class BooksController(
  IBooksService booksService,
  IBooksConverter booksConverter,
  ILogger<BooksController> logger
) : ControllerBase
{
  private readonly IBooksService _booksService = booksService;
  private readonly IBooksConverter _booksConverter = booksConverter;
  private readonly ILogger<BooksController> _logger = logger;

  [HttpGet]
  [Route("page")]
  public async Task<ActionResult<Paginated<BookDto>>> GetFirstPage()
  {
    _logger.LogInformation("GetFirstPage");
    ApiResult<Paginated<Book>> result = await _booksService.GetPage();
    return result.GetActionResult(_booksConverter.ToDto);
  }

  [HttpGet]
  [Route("page/{paginationToken}")]
  public async Task<ActionResult<Paginated<BookDto>>> GetPage([FromRoute] string paginationToken)
  {
    _logger.LogInformation("GetPage");
    ApiResult<Paginated<Book>> result = await _booksService.GetPage(paginationToken);
    return result.GetActionResult(_booksConverter.ToDto);
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<ActionResult<BookDto>> Get([FromRoute] Id<Book> id)
  {
    _logger.LogInformation("Get");
    ApiResult<Book> result = await _booksService.Get(id);
    return result.GetActionResult(_booksConverter.ToDto);
  }

  [HttpPost]
  [RequiresPermission([UserPermission.ModifyBooks])]
  public async Task<ActionResult<BookDto>> Create([FromBody] BookCreateDto createDto)
  {
    _logger.LogInformation("Create");
    ApiResult<Book> result = await _booksService.Create(createDto);
    return result.GetActionResult(_booksConverter.ToDto);
  }

  [HttpPost]
  [Route("many")]
  [RequiresPermission([UserPermission.ModifyBooks])]
  public async Task<ActionResult> CreateMany([FromBody] BookCreateManyDto createManyDto)
  {
    _logger.LogInformation("CreateMany");
    ApiResult result = await _booksService.CreateMany(createManyDto);
    return result.GetActionResult();
  }

  [HttpPut]
  [RequiresPermission([UserPermission.ModifyBooks])]
  public async Task<ActionResult<BookDto>> Put([FromBody] BookPutDto putDto)
  {
    _logger.LogInformation("Put");
    ApiResult<Book> result = await _booksService.Put(putDto);
    return result.GetActionResult(_booksConverter.ToDto);
  }

  [HttpDelete]
  [Route("{id}")]
  [RequiresPermission([UserPermission.ModifyBooks])]
  public async Task<ActionResult> Delete([FromRoute] Id<Book> id)
  {
    _logger.LogInformation("Delete");
    ApiResult result = await _booksService.Delete(id);
    return result.GetActionResult();
  }

  [HttpPost]
  [Route("seed/{num}")]
  [IsEnvironment(["Development", "Staging"])]
  [RequiresPermission([UserPermission.ModifyBooks])]
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
          Pages = i * 10,
        }
      );
    }

    return await CreateMany(new() { Books = books });
  }
}
