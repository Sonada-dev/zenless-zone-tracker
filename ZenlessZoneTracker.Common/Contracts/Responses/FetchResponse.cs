using ZenlessZoneTracker.Common.Contracts.Dto;

namespace ZenlessZoneTracker.Common.Contracts.Responses;

public class FetchResponse
{
    public FetchItem Standard { get; set; } = new();
    public FetchItem Event { get; set; } = new();
    public FetchItem Weapon { get; set; } = new();
    public FetchItem Bangboo { get; set; } = new();
}