using BankSystem.API.data;
using BankSystem.APII.repository;
using Microsoft.EntityFrameworkCore;


public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddScoped<IContaRepository, ContaRepository>();


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<BankContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BankDatabase")));


        var app = builder.Build();


       
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        
        app.MapGet("/health", async (BankContext context) =>
        {
            try
            {
                await context.Database.CanConnectAsync();
                return Results.Ok(new
                {
                    status = "SQL Server conectado com sucesso!",
                    database = context.Database.GetDbConnection().Database,
                    server = context.Database.GetDbConnection().DataSource,
                    message = "Instância: SQL"
                });
            }
            catch (Exception e)
            {
                return Results.Problem($"Falha na conexão: {e.Message}");
            }
        });

        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<BankContext>();

                
                await context.Database.MigrateAsync(); 

                Console.WriteLine("Migrações do banco de dados aplicadas com sucesso!");
                Console.WriteLine($"Banco: BankDb");
            }
            catch (Exception ex)
            {
                
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Um erro ocorreu ao aplicar as migrações.");
            }
        }

       
        app.Run();
    }
}