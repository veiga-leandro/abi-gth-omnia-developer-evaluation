using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Seeds
{
    public static class SeedData
    {
        private const string LoggerCategory = "SeedData";

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(LoggerCategory);

            var context = serviceProvider.GetRequiredService<DefaultContext>();
            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();

            try
            {
                // Aplica migrations pendentes se necessário
                await context.Database.MigrateAsync();

                // Seed de usuários
                await SeedUsersAsync(context, passwordHasher, logger);

                logger.LogInformation("Seed data completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database seed");
                throw;
            }
        }

        private static async Task SeedUsersAsync(DefaultContext context, IPasswordHasher passwordHasher, ILogger logger)
        {
            // Verifica se já existe algum usuário admin
            if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            {
                logger.LogInformation("Admin user already exists - skipping user seed");
                return;
            }

            logger.LogInformation("Creating initial admin user");

            // Cria o usuário admin inicial
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                Password = passwordHasher.HashPassword("Admin@123"), // Senha forte inicial
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            // Adiciona ao contexto
            context.Users.Add(adminUser);

            // Salva as mudanças
            await context.SaveChangesAsync();

            logger.LogInformation("Admin user created successfully. Email: admin@example.com, Password: Admin@123");
        }
    }

}
