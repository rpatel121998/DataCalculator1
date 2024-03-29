using DataCalculatorWebAppServer.Data;
using DataCalculatorWebAppServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<DataCalculatorDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataCalculatorDb"));
});

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<IAwsS3FileManager, AwsS3FileManager>();
builder.Services.AddScoped<IFileHandler, FileHandler>();
builder.Services.AddScoped<MetaDataDbContext>();
//builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });
//var client = new MongoClient("mongodb+srv://rpatel1:Yash2001@cluster0.fbfor.mongodb.net/NDVData?retryWrites=true&w=majority");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
