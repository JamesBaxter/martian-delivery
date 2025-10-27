using MartianDelivery;
using MartianDelivery.Domain;
using MartianDelivery.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//todo consider lifecycles
builder.Services.AddSingleton<IParcelRepository, ParcelRepository>();
builder.Services.AddSingleton<IParcelFactory, ParcelFactory>();
builder.Services.AddSingleton<IParcelCreateCommandMapper, ParcelCreateCommandMapper>();
builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

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