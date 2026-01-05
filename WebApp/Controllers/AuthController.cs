using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;
public class AuthController : Controller
{
    MailSetting setting;
    MemberRepository memberRepository;
    public AuthController(IConfiguration configuration, IOptions<MailSetting> options)
    {
        memberRepository = new MemberRepository(configuration);
        setting = options.Value;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel obj)
    {
        if (ModelState.IsValid)
        {
            Member? member = memberRepository.GetMember(obj);
            if(member != null)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, member.Id),
                    new Claim(ClaimTypes.Name, member.Name),
                    new Claim(ClaimTypes.GivenName, member.GivenName),
                    new Claim(ClaimTypes.Email, member.Email),
                };
                if(!string.IsNullOrEmpty(member.SurName)) claims.Add(new Claim(ClaimTypes.Surname, member.SurName));

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal, new AuthenticationProperties
                {
                    IsPersistent = obj.Remember
                });
                 TempData["Msg"] = "Login Success";
                return Redirect("/auth");
                // ModelState.AddModelError("Error", "Login Success");
            }
            else
            {
                ModelState.AddModelError("Error", "Login fail");
            }
        }
        return View(obj);
    }
    public IActionResult Change(ChangeModel obj)
    {
        ModelState.Remove(nameof(obj.MemberId));
        if (ModelState.IsValid)
        {
            string? memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(memberId is null) return Redirect("/auth/login");
            obj.MemberId = memberId;
            if(memberRepository.Change(obj) > 0)
            {
                TempData["Msg"] = "Change Password success";
                return Redirect("/auth/logout");
            }
            ModelState.AddModelError("Error","Old Password Failed");
        }
        return View(obj);
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/auth/login");
    }
    public IActionResult Success()
    {
        return View();
    }
    public IActionResult Verify(string token)
    {
        int ret = memberRepository.Active(token);
        if(ret > 0)
        {
            TempData["Msg"] = "Active Account Success";
            return Redirect("/auth/login");
        }
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel obj)
    {
        if (ModelState.IsValid)
        {
            obj.Token = Helper.RamdomString(32);
            string body = $"<a href=\"http://localhost:5158/auth/verify?token={obj.Token}\">Email you click here</a>";
            bool res = await Helper.SendMail(setting, obj.Email, "Verify You Email", body);
            if (res)
            {
                int ret = memberRepository.Add(obj);
                if(ret > 0) {
                    TempData["Msg"] = $"Register Success Please check your email {obj.Email} active account";
                    // return Redirect("/auth/login");
                    return Redirect("/auth/success");
                }
                ModelState.AddModelError("Error","Resgister Failed");
            }
            else
            {
                ModelState.AddModelError("Error",$"Your mail {obj.Email} invalid");
            }
           
        }
        return View(obj);
    }
}