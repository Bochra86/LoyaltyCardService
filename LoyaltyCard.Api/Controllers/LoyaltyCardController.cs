using LoyaltyCard.Application.Queries.GetLoyaltyCardByCustomerId;
using LoyaltyCard.Application.Commands.AddLoyaltyCard;
using LoyaltyCard.Application.Commands.UpdateLoyaltyCard;
using LoyaltyCard.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyCard.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LoyaltyCardController : ControllerBase
{
    private readonly GetLoyaltyCardByCustomerIdHandler _getHandler;
    private readonly AddLoyaltyCardHandler _addHandler;
    private readonly UpdateLoyaltyCardHandler _updateHandler;

    public LoyaltyCardController(GetLoyaltyCardByCustomerIdHandler getHandler,
        AddLoyaltyCardHandler addHandler, UpdateLoyaltyCardHandler updateHandler)
    {
        _addHandler = addHandler;
        _updateHandler = updateHandler;
        _getHandler = getHandler;
    }


    [HttpGet("{customerId}")]
    [ProducesResponseType(typeof(LoyaltyCardResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoyaltyCardResponseDto>> GetByCustomerId([FromRoute] GetLoyaltyCardByCustomerIdQuery query, CancellationToken token)
    {
        var result = await _getHandler.HandleAsync(query, token);

        if (result is null)
            return NotFound();

        return result; // Implicitly converts to Ok(result) 
    }


    [HttpPost]
    public async Task<IActionResult> Create(AddLoyaltyCardDto dto,CancellationToken token)
    {
        var command = new AddLoyaltyCardCommand(dto.CustomerId);
        await _addHandler.HandleAsync(command, token);

        return Ok("Loyalty card created");
    }

    [HttpPut("points")]
    public async Task<IActionResult> UpdatePoints(Guid customerId, UpdateLoyaltyCardPointsDto dto, CancellationToken token)
    {
        var command = new UpdateLoyaltyCardCommand(customerId, dto.Points);
        await _updateHandler.HandleAsync(command, token);

        return NoContent();
    }
}