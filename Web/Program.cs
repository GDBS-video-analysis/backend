using Microsoft.EntityFrameworkCore;
using Web.DataBaseContext;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<VideoAnalisysDBContext>(opt =>
            {
                opt.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            });
            builder.Services.AddHealthChecks();
            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            app.MapHealthChecks("/healthz").RequireHost("*:5000");
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            using (var db = app.Services.CreateScope().ServiceProvider.GetService<VideoAnalisysDBContext>())
            {
                db!.Database.Migrate();
            }

            app.Run();
        }
    }
}
