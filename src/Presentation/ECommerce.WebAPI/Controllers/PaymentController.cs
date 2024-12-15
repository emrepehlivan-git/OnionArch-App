using ECommerce.Application.Extenions;
using ECommerce.Application.Features.Payments.Pay;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

public sealed class PaymentController : BaseApiController
{
    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] PayCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.Map(
            success => Ok(success),
            error => BadRequest(error)
        );
    }
}
