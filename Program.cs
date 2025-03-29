using AtomServer.database;
using AtomServer.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Config
builder.Configuration.AddIniFile("config.ini");
var config = new Config();
builder.Configuration.Bind(config);
#endregion

builder.Services.AddControllers();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
Console.WriteLine($"Host={config.dbHost};Port={config.dbPort};Database={config.dbName};Username={config.dbUser};Password={config.dbPassword}");
#region DataBase
builder.Services.AddDbContext<DataContext>(x => x.UseNpgsql($"Host={config.dbHost};Port={config.dbPort};Database={config.dbName};Username={config.dbUser};Password={config.dbPassword}"));
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowAnyOrigin();
});

app.Run();

public class Config
{
    public string dbHost { get; set; }
    public string dbPort {  get; set; }
    public string dbName { get; set; }
    public string dbUser { get; set; }
    public string dbPassword { get; set; }
    public string clientHost {  get; set; }
    public string clientPort {  get; set; }
}