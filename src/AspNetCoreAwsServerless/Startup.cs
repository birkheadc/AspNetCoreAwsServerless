﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Services.Sums;

namespace AspNetCoreAwsServerless;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddAuthorization();
    services.AddAuthentication();

    services.AddAWSService<IAmazonDynamoDB>();
    services.AddScoped<IDynamoDBContext, DynamoDBContext>();

    services.AddScoped<ISumsService, SumsService>();

    services.AddScoped<IBooksService, BooksService>();
    services.AddScoped<IBooksRepository, BooksRepository>();
    services.AddScoped<IBooksConverter, BooksConverter>();

    services.AddControllers();

    services.AddSwaggerGen();
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
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
