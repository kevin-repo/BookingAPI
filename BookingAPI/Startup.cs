using AutoMapper.EquivalencyExpression;
using BookingAPI.Contracts.Requests;
using BookingAPI.Repositories;
using BookingAPI.Repositories.Providers;
using BookingAPI.Services;
using BookingAPI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace BookingAPI
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
            services.AddAutoMapper(config =>
            {
                config.AddCollectionMappers();
            }, AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.ImplicitlyValidateChildProperties = true;
                    fv.ImplicitlyValidateRootCollectionElements = true;

                    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
            SaveManually(services);
        }

        private static void SaveManually(IServiceCollection services)
        {
            SaveManuallyServices(services);
            SaveManuallyRepositories(services);
            SaveManuallyProviders(services);
            SaveManuallyValidators(services);
        }

        private static void SaveManuallyValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<BookingCreationRequest>, BookingCreationRequestValidator>();
            services.AddTransient<IValidator<BookingModifyRequest>, BookingModifyRequestValidator>();
        }

        private static void SaveManuallyProviders(IServiceCollection services)
        {
            services.AddTransient<IConnectionProvider, ConnectionProvider>();
        }

        private static void SaveManuallyRepositories(IServiceCollection services)
        {
            services.AddTransient<IBookingRepository, BookingRepository>();
        }

        private static void SaveManuallyServices(IServiceCollection services)
        {
            services.AddTransient<IBookingService, BookingService>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        }
    }
}
