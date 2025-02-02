using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ZenlessZoneTracker.Application.Interfaces;
using ZenlessZoneTracker.Common.Contracts.Requests;

namespace ZenlessZoneTracker.Api.Controllers;

[ApiController]
[Route("api/gacha")]
public class GachaController(ILogger<GachaController> logger, IFetchGachaService fetchGachaService) : ControllerBase
{
    [HttpPost("fetch")]
    public async Task<IActionResult> FetchGachaHistory([FromBody] FetchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.GachaUrl) ||
            !Uri.IsWellFormedUriString(request.GachaUrl, UriKind.Absolute))
        {
            logger.LogWarning("FetchGachaHistory: Empty or invalid URL received.");
            throw new ArgumentException("Invalid URL");
        }

        logger.LogInformation("FetchGachaHistory: Received request with URL: {GachaUrl}", request.GachaUrl);

        var uri = new Uri(request.GachaUrl);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);

        logger.LogInformation("FetchGachaHistory: Parsed query parameters: {QueryParams}", queryParams);

        try
        {
            logger.LogInformation("FetchGachaHistory: Calling FetchGachaHistoryAsync with request: {Request}", request);
            var result = await fetchGachaService.FetchGachaHistoryAsync(request, queryParams);

            logger.LogInformation("FetchGachaHistory: Successfully fetched gacha history for URL: {GachaUrl}",
                request.GachaUrl);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "FetchGachaHistory: Invalid argument exception occurred.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "FetchGachaHistory: Unexpected error occurred.");
            return StatusCode(500, "Internal server error");
        }
    }
}