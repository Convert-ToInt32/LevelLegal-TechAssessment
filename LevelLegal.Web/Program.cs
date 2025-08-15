using LevelLegal.Application.CommandHandler;
using LevelLegal.Domain.Interfaces;
using LevelLegal.Infrastructure.Data;
using LevelLegal.Infrastructure.Repository;
using LevelLegal.Web.Components;
using Microsoft.EntityFrameworkCore;
using System;
using static LevelLegal.Application.CommandHandler.ImportCsvCommandHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICsvImporter>(sp =>
{
    var matterRepo = sp.GetRequiredService<IMatterRepository>();
    var evidenceRepo = sp.GetRequiredService<IEvidenceRepository>();
    return new ImportCsvCommandHandler(matterRepo, evidenceRepo);
});

builder.Services.AddScoped<IMatterRepository, MatterRepository>();
builder.Services.AddScoped<IEvidenceRepository, EvidenceRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
