
using DesafioArvore.Interfaces;
using DesafioArvore.Domain.Services;
using DesafioArvore.Infraestrutura.Repository;
using Microsoft.EntityFrameworkCore;
using DesafioArvore.Infraestrutura.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DesafioArvore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PessoaContext>(options =>
                             options.UseSqlServer(builder.Configuration.GetConnectionString("PessoaContext"),
                             b => b.MigrationsAssembly("DesafioArvore.Infraestrutura")));

            builder.Services.AddScoped<IPessoaDomainService, PessoaDomainService>();
            builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}