using Application.UserAccount.Handlers;
using CloudinaryDotNet;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Authentication;
using Infrastructure.Mail;
using Infrastructure.Payment;
using Infrastructure.ServiceHelp;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Net.payOS;
using Persistence.Data;
using Persistence.IRepository;
using Persistence.Middlewares;
using Persistence.Repository;
using Persistence.UnitOfWork;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Registration DBContext
builder.Services.AddDbContext<FlawlessDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Transient);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(RegistrationHandler).Assembly));

// Registratio Unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registration Repository  
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IAppointmentDetailRepository, AppointmentDetailRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IArtistAvailabilityRepository, ArtistAvailabilityRepository>();
builder.Services.AddScoped<IArtistProgressRepository, ArtistProgressRepository>();
builder.Services.AddScoped<IBankInfoRepository, BankInfoRepository>();
builder.Services.AddScoped<ICommissionRepository, CommissionRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IHistoryRefundRepository, HistoryRefundRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IServiceCategoriesRepository, ServiceCategoriesRepository>();
builder.Services.AddScoped<IServiceOptionRepository, ServiceOptionRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserAppRepository, UserAppRepository>();
builder.Services.AddScoped<IUserProgressRepository, UserProgressRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IInformaitonArtistRepository, InformationArtistRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<IChatBoxAiRepository, ChatBoxAiRepository>();


// Đăng ký BlobStorageService
builder.Services.AddSingleton<BlobStorage>();

// Bind Cloudinary settings from appsettings.json
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Register PhotoService
builder.Services.AddScoped<IClouddinaryStorage, ClouddinaryStorage>();

// Đăng ký Email Sender
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

// PayOS 
builder.Services.AddScoped<IPayOsService, PayOsService>();


builder.Services.AddSingleton<PayOS>(sp =>
{
    var config = builder.Configuration;
    var clientId = config["PayOS:ClientId"];
    var apiKey = config["PayOS:ApiKey"];
    var checksumKey = config["PayOS:ChecksumKey"];

    return new PayOS(clientId, apiKey, checksumKey);
});

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();


builder.Services.AddIdentity<UserApp, IdentityRole>()
    .AddEntityFrameworkStores<FlawlessDBContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<UserManager<UserApp>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Tokens.EmailConfirmationTokenProvider = "Email";
});

// Notification Hub
builder.Services.AddSignalR();

// Chat Box AI
builder.Services.AddSingleton<ChatBoxAI>();
builder.Services.AddSingleton<ParseUserQuestionService>();

// Authentication Google
builder.Services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    googleOptions.CallbackPath = "/signin-google";  // thay bằng domain khi deploy
                }); 

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{

    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JWT_Local:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT_Local:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JWT_Local:Secret"]))
    };
    option.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            // Nếu request là cho SignalR
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/notificationHub")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
}
);

// GenerateJwtToken
builder.Services.AddSingleton(sp =>
{
    var config = builder.Configuration;
    return new GenerateJwtToken(
        jwtSecret: config["JWT_Local:Secret"],
        issuer: config["JWT_Local:ValidIssuer"],
        audience: config["JWT_Local:ValidAudience"],
        tokenValidityMins: int.Parse(config["JWT_Local:TokenValidityMins"])
    );
});



// Register IHttpContextAccessor and UserService to the DI container
builder.Services.AddHttpContextAccessor();  // Add IHttpContextAccessor



// Registration Controllers and Swagger

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "SWP API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(origin => true)
              .AllowCredentials(); 
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserApp>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var contenxt = scope.ServiceProvider.GetRequiredService<FlawlessDBContext>();
    await DBInitializer.Initialize(userManager, roleManager, contenxt);
}


// Configuration CORS - Allow many difference domain
//app.UseCors(policy => policy.AllowAnyHeader()
//                            .AllowAnyMethod()
//                            .SetIsOriginAllowed(origin => true)
//                            .AllowCredentials());


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ChatHub>("/chatHub");

app.Run();

