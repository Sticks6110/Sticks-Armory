using SpaceWarp.API.Configuration;
using Newtonsoft.Json;

namespace Armorysticks
{
    [JsonObject(MemberSerialization.OptOut)]
    [ModConfig]
    public class ArmorysticksConfig
    {
        [ConfigField("pi")] [ConfigDefaultValue(3.14159)] public double pi;
    }
}