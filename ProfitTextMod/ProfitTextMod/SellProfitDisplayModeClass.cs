using TMPro;
using zip.lexy.tgame.state;
using zip.lexy.tgame.ui.gamegeneration;
using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod.SellProfitDisplayModeClass
{
    internal static class SellProfitDisplayModeClass
    {

        // Change Profit display text according to display mode
        public static void SellProfitDisplayMode(ItemStack itemStack,
            PriceCalculator priceCalculator,
            TradeWindowGood __instance,
            GameState gameState,
            int tradeAmt,
            int displayMode,
            TMP_Text sellProfit)
        {
            // --- SELL PROFIT LOGIC ---
            if (itemStack != null && itemStack.amount > 0.1f)
            {
                int unitSellPrice = priceCalculator.CityBuysGoods(__instance.GetGood(), gameState.GetTradeCity(), tradeAmt);
                float unitProfit = unitSellPrice - itemStack.averageCost;

                string sellDisplayText = "";
                float finalSellVal = 0;

                switch (displayMode)
                {
                    case 1: // Total
                        finalSellVal = unitProfit * tradeAmt;
                        sellDisplayText = $"{(int)finalSellVal}";
                        break;
                    case 2: // Percentage
                            // Added a check to prevent division by zero just in case
                        float avgCost = itemStack.averageCost > 0 ? itemStack.averageCost : 1;
                        finalSellVal = (unitProfit / avgCost) * 100f;
                        sellDisplayText = $"{(int)finalSellVal}%";
                        break;
                    default: // Per Unit
                        finalSellVal = unitProfit;
                        sellDisplayText = $"{(int)finalSellVal}";
                        break;
                }

                sellProfit.text = finalSellVal >= 0 ? $"<color=green>{sellDisplayText}</color>" : $"<color=red>{sellDisplayText}</color>";
            }
            else
            {
                // If we don't own the good, clear the text so it doesn't show the default label
                sellProfit.text = "";
            }
        }
    }
}