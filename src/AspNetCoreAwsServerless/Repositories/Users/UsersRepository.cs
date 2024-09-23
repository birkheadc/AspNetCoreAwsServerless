using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Repositories.Users;

public class UsersRepository(IDynamoDBContext context, ILogger<UsersRepository> logger)
  : IUsersRepository
{
  private readonly IDynamoDBContext _context = context;
  private readonly ILogger<UsersRepository> _logger = logger;

  public async Task<ApiResult> Delete(Id<User> id)
  {
    _logger.LogInformation("Delete User Id: {id} (User may or may not have been present!)", id);
    try
    {
      await _context.DeleteAsync<User>(id);
      _logger.LogInformation("Deleted user Id: {id} (User may or may not have been present!)", id);
      return ApiResult.Success();
    }
    catch (Exception exception)
    {
      _logger.LogWarning("Failed to delete user Id: {id}. {exception}", id, exception);
      return ApiResultErrors.InternalServerError;
    }
    throw new NotImplementedException();
  }

  public async Task<ApiResult<User>> Get(Id<User> id)
  {
    _logger.LogInformation("Get User Id: {id} ", id);
    try
    {
      User? user = await _context.LoadAsync<User>(id);
      if (user is null)
      {
        _logger.LogWarning("Failed to get user Id: {id}. Id not present in database.", id);
        return ApiResultErrors.NotFound;
      }
      _logger.LogInformation("Found user Id: {id}", id);
      return user;
    }
    catch (Exception exception)
    {
      _logger.LogWarning("Failed to get user Id: {id}. {exception}", id, exception);
      return ApiResultErrors.InternalServerError;
    }
  }

  public async Task<ApiResult<IEnumerable<User>>> GetAll()
  {
    _logger.LogInformation("Get all users...");
    try
    {
      List<ScanCondition> conditions = [];

      List<User> users = await _context.ScanAsync<User>(conditions).GetRemainingAsync();
      _logger.LogInformation("Found all users. Count: {count}", users.Count);

      return users;
    }
    catch (Exception exception)
    {
      _logger.LogCritical("Failed to get all users. {exception}", exception);
      return ApiResultErrors.InternalServerError;
    }
  }

  public async Task<ApiResult<User>> Put(User user)
  {
    _logger.LogInformation("Putting user {id}...", user.Id);
    try
    {
      await _context.SaveAsync(user);
      _logger.LogInformation("Successfully put user {id}", user.Id);
      return ApiResult.Success(user);
    }
    catch (Exception exception)
    {
      _logger.LogCritical("Failed to put user {user}. {exception}", user, exception);
      return ApiResultErrors.InternalServerError;
    }
  }
}
