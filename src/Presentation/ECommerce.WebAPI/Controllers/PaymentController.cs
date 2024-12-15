using ECommerce.Application.Extensions;
using ECommerce.Application.Features.Payments.Pay;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

public sealed class PaymentController : BaseApiController
{
    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] PayCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
