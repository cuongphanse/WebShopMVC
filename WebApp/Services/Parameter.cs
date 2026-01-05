using System.Data;

namespace WebApp.Services;
public class Parameter
{
    public string Name { get; set; } = null!;   
    public object? Value { get; set; }
    public DbType DbType { get; set;}
}   