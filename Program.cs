using SignalR_Example.Hub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();   // ���U SignalR �A��

builder.Services.AddCors(options =>    // ���U CORS �A�ȡA���\��ӷ��ШD
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
app.UseCors("CORSPolicy");   // �ϥΤW���w�q�� CORS ����
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseFileServer();    // �ϥΤ��ت������n�鴣���R�A�ɮ�
app.MapHub<MessageHub>("/messageHub");   // �M�g SignalR Hub �� "/messageHub" (���e�ݪ��s�u�r��)
app.Run();
