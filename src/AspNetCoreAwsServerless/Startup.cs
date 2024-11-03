using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using AspNetCoreAwsServerless.Caches.Session;
using AspNetCoreAwsServerless.Config.Books;
using AspNetCoreAwsServerless.Config.Cognito;
using AspNetCoreAwsServerless.Config.Root;
using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Converters.Session;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Filters.FluentValidationFilter;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Repositories.Users;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Services.Jwt;
using AspNetCoreAwsServerless.Services.Session;
using AspNetCoreAwsServerless.Services.Sums;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Result;
using AspNetCoreAwsServerless.Validators.Example;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

namespace AspNetCoreAwsServerless;

public class Startup(IConfiguration configuration)
{
  public IConfiguration Configuration { get; } = configuration;

  // This method gets called by the runtime. Use this method to add services to the container
  public void ConfigureServices(IServiceCollection services)
  {
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
    services.AddSerilog();

    services
      .AddAuthentication(
        (o) =>
        {
          o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }
      )
      .AddCookie(options =>
      {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        options.Cookie.SameSite =
          Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? SameSiteMode.Strict
            : SameSiteMode.None;
        // Hacky nonsense, please fix
        // Problem is that the framework attempts to redirect in two cases:
        // 1. When the user is not authenticated and attempts to access a protected route
        // 2. When the controller returns a 403 Forbidden
        // In the first case, we want to return a 401 Unauthorized, but the StatusCode is 200 for some ASP.NET Core related reason
        // In the second case, we want to return the original StatusCode
        options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = c =>
        {
          c.Response.StatusCode =
            c.Response.StatusCode == StatusCodes.Status200OK
              ? StatusCodes.Status401Unauthorized
              : c.Response.StatusCode;
          return Task.FromResult(new ApiResultErrors(c.Response.StatusCode));
        };
      });

    services.AddAuthorization();

    services.AddHttpClient();

    // Create and register custom configuration that can be injected via IOptions<RootOptions>
    services.Configure<BooksOptions>(Configuration.GetSection("Books"));
    services.Configure<RootOptions>(Configuration.GetSection("Root"));
    services.Configure<CognitoOptions>(Configuration.GetSection("Cognito"));

    // Configure AWS services
    AWSOptions awsOptions = Configuration.GetAWSOptions();
    services.AddAWSService<IAmazonDynamoDB>(awsOptions);
    services.AddAWSService<IAmazonCognitoIdentityProvider>(awsOptions);

    DynamoDBContextConfig? dynamoDBContextConfig =
      Configuration.GetSection("DynamoDBContext").Get<DynamoDBContextConfig>() ?? new();
    services.AddSingleton(dynamoDBContextConfig);

    services.AddScoped<IDynamoDBContext, DynamoDBContext>();

    // Register application services
    services.AddScoped<ISumsService, SumsService>();

    services.AddScoped<IBooksService, BooksService>();
    services.AddScoped<IBooksRepository, BooksRepository>();
    services.AddScoped<IBooksConverter, BooksConverter>();

    services.AddScoped<IUsersService, UsersService>();
    services.AddScoped<IUsersRepository, UsersRepository>();
    services.AddScoped<IUsersConverter, UsersConverter>();

    services.AddScoped<ICognitoService, CognitoService>();

    services.AddScoped<ISessionService, SessionService>();
    services.AddScoped<ISessionConverter, SessionConverter>();
    services.AddSingleton<ISessionCache, SessionCache>();

    services.AddScoped<IJwtService, JwtService>();

    services.AddControllers(o =>
    {
      o.Filters.Add<FluentValidationFilter>();
    });

    string[] allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];

    Console.WriteLine($"AllowedOrigins: {string.Join(", ", allowedOrigins)}");

    services.AddCors(
      (options) =>
      {
        options.AddPolicy(
          name: "All",
          builder =>
          {
            builder
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins(allowedOrigins)
              .AllowCredentials();
          }
        );
      }
    );

    // Register Swagger to easily make manual calls to the API
    services.AddSwaggerGen();

    // Registers all validators automagically
    services.AddValidatorsFromAssemblyContaining<ExampleDtoValidator>();
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    // Use Serilog to log requests rather than AspNetCore's default logging
    app.UseSerilogRequestLogging();

    // Enable custom error handling to be handled by ErrorsController
    // Currently not implemented
    // app.UseExceptionHandler("/errors");

    app.UseCors("All");

    if (env.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    if (!env.IsDevelopment())
    {
      app.UseHttpsRedirection();
    }

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}

public partial class Program { }
