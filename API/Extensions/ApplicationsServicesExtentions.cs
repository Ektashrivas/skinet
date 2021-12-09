using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationsServicesExtentions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<iProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
              services.Configure<ApiBehaviorOptions>(options=>
            {
                options.InvalidModelStateResponseFactory = ApplicationModelConventionExtensions => 
                {
                    var errors = ApplicationModelConventionExtensions.ModelState
                       .Where(e=>e.Value.Errors.Count > 0)
                       .SelectMany(x=>x.Value.Errors)
                       .Select(x=>x.ErrorMessage).ToArray();

                       var errorResponse = new ApiValidationErrorResponse
                       {
                           Errors=errors
                       };
                       return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }

        }
    }
