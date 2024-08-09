using ProductsApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ProductsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", (ProductsService service) =>
{
    return service.Get();
})
.WithName("get_all_products")
.WithSummary("Get all products")
.WithDescription("Returns a list of products in the system")
.WithOpenApi();

app.MapPost("/order", (ProductsService service, string body) =>
{
    return service.Order(body);
})
.WithName("order_product")
.WithSummary("Order product with name")
.WithDescription("Order a new product with product name")
.WithOpenApi();

app.MapGet("/orders", (ProductsService service) =>
{
    return service.GetOrderedProducts();
})
.WithName("get_ordered_products")
.WithSummary("Get ordered products")
.WithDescription("Returns a list of ordered products")
.WithOpenApi();

app.Run();


