using LoyaltyCard.Application.Commands.AddLoyaltyCard;
using LoyaltyCard.Application.Commands.UpdateLoyaltyCard;
using LoyaltyCard.Application.Queries.GetLoyaltyCardByCustomerId;
using LoyaltyCard.Application.Dtos;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyCard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoyaltyCardController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoyaltyCardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ===================== GET =====================

    [HttpGet("{customerId:guid}")]
    [ProducesResponseType(typeof(LoyaltyCardResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoyaltyCardResponseDto>> GetByCustomerId(
        Guid customerId,
        CancellationToken token)
    {
        var query = new GetLoyaltyCardByCustomerIdQuery(customerId);

        var result = await _mediator.Send(query, token);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    // ===================== POST =====================

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] AddLoyaltyCardDto dto,CancellationToken token)
    {
        var command = new AddLoyaltyCardCommand(dto.CustomerId);

        var id = await _mediator.Send(command, token);

        return CreatedAtAction(nameof(GetByCustomerId),new { customerId = dto.CustomerId },id);
    }

    // ===================== PUT =====================

    [HttpPut("{customerId:guid}/points")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePoints(Guid customerId,[FromBody] UpdateLoyaltyCardPointsDto dto, CancellationToken token)
    {
        var command = new UpdateLoyaltyCardCommand(customerId, dto.Points);

        await _mediator.Send(command, token);

        return NoContent();
    }
}