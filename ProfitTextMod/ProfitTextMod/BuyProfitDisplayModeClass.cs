using TMPro;
using zip.lexy.tgame.state;
using zip.lexy.tgame.ui.gamegeneration;

namespace ProfitTextMod.BuyProfitDisplayModeClass
{
    internal static class BuyProfitDisplayModeClass
    {

        // Changes Display logic according to mod settings
        public static void BuyProfitDisplayMode(GameState gameState,
            string goodId,
            PriceCalculator priceCalculator,
            int tradeAmt,
            int displayMode,
            TMP_Text buyProfit)
        {
            // --- BUY PROFIT LOGIC ---
            if (gameState != null && gameState.corePrices.TryGetValue(goodId, out float basePrice))
            {
                int unitCityPrice = priceCalculator.CitySellsGoods(goodId, gameState.GetTradeCity(), tradeAmt);
                float unitSavings = basePrice - unitCityPrice;

                string buyDisplayText = "";
                float finalBuyVal = 0;

                switch (displayMode)
                {
                    case 1: // Total
                        finalBuyVal = unitSavings * tradeAmt;
                        buyDisplayText = $"{(int)finalBuyVal}";
                        break;
                    case 2: // Percentage
                        finalBuyVal = (unitSavings / basePrice) * 100f;
                        buyDisplayText = $"{(int)finalBuyVal}%";
                        break;
                    default: // Per Unit
                        finalBuyVal = unitSavings;
                        buyDisplayText = $"{(int)finalBuyVal}";
                        break;
                }

                buyProfit.text = finalBuyVal >= 0 ? $"<color=green>{buyDisplayText}</color>" : $"<color=red>{buyDisplayText}</color>";
            }
        }
    }
}