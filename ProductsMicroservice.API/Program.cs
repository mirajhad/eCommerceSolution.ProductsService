using BusinessLogicLayer;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using ProductsMicroservice.API.APIEndpoints;
using ProductsMicroservice.API.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


//ADD DAL AND BAL SERVICES
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddDataBusinessLogicLayer();

builder.Services.AddControllers();

//FLUENT VALIDATION
builder.Services.AddFluentValidationAutoValidation();

//json to enum
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

//Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapProductAPIEndpoints();

app.Run();
