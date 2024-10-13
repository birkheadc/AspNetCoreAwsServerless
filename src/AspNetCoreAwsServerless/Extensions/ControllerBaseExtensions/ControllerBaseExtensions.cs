// using AspNetCoreAwsServerless.Entities.Users;
// using AspNetCoreAwsServerless.Utils.Id;
// using Microsoft.AspNetCore.Mvc;

// namespace AspNetCoreAwsServerless.Extensions.ControllerBaseExtensions;

// public static class ControllerBaseExtensions
// {
//   public static Id<User> GetUserId(this ControllerBase controller)
//   {
//     return new Id<User>(controller.User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException());
//   }
// }
