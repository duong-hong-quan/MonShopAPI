using System.Text;
using CorePush.Apple;
using CorePush.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.Mapping;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.Realtime;
using Monshop.BackEnd.Service.Contracts;
using Monshop.BackEnd.Service.Implementations;
using Monshop.BackEnd.Service.Payment.PaymentService;
using Monshop.BackEnd.Service.Services;
using Monshop.BackEnd.Service.Services.Firebase;
using NetCore.QK.BackEndCore.Application.IRepositories;
using NetCore.QK.BackEndCore.Application.IUnitOfWork;
using NetCore.QK.BackEndCore.Domain;
using NetCore.QK.BackEndCore.Infrastructure.Repositories;
using NetCore.QK.BackEndCore.Infrastructure.UnitOfWork;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(p => p.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    // builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    builder.WithOrigins("http://localhost:3001").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    builder.WithOrigins("https://mon-shop-fe.vercel.app").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

builder.Services.AddDbContext<MonShopContext>(option =>
{
    option.UseSqlServer(builder.Configuration["ConnectionStrings:DB"]);
});
var mapper = MappingConfig.RegisterMap().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDbContext, MonShopContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFirebaseService, FirebaseService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();

builder.Services.AddTransient<ICloudMessagingService, CloudMessagingService>();
builder.Services.AddHttpClient<FcmSender>();
builder.Services.AddHttpClient<ApnSender>();

// Configure strongly typed settings objects
var appSettingsSection = builder.Configuration.GetSection("FcmNotification");
builder.Services.Configure<FcmNotificationSetting>(appSettingsSection);


builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bear Authorization string as following: `Bearer Generate-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] { }
        }
    });
});
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MonShopContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});


var app = builder.Build();

app.UseSwagger(options => { options.SerializeAsV2 = true; });
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/chat");

app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<MonShopContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0) _db.Database.Migrate();
    }
}