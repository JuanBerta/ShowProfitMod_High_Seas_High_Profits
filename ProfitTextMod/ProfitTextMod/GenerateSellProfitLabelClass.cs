using MelonLoader;
using TMPro;
using UnityEngine;
using zip.lexy.tgame.ui.widget.trade;
using ProfitTextMod.SyncUiPropertiesClass;

namespace ProfitTextMod.GenerateSellProfitLabelClass
{
    internal static class GenerateSellProfitLabelClass
    {

        // We generate the SellProfit Label
        public static void GenerateSellProfitLabel(Transform sellTransform,
            TMP_Text sellProfit,
            GameObject averagePriceObj,
            TradeWindowGood __instance,
            TMP_Text averageTextTemplate)
        {
            if (sellTransform != null)
            {
                sellProfit = sellTransform.GetComponent<TMP_Text>();
            }
            else
            {
                GameObject sellObj = GameObject.Instantiate(averagePriceObj, __instance.transform);
                sellObj.name = "sell-profit";
                sellProfit = sellObj.GetComponent<TMP_Text>();
                RectTransform buyProfitRectTransform = sellObj.GetComponent<RectTransform>();
                buyProfitRectTransform.anchoredPosition = new Vector2(buyProfitRectTransform.anchoredPosition.x, buyProfitRectTransform.anchoredPosition.y);
                buyProfitRectTransform.anchorMax = new Vector2(0.23f, buyProfitRectTransform.anchorMax.y);

                // SYNC PROPERTIES
                SyncUiPropertiesClass.SyncUiPropertiesClass.SyncUiProperties(buyProfitRectTransform, averageTextTemplate, sellProfit);

                // Apply your specific horizontal offset
                sellProfit.text = "Sell Profit";
                MelonLogger.Msg("Sell Profit Text: Created and Synced");
            }
        }
    }
}