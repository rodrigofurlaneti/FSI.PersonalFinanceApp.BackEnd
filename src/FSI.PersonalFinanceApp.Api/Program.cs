using FSI.PersonalFinanceApp.Api.DependencyInjection;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ?? Aplicação e Infraestrutura separadas
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

// Extras
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS precisa vir aqui 👇
app.UseCors();

app.UseAuthorization();
app.MapControllers();
app.Run();
