using HarmonyLib;
using zip.lexy.tgame.ui.gamegeneration;

namespace ProfitTextMod.GetPriceCalculatorClass
{
    internal static class GetPriceCalculatorClass
    {

        public static PriceCalculator GetPriceCalculator(Traverse trv)
        {
            var priceCalculator = trv.Property("priceCalculator").GetValue<PriceCalculator>();
            return priceCalculator;
        }
    }
}