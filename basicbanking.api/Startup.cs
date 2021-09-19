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
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

using basicbanking.api.Data;
using basicbanking.api.Domain;
using basicbanking.api.Controllers;

namespace basicbanking.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "basicbanking.api", Version = "v1" });
            });

            var databaseURL = Configuration["DATABASE_URL"];

            if (string.IsNullOrEmpty(databaseURL))
            {
                Console.WriteLine("Mode: In memory database");
                services.AddDbContext<PostgresDbContext>(options => options.UseInMemoryDatabase(databaseName: "basicbank"));
            }
            else
            {
                Uri uri;
                Uri.TryCreate(databaseURL, UriKind.Absolute, out uri);
                var connectionString = $"Server={uri.Host};Port={uri.Port};Database={uri.LocalPath.Substring(1)};User Id={uri.UserInfo.Split(':')[0]};Password={uri.UserInfo.Split(':')[1]}";
                services.AddEntityFrameworkNpgsql().AddDbContext<PostgresDbContext>(options => options.UseNpgsql(connectionString));
            }

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "basicbanking.api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PostgresDbContext>();

                if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    context.Database.Migrate();
                }

                IRepository<User> userRepo = serviceScope.ServiceProvider.GetService<IRepository<User>>();
                IRepository<BankAccount> bankAccountRepo = serviceScope.ServiceProvider.GetService<IRepository<BankAccount>>();

                var defaultUser = userRepo.Find(u => u.Name == "Default User").FirstOrDefault();
                if (defaultUser == null)
                {
                    defaultUser = new User
                    {
                        Name = "Default User",
                        CreatedDate = DateTime.Now,
                    };
                    userRepo.Insert(defaultUser);
                    var defaultUserAccount = new BankAccount
                    {
                        IBAN = "NL66ABNA7951708135",
                        Balance = 50,
                        UserId = defaultUser.Id
                    };
                    bankAccountRepo.Insert(defaultUserAccount);
                }
            }
        }
    }
}
