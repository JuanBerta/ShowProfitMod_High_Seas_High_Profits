using HarmonyLib;
using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod.CreateTraverseClass
{
    internal static class CreateTraverseClass
    {

        public static Traverse CreateTraverse(TradeWindowGood tradeWindowGood)
        {
            var trv = Traverse.Create(tradeWindowGood);
            return trv;
        }
    }
}