namespace WebApp.Services;
public class MailSetting
{
    public string Host {get; set;} = null!;
    public int Port {get; set;}
    public string Email {get; set;} = null!;
    public string Password {get; set;} = null!;
    public string DisplayName {get; set;} = null!;
}