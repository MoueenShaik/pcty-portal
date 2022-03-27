using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using PCTYLibrary;
using PCTYLibrary.Authentication;
using PCTYLibrary.Constants;
using PCTYLibrary.Services;
using System.Reflection;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(
    options => options.DefaultScheme = AuthSchemeConstants.CustomAuthScheme)
    .AddScheme<CustomAuthSchemeOptions, CustomAuthHandler>(
        AuthSchemeConstants.CustomAuthScheme, options => { });

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(MyAllowSpecificOrigins,
//                          builder =>
//                          {
//                              builder.WithOrigins("https://localhost:7046/",
//                                "http://localhost:3000/")
//                                                  .AllowAnyHeader()
//                                                  .AllowAnyMethod();
//                          });
//});
builder.Services.AddMvc();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();


builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
var configuration = builder.Configuration;

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition(AuthSchemeConstants.CustomAuthScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        ////  BearerFormat = "JWT",
        Scheme = AuthSchemeConstants.CustomAuthScheme
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=AuthSchemeConstants.CustomAuthScheme
                }
            },
            new string[]{}
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
