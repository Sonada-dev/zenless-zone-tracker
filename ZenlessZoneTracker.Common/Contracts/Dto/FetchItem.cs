using ZenlessZoneTracker.Domain.Entities;

namespace ZenlessZoneTracker.Common.Contracts.Dto;

public class FetchItem
{
    public int TotalCount { get; set; }
    public List<GachaItem> List { get; set; } = [];
}