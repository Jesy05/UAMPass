// Esta es la configuración principal de una aplicación web ASP.NET Core MVC.
// Configura servicios como MVC, Entity Framework Core con PostgreSQL, y sesiones.
// También define el pipeline de solicitudes HTTP y las rutas predeterminadas.

using Microsoft.EntityFrameworkCore;
using UAMPass.Models;

var builder = WebApplication.CreateBuilder(args);


// 1) MVC ( lo que quiere decir que usaremos Controladores y Vistas )
// MVC es un patrón de diseño para aplicaciones web y significa Modelo-Vista-Controlador
// El patrón MVC separa una aplicación en tres componentes principales:
// - Modelo: representa los datos y la lógica de negocio de la aplicación.
// - Vista: es la interfaz de usuario que muestra los datos del modelo.
// - Controlador: maneja la entrada del usuario, interactúa con el modelo y selecciona la vista para renderizar.

builder.Services.AddControllersWithViews();


// 2) BASE DE DATOS (PostgreSQL) 
// Usamos Entity Framework Core como ORM
// La cadena de conexión está en appsettings.json
//el .json es un archivo de configuración basado en texto plano
//que utiliza una sintaxis de notación de objetos de JavaScript (JSON) para almacenar datos estructurados.
//en este caso, se utiliza para almacenar configuraciones de la aplicación, como cadenas de conexión a bases de datos,

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));



// 3) SESSION (NECESARIO PARA LOGIN)
// Se usa para mantener el estado del usuario a través de múltiples solicitudes HTTP.
// La sesión permite almacenar datos específicos del usuario en el servidor,
// lo que es útil para funcionalidades como el inicio de sesión, carritos de compra, etc.

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // Tiempo activa
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // <-- NECESARIO
});



// 4) BUILD APP
// Construimos la aplicación
// Aquí es donde se configura el pipeline de solicitudes HTTP
// La construcción de la aplicación es el proceso de ensamblar todos los componentes y configuraciones necesarios
// para que la aplicación web funcione correctamente.

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


// 5) ARCHIVOS ESTÁTICOS (wwwroot)
// Los archivos estáticos son archivos que no cambian dinámicamente
// y se sirven directamente al cliente, como imágenes, archivos CSS, archivos JavaScript, etc.

app.UseStaticFiles();


// 6) ROUTING
// El enrutamiento es el proceso de determinar cómo una aplicación responde a una solicitud de un cliente
// a una URL específica.

app.UseRouting();


// 7) SESSION (DEBE IR AQUÍ ANTES DE Authorization)
// Aqui se habilita el uso de sesiones en la aplicación web
// La sesión permite almacenar y recuperar datos específicos del usuario
// a lo largo de múltiples solicitudes HTTP.

app.UseSession();


// 8) AUTHORIZATION
// La autorización es el proceso de determinar si un usuario tiene permiso para acceder a un recurso específico
// o realizar una acción determinada.

app.UseAuthorization();



// 9) RUTAS
// Aquí se definen las rutas predeterminadas para los controladores y acciones
// Una ruta define cómo se mapean las URL a los controladores y acciones en una aplicación MVC.
// Esto permite que la aplicación responda a diferentes solicitudes de URL de manera adecuada.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
