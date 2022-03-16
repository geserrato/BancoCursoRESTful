using Application.Features.Clients.Commands.CreateClientCommand;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ClientController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateClientCommand clientCommand)
        {
            return Ok(await Mediator.Send(clientCommand));
        }
    }
}