using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NytteveksterApi.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
  {
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
  });

builder.Services.AddDbContext<NytteveksterContext>(options => options.UseSqlite("Data Source=nyttevekster.db"));

builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowAll",
      builder => builder
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowAnyOrigin()
    );
  });

builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
