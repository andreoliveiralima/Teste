using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Text;
using Teste_CrossCutting;
using Teste_Domain.Abstractions;
using Teste_Domain.Entities;
using Teste_Domain.Interfaces;
using Teste_Infra.Data.Context;
using Teste_Infra.Data.Repository;
using Teste_Service.Configuration;
using Teste_Service.Services;

namespace Teste_Application
{
    public class Startup
    {
        private const string _applicationName = "Teste";
        private const int _tentativas = 5;
        private TimeSpan retryWait = TimeSpan.FromSeconds(2);
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.Configure<ApiConfig>(_configuration.GetSection(nameof(ApiConfig)));
            services.Configure<AcessoDadosConfig>(_configuration.GetSection(nameof(AcessoDadosConfig)));

            services.AddSingleton<IApiConfig>(x => x.GetRequiredService<IOptions<ApiConfig>>().Value);
            services.AddSingleton<IAcessoDadosConfig>(x => x.GetRequiredService<IOptions<AcessoDadosConfig>>().Value);

            services.AddScoped<IPapelNegociado, PapelNegociadoRepository>();
            services.AddScoped<IEmpresa, Empresa>();
            services.AddScoped<IEmpresaRefit, EmpresaRefit>();
            services.AddScoped<IServiceToken, ServiceToken>();
            services.AddScoped<IAcessoDados, AcessoDados>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Teste",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme.
                   \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.
                    \r\n\r\nExample: \Bearer 12345abcdef\"
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
                         new string[] {}
                    }
                });
            });

            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            var tentativa = 1;
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(_tentativas, retryAttemp => retryWait);

            services.AddHttpClient<IEmpresaResponse, EmpresaResponse>(c =>
            c.BaseAddress = new Uri(_configuration["ApiConfig:BaseUrl"]))
                .AddPolicyHandler(retryPolicy);
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Teste");
                c.RoutePrefix = String.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
