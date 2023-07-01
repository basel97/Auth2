
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using RP_Identity_API.Data;
using RP_Identity_API.Services;
using Stripe;
using System.Text;

namespace RP_Identity_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ELearningDbContext>(o=>o.UseSqlServer(builder.Configuration.GetConnectionString("DefConnection")));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ELearningDbContext>();
            builder.Services.AddScoped<IRegister,RegisterServices>();
            //builder.Services.Configuration.GetSection("Stripe");
            
            
            // prevent domain conflicts and error access
            builder.Services.AddCors(o => o.AddPolicy("newPolicy", p =>
            {
                p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            //allow only Who have the token to access the api resources
            builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(b =>
            {
                b.RequireHttpsMetadata = false;
                b.SaveToken = true;
                b.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test..................................................secretkey")),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    //ClockSkew=TimeSpan.Zero (refresh token replacement)
                };
            });
            //retreiv key from jsonsettings
            //StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            //builder.Services.AddRouting(options => options.LowercaseUrls = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();
            //apply cors pipeline in middleware
            app.UseCors("newPolicy");
           // app.UseRouting();
            //is a must to secure ur api resources by token only send what is necessary and allow show resources if u have token
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}