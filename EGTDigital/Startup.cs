using EGTDigital.Entities;
using EGTDigital.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace EGTDigital
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddXmlDataContractSerializerFormatters();

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });

            string dbConnectionString = Configuration.GetConnectionString("DbConnection");

            services.AddDbContext<EgtDbContext>(options =>
               options.UseNpgsql(dbConnectionString));

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DBConnectionManager.SetAppBuilder(app);

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<EgtDbContext>();
                dbContext.Database.EnsureCreated();

                RatesCollector currencyUpdater = new RatesCollector();

                int refreshInterval = Convert.ToInt16(Configuration.GetSection("CurrencyRefreshInterval").Value);

                string url = Configuration.GetSection("CurrencyUrl").Value + Configuration.GetSection("CurrencyAuthToken").Value;
                Task.Factory.StartNew(() => currencyUpdater.StartUpdating(refreshInterval, url));
            }

            string rabbitMqExchange = Configuration.GetSection("RabbitMqExchange").Value.ToString();

            MessageSenderService.SetRabbitMqExchange(rabbitMqExchange);
        }
    }
}
