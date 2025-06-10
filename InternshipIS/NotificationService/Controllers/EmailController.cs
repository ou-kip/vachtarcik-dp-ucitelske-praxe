using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Controllers.Base;

namespace NotificationService.Controllers
{
    [ApiVersion("1.0")]
    public class EmailController : BaseController
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] string message)
        {
            //var result = await _mediator.Send(message);

            //if (!result.HasErrors)
            //{
            //    return Ok(result.Data); // Vrátí ID uživatele
            //}
            //else
            //{
            //    return BadRequest(result.Errors); // Vrátí chyby
            //}

            throw new NotImplementedException("Not implemented.");
        }
    }
}
