using System.Security.Claims;
using ECommerce.Application.Interfaces.Services;

namespace ECommerce.WebAPI.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId => GetUserId();

    private Guid? GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId is not null ? Guid.Parse(userId) : null;
    }
}

