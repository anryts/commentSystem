using CommentarySystem.Server.Services;
using CommentarySystem.Server.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Common.Middlewares;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options => options.AddDefaultPolicy(corsPolicyBuilder =>
{
    corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

app.UseExceptionHandler(opt => { });

app.UseDefaultFiles();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(); // Use the CORS policy
app.MapControllers();
app.MapFallbackToFile("/index.html");
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
    dbContext.Database.Migrate();
}

app.Run();

