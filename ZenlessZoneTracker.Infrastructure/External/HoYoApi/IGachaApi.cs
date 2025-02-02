using Refit;
using ZenlessZoneTracker.Infrastructure.External.HoYoApi.Responses;

namespace ZenlessZoneTracker.Infrastructure.External.HoYoApi;

public interface IGachaApi
{
    [Get("/common/gacha_record/api/getGachaLog")]
    Task<GachaResponse> GetGachaLogAsync(
        [AliasAs("authkey")] string authKey,
        [AliasAs("authkey_ver")] int authKeyVer,
        [AliasAs("sign_type")] int signType,
        [AliasAs("lang")] string lang,
        [AliasAs("game_biz")] string gameBiz,
        [AliasAs("gacha_type")] int gachaType,
        [AliasAs("size")] int size,
        [AliasAs("end_id")] string endId
    );
}