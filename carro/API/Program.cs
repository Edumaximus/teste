using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using API.Models;
using System.ComponentModel.DataAnnotations;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Swagger Documentação API Carros",
        Description = "Endpoints para gerenciamento de cadastro de carros",
        Contact = new OpenApiContact(){
            Name = "Eduardo Zaduski",
            Email = "eduardozaduski@gmail.com"
        },
        License = new OpenApiLicense(){
            Name = "MIT License",
            Url = new Uri ("htps://opensource.org/licenses/MIT")
        }

    });
    
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

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

    carro.Modelo = ctx.Modelos.Find(carro.Modelo.Id);

    if(carro.Modelo == null){
        return Results.BadRequest("Modelo não existe!");
    }

    if(carro.Name == null || carro.Name.Length < 3){
        return Results.BadRequest("Nome do carro deve conter mais de 3 char");
    }


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
    entidade.Modelo=ctx.Modelos.Find(id);

    if(entidade.Modelo == null){
        return Results.BadRequest("Modelo não existe!");
    }

    if(carro.Name == null || carro.Name.Length < 3){
        return Results.BadRequest("Nome do carro deve conter mais de 3 char");
    }

    if(entidade != null){
        entidade.Name = carro.Name;
        ctx.Carros.Update(entidade);
        ctx.SaveChanges();
        return Results.Ok(entidade);
    }
    return Results.NotFound();
});

//DELETE: remove um carro pelo id
app.MapDelete("api/carros/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx)=>{
    Carro? carro = ctx.Carros.Find(id);
    if(carro==null){
        return Results.NotFound();
    }
    ctx.Carros.Remove(carro);
    ctx.SaveChanges();
    return Results.NoContent();
});

//GET: Lista todos os modelos cadastrados
app.MapGet("api/modelos", ([FromQuery] string? name, [FromServices] AppDataContext ctx) =>{
    var query = ctx.Modelos.AsQueryable();

    if(!string.IsNullOrWhiteSpace(name)){
        query = query.Where(m=>EF.Functions.Like(m.Name, $"%{name}%"));
    }

    var modelos = query.ToList();
    if(modelos == null || modelos.Count == 0){
        return Results.NotFound();
    }
    return Results.Ok(modelos);
});

//GET: Busca o modelo cadastrado pelo id
app.MapGet("api/modelos/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>{
    var modelo = ctx.Modelos.Find(id);
    if(modelo == null){
        return Results.NotFound();
    }
    return Results.Ok(modelo);
});

app.Run();