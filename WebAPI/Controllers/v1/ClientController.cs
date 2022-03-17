using Application.Features.Clients.Commands.CreateClientCommand;
using Application.Features.Clients.Commands.DeleteClientCommand;
using Application.Features.Clients.Commands.UpdateClientCommand;
using Application.Features.Clients.Queries;
using Application.Features.Clients.Queries.GetAllClients;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ClientController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateClientCommand command)
        {
            return Ok(await Mediator?.Send(command)!);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateClientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            return Ok(await Mediator?.Send(command)!);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Put(int id)
        {
            return Ok(await Mediator?.Send(new DeleteClientCommand { Id = id })!);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator?.Send(new GetClientByIdQuery { Id = id })!);
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] GetAllClientsParameters filter)
        {
            return Ok(await Mediator?.Send(new GetAllClientsQuery
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Name = filter.Name,
                Surname = filter.Surname
            })!);
        }
    }
}