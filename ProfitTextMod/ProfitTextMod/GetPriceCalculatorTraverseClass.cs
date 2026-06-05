using HarmonyLib;
using zip.lexy.tgame.ui.gamegeneration;

namespace ProfitTextMod.GetPriceCalculatorTraverseClass
{
    internal static class GetPriceCalculatorTraverseClass
    {

        public static Traverse GetPriceCalculatorTraverse(PriceCalculator priceCalculator)
        {
            var priceCalculatorTraverse = Traverse.Create(priceCalculator);
            return priceCalculatorTraverse;
        }
    }
}