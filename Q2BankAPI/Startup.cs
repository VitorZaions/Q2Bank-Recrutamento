using Q2Bank.Data;
using Q2Bank.Repositorios.Interfaces;
using Q2Bank.Repositorios;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Q2Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Q2BankAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {

        var connection = Configuration.GetConnectionString("DatabaseQ2");

        services.AddCors(setup =>
            setup.AddPolicy("AllowAll", builder =>
                 builder.WithOrigins("https://web.vitorzaions.top")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()         
                )
            );

        services.AddEntityFrameworkMySql().AddDbContext<DataContext>(
        options => options.UseMySql(connection, ServerVersion.AutoDetect(connection))
        );

        services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
        services.AddScoped<IFuncionarioRepositorio, FuncionarioRepositorio>();
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Token.Key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddControllers(); 

        services.AddSwaggerGen();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
               

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowAll");

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });


        app.UseSwagger()
        .UseSwaggerUI(setup =>
        {
            string swaggerjsonbasepath = string.IsNullOrWhiteSpace(setup.RoutePrefix) ? "n" : "..";
            setup.SwaggerEndpoint($"{swaggerjsonbasepath}/swagger/v1/swagger.sjon", "Version 1.0");
            setup.OAuthAppName("Lambda API");
            setup.OAuthScopeSeparator(" ");
            setup.OAuthUsePkce();
        });
    }
}