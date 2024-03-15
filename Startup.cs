using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using retonet.Services;
using Microsoft.Extensions.Configuration; // Importante para acceder a la configuración

[assembly: FunctionsStartup(typeof(retonet.Startup))]

namespace retonet
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Obtiene la configuración del contexto del host de funciones
            var configuration = builder.GetContext().Configuration;
            
            // Obtiene la cadena de conexión desde las configuraciones
            var connectionString = configuration["SqlConnectionString"];

            // Configura el contexto de la base de datos con la cadena de conexión
            builder.Services.AddDbContext<RetonetDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registra el servicio de usuario como un singleton
            builder.Services.AddSingleton<IUserService, UserService>();
            // Agrega más servicios según sea necesario.
        }
    }
}