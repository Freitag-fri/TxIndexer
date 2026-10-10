using TxIndexer.Api.Data;
using TxIndexer.Api.Domain;
using TxIndexer.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();

var app = builder.Build();

var repository = app.Services.GetRequiredService<ITransactionRepository>();

await repository.AddTransactionAsync(new Transaction(
    "0x5c504ed432cb51138bcf09aa5e8a410dd4a1e204ef84bfed1be16dfba1b22060",
    21_000_001,
    "0x742d35cc6634c0532925a3b844bc454e4438f44e",
    "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae",
    125.50m,
    "USDT",
    DateTimeOffset.UtcNow.AddMinutes(-10).ToUnixTimeSeconds(),
    TransactionStatus.Confirmed), CancellationToken.None);

await repository.AddTransactionAsync(new Transaction(
    "0x5c504ed432cb51138bcf09aa5e8a410dd4a1e204ef84bfed1be16dfba1b22061",
    21_000_001,
    "0x742d35cc6634c0532925a3b844bc454e4438f44e",
    "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae",
    125.50m,
    "USDT",
    DateTimeOffset.UtcNow.AddMinutes(-9).ToUnixTimeSeconds(),
    TransactionStatus.Pending), CancellationToken.None);

await repository.AddTransactionAsync(new Transaction(
    "0x5c504ed432cb51138bcf09aa5e8a410dd4a1e204ef84bfed1be16dfba1b22062",
    21_000_001,
    "0x742d35cc6634c0532925a3b844bc454e4438f44e",
    "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae",
    125.50m,
    "USDT",
    DateTimeOffset.UtcNow.AddMinutes(-8).ToUnixTimeSeconds(),
    TransactionStatus.Failed), CancellationToken.None);

await repository.AddTransactionAsync(new Transaction(
    "0x5c504ed432cb51138bcf09aa5e8a410dd4a1e204ef84bfed1be16dfba1b22063",
    21_000_001,
    "0x742d35cc6634c0532925a3b844bc454e4438f44e",
    "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae",
    125.50m,
    "USDT",
    DateTimeOffset.UtcNow.AddMinutes(-8).ToUnixTimeSeconds(),
    TransactionStatus.Failed), CancellationToken.None);

await repository.AddTransactionAsync(new Transaction(
    "0x5c504ed432cb51138bcf09aa5e8a410dd4a1e204ef84bfed1be16dfba1b22064",
    21_000_001,
    "0x742d35cc6634c0532925a3b844bc454e4438f44e",
    "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae",
    125.50m,
    "USDT",
    DateTimeOffset.UtcNow.AddMinutes(-7).ToUnixTimeSeconds(),
    TransactionStatus.Confirmed), CancellationToken.None);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
