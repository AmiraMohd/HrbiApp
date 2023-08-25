using DBContext;
using HrbiApp.API.Helpers;
using HrbiApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nancy.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// For Entity Framework
builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("ConnString")));

// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        try
        {
            var db = builder.Services.BuildServiceProvider().CreateScope().ServiceProvider.GetService<ApplicationDBContext>();
            ExceptionHandler excptionHandler = new ExceptionHandler(db);
            var serializedErrors = new JavaScriptSerializer().Serialize(actionContext.ModelState.Values);
            //var serializedModel= new JavaScriptSerializer().Serialize(actionContext.ActionDescriptor.);
            excptionHandler.LogException(serializedErrors, "BadRequest", actionContext.HttpContext.Request.Path.Value);
            //excptionHandler.LogException(serializedErrors, "BadRequest", actionContext.HttpContext.Request.Path.Value);
        }
        catch (Exception ex)
        {


        }

        var values = actionContext.ModelState.Values.ToList();
        string errorString = "";
        var errorList = new List<string>();
        foreach (var value in values)
        {
            if (value.Errors.Count > 0)
                errorList.Add(value.Errors.First().ErrorMessage);
        }
        errorString += string.Join(',', errorList);

        return new OkObjectResult(new BaseResponse
        {
            Status = false,
            Message = errorString
        });

        //return new BadRequestObjectResult(actionContext.ModelState);
    };
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
});
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
