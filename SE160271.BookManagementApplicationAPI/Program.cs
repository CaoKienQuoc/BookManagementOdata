
using SE160271.BookManagementApplicationRepo.UnitOfWork;
using SE160271.BookManagementApplicationRepo.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOData();
// Đăng ký Swagger và cấu hình cho OData
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Management API", Version = "v1" });
    c.CustomOperationIds(apiDesc =>
    {
        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });

    // Đăng ký tệp EDM và các thực thể cho OData
    c.MapODataServiceRoute("odata", "odata", GetEdmModel());
});



// ??ng ký d?ch v? IUnitOfWork ? ?ây
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// ??ng ký DbContext trong h? th?ng d?ch v? c?a b?n
builder.Services.AddDbContext<BookManagementDBContext>();
var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Management API v1");
    });
}



app.UseHttpsRedirection();

app.UseAuthorization();
// Add OData Middleware
app.UseODataRouteDebug();

app.MapControllers();

app.Run();
static IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<Book>("Books");
    odataBuilder.EntitySet<Press>("Presses");
    odataBuilder.EntitySet<Address>("Addresses");
    return odataBuilder.GetEdmModel();
}

