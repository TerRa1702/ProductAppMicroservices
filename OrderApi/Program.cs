using OrderApi.Application.DependencyInjection;
using OrderApi.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddinfrastructureService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseInfrastructurePolicy();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapOrderEndpoints();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthorization();

app.Run();
