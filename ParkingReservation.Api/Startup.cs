using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ParkingReservation.Api.Configuration;
using ParkingReservation.Api.Models;
using ParkingReservation.Core;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;
using ParkingReservation.Core.Tests.PriceRules;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ParkingReservation.Api
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
            services.AddControllers()
                    .AddJsonOptions(options => 
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    );

            services.AddVersioning();
            services.AddSwaggerGen();

            services.AddAutoMapper(typeof(MappingProfile));

            var priceRules = new List<IPriceRule>()
            {
                new SummerPriceRule(),
                new WinterPriceRule()
            };

            var pricingConfig = new PricingConfig(Configuration);

            services.AddTransient<IAvailabilityService>((services) => new AvailabilityService());
            services.AddSingleton<IBookingService>((services) => new BookingService(BookableItems.Items));
            services.AddSingleton<IPricingService>((services) => new PricingService(priceRules, pricingConfig));
            services.AddSingleton<IParkingService>((services) => new ParkingService(
                services.GetRequiredService<IAvailabilityService>(),
                services.GetRequiredService<IBookingService>(),
                services.GetRequiredService<IPricingService>()
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class BookableItems
        {
            public static readonly List<IBookable> Items = new (
                Enumerable.Range(1, 10).ToList().Select(i => new CarParkingSpot(i.ToString())).ToList()
               );
        }
    }
}
