using Microsoft.AspNetCore.Mvc;
using OPI.HelpDesk.Application.DTOs.Ticket;
using OPI.HelpDesk.Application.Interfaces.Ticket;

namespace OPI.HelpDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketRequest request)
        {
            try
            {
                var result = await _ticketService.CreateAsync(request);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ticketService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (ticket is null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTicketRequest request)
        {
            var ticket = await _ticketService.UpdateAsync(id, request);

            if (ticket is null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPut("{id:guid}/assign")]
        public async Task<IActionResult> Assign(Guid id, AssignTicketRequest request)
        {
            try
            {
                var ticket = await _ticketService.AssignAsync(id, request);

                if (ticket is null)
                    return NotFound();

                return Ok(ticket);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _ticketService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }
}
