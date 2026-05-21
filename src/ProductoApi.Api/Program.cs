using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductoApi.Api.GraphQL.Mutations;
using ProductoApi.Api.GraphQL.Queries;
using ProductoApi.Application;
using ProductoApi.Application.Features.Products;
using ProductoApi.Application.Features.Products.Requests;
using ProductoApi.Infrastructure.Persistance;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Configuración GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ProductQuery>()
    .AddMutationType<ProductMutation>()
    .AddFiltering()
    .AddSorting();

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Products API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}
// Endpoint GraphQL
app.MapGraphQL();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapControllers();

//Minamal APi
// GET ALL PRODUCTS
app.MapGet("/api/products", async ([FromServices] IProductService service) =>
{
    var products = await service.GetAllAsync();
    return Results.Ok(products);
});


// GET PRODUCT BY ID
app.MapGet("/api/products/{id:guid}", async (Guid id, [FromServices] IProductService service) =>
{
    var product = await service.GetByIdAsync(id);

    return product is not null
        ? Results.Ok(product)
        : Results.NotFound();
});


// CREATE PRODUCT
app.MapPost("/api/products", async (
    CreateProductRequest command,
    [FromServices] IProductService service) =>
{    

    var result = await service.CreateAsync(command);    
     if (result.Success)
    {
        return Results.Created($"/api/products/{result.Data}" ,result);
    }
    else
    {
        return Results.BadRequest(result);
    }
});


// UPDATE PRODUCT
app.MapPut("/api/products", async (    
    UpdateProductRequest command,
    [FromServices] IProductService service) =>
{   

    var result = await service.UpdateAsync(command);

    return result.Success
        ? Results.Ok(result)
        : Results.NotFound(result);
});


// DELETE PRODUCT
app.MapDelete("/api/products/{id:guid}", async (
    Guid id,
    [FromServices] IProductService service) =>
{
    var result = await service.DeleteAsync(id);

    return result.Success
        ? Results.Ok(result)
        : Results.NotFound(result);
});

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404)
    {
        response.ContentType = "application/problem+json";

        await response.WriteAsJsonAsync(new
        {
            success = false,
            errors = new[] { "Endpoint not found" },
        });
    }
});
app.Run();


