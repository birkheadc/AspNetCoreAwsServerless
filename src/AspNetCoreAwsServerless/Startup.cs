using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using AspNetCoreAwsServerless.Config.Root;
using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Services.Sums;
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

    services.AddAuthorization();
    services.AddAuthentication();

    // Create and register custom configuration that can be injected via IOptions<RootOptions>
    services.Configure<RootOptions>(Configuration.GetSection("Root"));

    // Configure AWS services
    AWSOptions awsOptions = Configuration.GetAWSOptions();
    services.AddAWSService<IAmazonDynamoDB>(awsOptions);

    DynamoDBContextConfig? dynamoDBContextConfig =
      Configuration.GetSection("DynamoDBContext").Get<DynamoDBContextConfig>() ?? new();
    services.AddSingleton(dynamoDBContextConfig);

    services.AddScoped<IDynamoDBContext, DynamoDBContext>();

    // Register application services
    services.AddScoped<ISumsService, SumsService>();

    services.AddScoped<IBooksService, BooksService>();
    services.AddScoped<IBooksRepository, BooksRepository>();
    services.AddScoped<IBooksConverter, BooksConverter>();

    services.AddControllers();

    // Register Swagger to easily make manual calls to the API
    services.AddSwaggerGen();
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    // Use Serilog to log requests rather than AspNetCore's default logging
    app.UseSerilogRequestLogging();

    app.UseExceptionHandler("/errors");

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

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}

public partial class Program { }
