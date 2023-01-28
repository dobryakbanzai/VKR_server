using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;
using VKR_server.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);



string MyAllowSpecificOrigins = "MyPolicy";


builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));

var secretKey = builder.Configuration.GetSection("JWTSettings:SecretKey").Value;
var issuer = builder.Configuration.GetSection("JWTSettings:Issuer").Value;
var audience = builder.Configuration.GetSection("JWTSettings:Audience").Value;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

// Add services to the container.


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer= true,
        ValidIssuer = issuer,

        ValidateAudience= true,
        ValidAudience= audience,

        ValidateLifetime= true,
        IssuerSigningKey = signingKey,

        ValidateIssuerSigningKey= true,
        
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddControllers();
builder.Services.AddControllersWithViews().
    AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
    builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<PostgresContext>(opt =>
    opt.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=200018"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
   // app.UseSwagger();
   // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.MapControllers();

string Host = System.Net.Dns.GetHostName();
string IP = System.Net.Dns.GetHostByName(Host).AddressList[3].ToString();

app.Run("http://" + IP + ":8080/");
