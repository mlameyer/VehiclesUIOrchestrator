using VehiclesUIOrchestrator.Managers;
using VehiclesUIOrchestrator.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Scoped objects are the same within a request, but different across different requests
builder.Services.AddScoped<IVehiclesManager, VehiclesManager>();
builder.Services.AddScoped<INavixCaseStudyRepository, NavixCaseStudyRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
