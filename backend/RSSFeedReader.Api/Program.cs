using RSSFeedReader.Api.Endpoints;
using RSSFeedReader.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SubscriptionStore>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("UiPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5213")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("UiPolicy");
app.MapSubscriptionsEndpoints();

app.Run();

public partial class Program
{
}
