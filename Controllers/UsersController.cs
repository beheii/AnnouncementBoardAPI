using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoticeBoard.Repositories;

namespace NoticeBoard.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult> GetCurrentUser()
    {
        var sub = User.FindFirstValue("sub");
        var email = User.FindFirstValue("email");
        var name = User.FindFirstValue("name");

        if (string.IsNullOrEmpty(sub) || string.IsNullOrEmpty(email))
            return BadRequest("Missing required claims.");

        var user = await userRepository.FindOrCreateAsync(sub, email, name);
        return Ok(user);
    }
}
