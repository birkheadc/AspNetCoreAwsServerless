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
        options.Events.OnRedirectToAccessDenied =
        options.Events.OnRedirectToLogin = c =>
        {
          c.Response.StatusCode = StatusCodes.Status401Unauthorized;
          return Task.FromResult(ApiResultErrors.Unauthorized);
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

    services.AddCors(
      (options) =>
      {
        options.AddPolicy(
          name: "All",
          builder =>
          {
            builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(allowedOrigins).AllowCredentials();
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
