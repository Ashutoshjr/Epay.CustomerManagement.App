using AutoMapper;
using CustomerManagement.API.Extension;
using CustomerManagement.Application.Customers;
using CustomerManagement.Domain.Customers;
using CustomerManagement.Infrastructure;
using Microsoft.OpenApi.Models;

namespace CustomerManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                // Add services to the container.
                builder.Services.AddControllers();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();



                builder.Services.ConfigureCors();
                builder.Services.ConfigureAutoMapper();



                builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
                builder.Services.AddScoped<ICustomerService, CustomerService>();
            }




            var app = builder.Build();
            {
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseDefaultFiles();
                app.UseStaticFiles();
               
                
                app.UseCors("MyPolicy");

                app.MapControllers();

                app.Run();
            }
        }
    }
}