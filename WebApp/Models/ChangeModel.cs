namespace WebApp.Models;
public class ChangeModel
{
    public string MemberId {get;set;} = null!;
    public string OldPassword {get;set;} = null!;
    public string NewPassword {get;set;} = null!;
}