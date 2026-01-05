using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Areas.Dashboard.Controllers;
[Area("dashboard")]
public class MemberController : Controller
{
    MemberRepository memberRepository;
    public MemberController(IConfiguration configuration)
    {
        memberRepository = new MemberRepository(configuration);
    }
    public IActionResult Index()
    {
        return View(memberRepository.GetMembers());
    }
}