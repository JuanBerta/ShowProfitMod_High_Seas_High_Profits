using MelonLoader;
using TMPro;
using UnityEngine;
using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod.GenerateBuyProfitLabelClass
{
    internal static class GenerateBuyProfitLabelClass
    {

        // Generate the BuyProfit Label
        public static void GenerateBuyProfitLabel(TradeWindowGood __instance,
            GameObject averagePriceObj,
            TMP_Text averageTextTemplate,
            TMP_Text buyProfit,
            Transform buyTransform)
        {
            if (buyTransform != null)
            {
                buyProfit = buyTransform.GetComponent<TMP_Text>();
            }
            else
            {
                GameObject buyObj = GameObject.Instantiate(averagePriceObj, __instance.transform);
                buyObj.name = "buy-profit";
                buyProfit = buyObj.GetComponent<TMP_Text>();
                RectTransform buyProfitRectTransform = buyObj.GetComponent<RectTransform>();
                buyProfitRectTransform.anchoredPosition = new Vector2(buyProfitRectTransform.anchoredPosition.x, buyProfitRectTransform.anchoredPosition.y);
                buyProfitRectTransform.anchorMax = new Vector2(0.12f, buyProfitRectTransform.anchorMax.y);

                // SYNC PROPERTIES
                SyncUiPropertiesClass.SyncUiPropertiesClass.SyncUiProperties(buyProfitRectTransform, averageTextTemplate, buyProfit);

                // Apply your specific horizontal offset (e.g., to the left of the other columns)
                buyProfit.text = "Buy Profit";
                MelonLogger.Msg("Buy Profit Text: Created and Synced");
            }
        }
    }
}