using Application;
using Persistence;
using Shared;
using WebAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddApiVersioningExtension();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseErrorHandlingMiddleware();
app.MapControllers();

app.Run();