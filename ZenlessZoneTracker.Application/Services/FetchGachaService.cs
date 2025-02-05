using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using ZenlessZoneTracker.Application.Interfaces;
using ZenlessZoneTracker.Common.Contracts.Dto;
using ZenlessZoneTracker.Common.Contracts.Requests;
using ZenlessZoneTracker.Common.Contracts.Responses;
using ZenlessZoneTracker.Domain.Entities;
using ZenlessZoneTracker.Domain.Enums;
using ZenlessZoneTracker.Infrastructure.External.HoYoApi;

namespace ZenlessZoneTracker.Application.Services;

public class FetchGachaService(IGachaApi gachaApi, ILogger<FetchGachaService> logger) : IFetchGachaService
{
    public async Task<FetchResponse> FetchGachaHistoryAsync(FetchRequest request,
        Dictionary<string, StringValues> queryParams)
    {
        var requiredParams = new[] { "authkey", "authkey_ver", "sign_type", "game_biz" };
        foreach (var param in requiredParams)
            if (!queryParams.TryGetValue(param, out var value) || string.IsNullOrEmpty(value))
            {
                logger.LogError($"FetchGachaHistory: Missing or invalid {param}.");
                throw new ArgumentException($"Missing or invalid {param}");
            }

        var lang = queryParams.TryGetValue("lang", out var langValue) ? langValue.ToString() : "en";
        var gameBiz = queryParams["game_biz"].ToString();

        logger.LogInformation("FetchGachaHistory: Starting retrieval for game_biz {GameBiz} with language {Lang}",
            gameBiz, lang);

        var pullsByGachaType = Enum.GetValues<GachaType>()
            .ToDictionary(gachaType => gachaType, _ => new List<GachaItem>());

        try
        {
            foreach (var gachaType in Enum.GetValues<GachaType>())
            {
                var allPulls = new List<GachaItem>();
                var nextEndId = request.EndId ?? "0";

                do
                {
                    logger.LogDebug("FetchGachaHistory: Requesting pulls with end_id {NextEndId}", nextEndId);

                    var response = await gachaApi.GetGachaLogAsync(
                        queryParams["authkey"].ToString(),
                        int.Parse(queryParams["authkey_ver"].ToString()),
                        int.Parse(queryParams["sign_type"].ToString()),
                        lang,
                        gameBiz,
                        (int)gachaType,
                        20,
                        nextEndId
                    );

                    if (response.Retcode != 0)
                    {
                        logger.LogWarning("FetchGachaHistory: Error {Retcode}: {Message}", response.Retcode,
                            response.Message);
                        throw new Exception($"Error {response.Retcode}: {response.Message}");
                    }

                    if (response.Data.List.Count == 0)
                    {
                        logger.LogInformation("FetchGachaHistory: No more pulls found for {GachaType}, stopping.",
                            gachaType);
                        break;
                    }

                    allPulls.AddRange(response.Data.List);
                    nextEndId = response.Data.List.Last().Id;
                } while (!string.IsNullOrEmpty(nextEndId));

                pullsByGachaType[gachaType] = allPulls;
            }

            return new FetchResponse
            {
                Standard = CreateFetchItem(pullsByGachaType[GachaType.Standard]),
                Event = CreateFetchItem(pullsByGachaType[GachaType.Event]),
                Weapon = CreateFetchItem(pullsByGachaType[GachaType.Weapon]),
                Bangboo = CreateFetchItem(pullsByGachaType[GachaType.Bangboo])
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "FetchGachaHistory: An error occurred.");
            throw;
        }
    }

    private FetchItem CreateFetchItem(List<GachaItem> pulls)
    {
        return new FetchItem
        {
            List = pulls,
            TotalCount = pulls.Count
        };
    }
}