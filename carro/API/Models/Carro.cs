namespace API.Models;
public class Carro
{
    public int Id { get; set;}
    public string Name{ get; set;}
    public Modelo? Modelo {get; set;}
    public DateTime CriadoEm {get; set;}

    public Carro(){
        this.CriadoEm = DateTime.Now;
    }
}