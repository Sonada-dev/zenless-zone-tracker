using Microsoft.Extensions.Primitives;
using ZenlessZoneTracker.Common.Contracts.Requests;
using ZenlessZoneTracker.Common.Contracts.Responses;

namespace ZenlessZoneTracker.Application.Interfaces;

public interface IFetchGachaService
{
    Task<FetchResponse> FetchGachaHistoryAsync(FetchRequest request, Dictionary<string, StringValues> queryParams);
}