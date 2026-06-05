using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod.GetGoodIdClass
{
    internal static class GetGoodIdClass
    {

        // Get the goodId (String)
        public static string GetGoodId(TradeWindowGood tradeWindowGood)
        {
            string goodId = tradeWindowGood.GetGood();
            return goodId;
        }
    }
}