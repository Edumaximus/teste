using Microsoft.AspNetCore.Mvc;
using API.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Carro> carros = [
    new Carro (){Id = 1, Name = "Fusca"},
    new Carro (){Id = 2, Name = "Ferrari"}
];

app.MapGet("/", () => "Hello World!");
app.MapGet("/api/carros",()=>{
    return Results.Ok(carros);
});

app.Run();
