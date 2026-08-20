using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Application.Mapping;
using Marquee.Application.Services;
using Marquee.Infrastructure.Data;
using Marquee.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MarqueeDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<MarqueeDbContext>());
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddAutoMapper(configuration => configuration.AddProfile<MediaProfile>(), typeof(MediaProfile));
builder.Services.AddAutoMapper(configuration => configuration.AddProfile<PersonProfile>(), typeof(PersonProfile));
builder.Services.AddAutoMapper(configuration => configuration.AddProfile<GenreProfile>(), typeof(GenreProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();