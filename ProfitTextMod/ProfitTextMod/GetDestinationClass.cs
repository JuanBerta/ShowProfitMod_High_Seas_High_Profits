using HarmonyLib;
using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod.GetDestinationClass
{
    internal static class GetDestinationClass
    {

        public static TradeDestination GetDestination(Traverse trv)
        {
            var destination = trv.Field("destination").GetValue<TradeDestination>();
            return destination;
        }
    }
}