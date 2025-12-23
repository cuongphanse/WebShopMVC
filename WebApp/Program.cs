var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
var app = builder.Build();

// app.MapGet("/", () => "Hello World!");
app.UseStaticFiles(); // su dung css vs js
app.MapDefaultControllerRoute();
app.Run();
