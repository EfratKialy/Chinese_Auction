using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using project.BLL;
using project.DAL;
using Project.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

var tkConf = builder.Configuration.GetSection("Jwt");
var TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = tkConf["Issuer"],
    ValidateAudience = true,
    ValidAudience = tkConf["Audience"],
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tkConf["Key"])),
    ValidateLifetime = true,
    //ClockSkew = TimeSpan.FromSeconds(30),
    //RequireExpirationTime = true,
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = TokenValidationParameters;
    });

builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(c => c.UseSqlServer("Data Source=localhost;Initial Catalog=EfratChinaS;Integrated Security=True;Trust Server Certificate=True;"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddScoped<IDonorDAL, DonorDAL>();
builder.Services.AddScoped<IDonorService, DonorService>();

builder.Services.AddScoped<IGiftDAL, GiftDAL>();
builder.Services.AddScoped<IGiftService, GiftService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();

builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IPurchaseDAL, PurchaseDAL>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerDAL, CustomerDAL>();

//builder.Services.AddScoped<IRaffleService, RaffleService>();
//builder.Services.AddScoped<IRaffleDAL, raffleDAL>();

builder.Services.AddScoped<EmailValidator, EmailValidator>();



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});


var app = builder.Build();
app.UseRouting();

// Configure the HTTP request pipeline.
app.UseCors(options => options
    .WithOrigins("http://localhost:3000") // Replace with your frontend origin
    .AllowAnyMethod() // Adjust based on your API requirements
    .AllowAnyHeader()); // Adjust based on your API requirements

app.UseStaticFiles(); // Serve static files from wwwroot


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.MapControllers();

app.Run();
