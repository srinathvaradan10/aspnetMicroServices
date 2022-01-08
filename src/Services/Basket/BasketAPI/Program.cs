using BasketAPI.gRPCServices;
using static DiscountgRPC.Protos.DiscountProtoService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;


builder.Services.AddControllers();
builder.Services.AddTransient<BasketAPI.Repositories.IBasketRepository, BasketAPI.Repositories.BasketRepository>();
builder.Services.AddGrpcClient<DiscountProtoServiceClient>(o => o.Address = new Uri(configuration.GetValue<string>("GrpcSettings:DiscountUrl")));
builder.Services.AddScoped<DiscountgRPCService>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
