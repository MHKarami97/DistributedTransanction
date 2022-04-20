using Oms.Context;
using Oms.Repository;
using OpenTelemetry.Trace;
using Prometheus;
using Saga.V2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<RequestRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddOpenTelemetryTracing((builder) => builder
    .AddSource("DistributedTransaction")
    .AddAspNetCoreInstrumentation()
    .AddEntityFrameworkCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddConsoleExporter()
    .AddJaegerExporter());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.UseMetricServer();

app.MapControllers();

app.Run();