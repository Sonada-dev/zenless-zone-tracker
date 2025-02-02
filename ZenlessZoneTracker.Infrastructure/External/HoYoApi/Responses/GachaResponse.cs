using ZenlessZoneTracker.Domain.Entities;

namespace ZenlessZoneTracker.Infrastructure.External.HoYoApi.Responses;

public class GachaResponse
{
    public int Retcode { get; set; }
    public string Message { get; set; }
    public GachaData Data { get; set; } = new();
}