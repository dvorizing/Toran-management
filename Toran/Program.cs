using Microsoft.EntityFrameworkCore;
using Toran.BL;
using Toran.Dal;
using Toran.DAL;
using Toran.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BoiappContext>();

builder.Services.AddScoped<IToranRepository, ToranRepository>();
builder.Services.AddScoped<IToranStatusRepository, ToranStatusRepository>();

builder.Services.AddScoped<ToranDutyCalculator>();
builder.Services.AddScoped<SendMailToToran>(provider =>
{
    var calculator = provider.GetRequiredService<ToranDutyCalculator>();
    var config = provider.GetRequiredService<IConfiguration>();
    var sendGridApiKey = config["SendGrid:ApiMail"];
    return new SendMailToToran(calculator, sendGridApiKey);
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
