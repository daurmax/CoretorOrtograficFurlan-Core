using CoretorOrtografic.API.Hubs;
using CoretorOrtografic.Business;
using Autofac;
using Autofac.Extensions.DependencyInjection;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var frontendUrl = builder.Configuration["FRONTEND_URL"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          if (!string.IsNullOrWhiteSpace(frontendUrl))
                          {
                              policy.WithOrigins(frontendUrl, "http://localhost:4200")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials();
                          }
                          else
                          {
                              policy.WithOrigins("http://localhost:4200")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials();
                          }
                      });
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(
        new CoretorOrtograficDependencyModule(
            builder.Environment.IsDevelopment(),
            CallerApplicationEnum.Web
        )
    );
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseRouting();
app.UseAuthorization();
app.MapHub<SpellCheckHub>("/spellcheckhub");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.Run();
