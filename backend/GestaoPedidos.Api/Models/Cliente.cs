namespace GestaoPedidos.Api.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Cliente() { }

    public Cliente(string nome, string email)
    {
        Nome = nome;
        Email = email;
    }
}