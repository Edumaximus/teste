using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

var app = builder.Build();

//endpoints relacionados ao recurso de carros
//GET: Lista de todos os carros cadastrados

app.MapGet("api/carros",([FromServices] AppDataContext ctx)=>{
    if(ctx.Carros.Any()){
        return Results.Ok(ctx.Carros.ToList());
    }
    return Results.NotFound();
});

//POST: Cadastrar carro

app.MapPost("api/carros", ([FromBody] Carro carro, [FromServices] AppDataContext ctx)=>{

    ctx.Carros.Add(carro);
    ctx.SaveChanges();

    return Results.Created("", carro);
});

//GET: Buscar carros pela id
app.MapGet("api/carros/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx)=>{
    Carro? carro = ctx.Carros.Find(id);
    if(carro!=null){
        return Results.Ok(carro);
    }
    return Results.NotFound();
});

//PUT: atualiza os dados do carro pelo id
app.MapPut("api/carros/{id}", ([FromRoute] int id, [FromBody] Carro carro, [FromServices] AppDataContext ctx)=>{
    Carro? entidade=ctx.Carros.Find(id);

    if(entidade != null){
        entidade.Name = carro.Name;
        ctx.Carros.Update(entidade);
        ctx.SaveChanges();
        return Results.Ok(entidade);
    }
    return Results.NotFound();
});

//DELETE: remove um carro pelo id
app.MapDelete("api/carros{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx)=>{
    Carro? carro = ctx.Carros.Find(id);
    if(carro==null){
        return Results.NotFound();
    }
    ctx.Carros.Remove(carro);
    ctx.SaveChanges();
    return Results.NoContent();
});

app.Run();