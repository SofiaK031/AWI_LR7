using Microsoft.OpenApi.Models;
using WebApplicationLR7.Auth;
using WebApplicationLR7.Services;

namespace WebApplicationLR7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // FlowerDisplayService, FlowerStorageService та CartService імітують роботу з БД тому важливо
            // зберігати стан від початку до кінця роботи застосунку незалежно від запитів (Singleton)
            builder.Services.AddSingleton<IFlowerStorageService, FlowerStorageService>();
            builder.Services.AddSingleton<IFlowerDisplayService, FlowerDisplayService>();
            builder.Services.AddSingleton<ICartService, CartService>();

            // UserService не зберігає стан, тому задля оптимізації роботи застосунку обирається Transient
            // який створює сервіс кожного разу коли його викликають
            builder.Services.AddTransient<IUserService, UserService>();

            // Використання JWT
            builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
            builder.Services.AddAuth(builder.Configuration);
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    new string[] { }
                }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline!
            // Реєстрація Swagger тільки для оточення Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}