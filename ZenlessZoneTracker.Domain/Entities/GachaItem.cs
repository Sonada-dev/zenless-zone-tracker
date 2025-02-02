namespace ZenlessZoneTracker.Domain.Entities;

public class GachaItem
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string GachaId { get; set; } = null!;
    public string GachaType { get; set; } = null!;
    public string ItemType { get; set; } = null!;
    public string RankType { get; set; } = null!;
    public string Time { get; set; } = null!;
    public string Lang { get; set; } = null!;
}