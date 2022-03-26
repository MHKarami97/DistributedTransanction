using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Oms.Context;
using RecoveryAgent;

var builder = WebApplication
    .CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddHostedService<TranactionRecoveryWorker>();

var app = builder.Build();

app.Run();