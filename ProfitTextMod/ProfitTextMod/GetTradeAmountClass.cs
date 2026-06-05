using HarmonyLib;

namespace ProfitTextMod.GetTradeAmountClass
{
    internal static class GetTradeAmountClass
    {

        // Get the amount that will be traded
        public static int GetTradeAmount(Traverse trv)
        {
            int tradeAmt = trv.Field("tradeAmount").GetValue<int>(); // The 1, 10, 100 multiplier [cite: 1]
            return tradeAmt;
        }
    }
}