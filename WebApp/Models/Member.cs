namespace WebApp.Models; 
public class Member
{
    public string Id {get;set;} = null!;
    public string GivenName {get;set;} = null!;
    public string? SurName {get;set;} = null!;
    public string Name {get;set;} = null!;
    public string Email {get;set;} = null!;
    public byte[] Password {get;set;} = null!;
    public bool IsActived {get;set;}
    public string? Token {get;set;} 
}