using SignalR_Example.Hub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();   // 註冊 SignalR 服務

builder.Services.AddCors(options =>    // 註冊 CORS 服務，允許跨來源請求
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});     
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CORSPolicy");   // 使用上面定義的 CORS 策略
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseFileServer();    // 使用內建的中介軟體提供靜態檔案
app.MapHub<MessageHub>("/messageHub");   // 映射 SignalR Hub 到 "/messageHub" (為前端的連線字串)
app.Run();
