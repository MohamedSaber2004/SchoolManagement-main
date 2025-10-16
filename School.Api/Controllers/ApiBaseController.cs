using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class ApiBaseController: ControllerBase
{
    protected string GetEmailFromToken() => User.FindFirstValue(ClaimTypes.Email)!;
}
