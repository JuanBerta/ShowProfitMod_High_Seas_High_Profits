using HarmonyLib;
using MelonLoader;
using Mono.Security.Authenticode;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using zip.lexy.tgame.constants;
using zip.lexy.tgame.state;
using zip.lexy.tgame.ui.gamegeneration;
using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod
{
    public class ProfitTextModClass : MelonMod
    {
        [HarmonyPatch(typeof(TradeWindowGood), nameof(TradeWindowGood.UpdatePriceInUI))]
        public static class Patch_UpdatePriceInUI
        {
            public static void Postfix(TradeWindowGood __instance)
            {
                var trv = Traverse.Create(__instance);
                var gameState = trv.Property("gameState").GetValue<GameState>();
                var priceCalculator = trv.Property("priceCalculator").GetValue<PriceCalculator>();
                var priceCalculatorTraverse = Traverse.Create(priceCalculator);
                var destination = trv.Field("destination").GetValue<TradeDestination>();

                if (gameState == null || priceCalculator == null)
                {
                    // Send a log message if we fail to get the necessary components, but don't spam if they are just missing
                    MelonLogger.Msg("Failed to access gameState or priceCalculator. Profit text will not be shown.");
                    return;
                }

                // --- UI SETUP ---
                GameObject averagePriceObj = GameObject.Find("ui/trade-window/window/trade/goods/ale/avg-price");
                if (averagePriceObj == null) return;

                // Get the templates once to avoid repeated calls
                TMP_Text avgTextTemplate = averagePriceObj.GetComponent<TMP_Text>();
                RectTransform avgRectTemplate = averagePriceObj.GetComponent<RectTransform>();

                // 1. Handle SELL PROFIT
                TMP_Text sellProfit;
                Transform sellTransform = __instance.transform.Find("sell-profit");

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
                    SyncUiProperties(buyProfitRectTransform, avgTextTemplate, sellProfit);

                    // Apply your specific horizontal offset
                    sellProfit.text = "Sell Profit";
                    MelonLogger.Msg("Sell Profit Text: Created and Synced");
                }

                // 2. Handle BUY PROFIT
                TMP_Text buyProfit;
                Transform buyTransform = __instance.transform.Find("buy-profit");

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
                    SyncUiProperties(buyProfitRectTransform, avgTextTemplate, buyProfit);

                    // Apply your specific horizontal offset (e.g., to the left of the other columns)
                    buyProfit.text = "Buy Profit";
                    MelonLogger.Msg("Buy Profit Text: Created and Synced");
                }

                string goodId = __instance.GetGood();

                if (sellProfit == null || buyProfit == null) return;

                // --- DATA FETCHING ---
                var cargoHolder = destination.GetCargoHolder(gameState);
                MelonLogger.Msg($"CargoHolder: {(cargoHolder != null ? "Found" : "Not found")}");
                ItemStack itemStack = cargoHolder?.GetGood(__instance.GetGood());
                MelonLogger.Msg($"ItemStack: {(itemStack != null ? $"Found (amount: {itemStack.amount}, avgCost: {itemStack.averageCost})" : "Not found")}");
                int tradeAmt = trv.Field("tradeAmount").GetValue<int>(); // The 1, 10, 100 multiplier [cite: 1]
                MelonLogger.Msg($"Trade Amount: {tradeAmt}");

                // --- SELL PROFIT LOGIC ---
                if (itemStack != null && itemStack.amount > 0.1f)
                {
                    // Use 1 as the count to get the "Per Unit" price the city offers
                    int unitSellPrice = priceCalculator.CityBuysGoods(__instance.GetGood(), gameState.GetTradeCity(), tradeAmt);

                    // Profit = (Unit Sale Price) - (Your Unit Average Cost)
                    float unitProfit = unitSellPrice - itemStack.averageCost;

                    // If you want to see total profit for the whole slider amount:
                    // float totalProfit = unitProfit * tradeAmt;

                    sellProfit.text = unitProfit >= 0 ? $"<color=green>{(int)unitProfit}</color>" : $"<color=red>{(int)unitProfit}</color>";

                    MelonLogger.Msg($"Unit Sell Price: {unitSellPrice}, Avg Cost: {itemStack.averageCost}, Unit Profit: {unitProfit}");
                }
                else
                {
                    sellProfit.text = "";
                    MelonLogger.Msg("No goods to sell, skipping sell profit calculation.");
                }

                // Buy DEAL logic, if the gameState is available and has the corePrices dictionary (which it should, but better safe than crashy)
                // Then we can compare the city's price to the global base price to determine if it's a good deal or not
                if (gameState != null && gameState.corePrices.TryGetValue(goodId, out float basePrice))
                {
                    // Get what the city charges for exactly 1 unit
                    int unitCityPrice = priceCalculator.CitySellsGoods(goodId, gameState.GetTradeCity(), tradeAmt);

                    // Savings = (Standard Global Price) - (Current City Price)
                    // Positive means it's a bargain (Cheap), Negative means it's overpriced
                    float savings = basePrice - unitCityPrice;

                    buyProfit.text = savings >= 0 ? $"<color=green>{(int)savings}</color>" : $"<color=red>{(int)savings}</color>";
                }

                // 1. Find the parent container
                GameObject goodsContainer = GameObject.Find("ui/trade-window/window/trade/goods");

                if (goodsContainer != null)
                {
                    // 2. Iterate through every good row (Mead, Ale, etc.)
                    foreach (Transform goodRow in goodsContainer.transform)
                    {
                        // 3. Find the specific child by name
                        Transform selectionBg = goodRow.GetChild(1);

                        if (selectionBg != null)
                        {
                            // 4. Get the RectTransform to modify anchors
                            RectTransform rect = selectionBg.GetComponent<RectTransform>();

                            if (rect != null)
                            {
                                // Set the Anchor Max as requested
                                rect.anchorMax = new Vector2(1.24f, rect.anchorMax.y);

                                // QA Note: If the background looks offset, you may also need 
                                // to set the sizeDelta or anchoredPosition to 0 to "snap" it to the new anchor.
                                // rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);
                            }
                        }
                    }
                }
            }

            private static void SyncUiProperties(RectTransform sourceRect, TMP_Text sourceText, TMP_Text targetText)
            {
                RectTransform targetRect = targetText.rectTransform;

                // Sync RectTransform (Layout)
                targetRect.anchorMin = sourceRect.anchorMin;
                targetRect.anchorMax = sourceRect.anchorMax;
                targetRect.pivot = sourceRect.pivot;
                targetRect.sizeDelta = sourceRect.sizeDelta;
                targetRect.anchoredPosition = sourceRect.anchoredPosition;

                // Sync TextMeshPro (Visuals)
                targetText.font = sourceText.font;
                targetText.fontSize = sourceText.fontSize;
                targetText.alignment = sourceText.alignment;
                targetText.color = sourceText.color;
                // Set raycastTarget to false to prevent the UI from "blocking" mouse clicks
                targetText.raycastTarget = false;
            }
        }


        [HarmonyPatch(typeof(TradeWindow), "Start")]
        public static class Patch_TradeWindow_Start
        {
            public static void Postfix(TradeWindow __instance)
            {
                // Declare uiObj, scale it with screen size
                GameObject uiObj = GameObject.Find("ui");
                CheckIfObjExists(uiObj);
                CanvasScaler uiCanvasScaler = uiObj.GetComponent<CanvasScaler>();
                uiCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                uiCanvasScaler.referenceResolution = new Vector2(1920, 1080);
                uiCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                uiCanvasScaler.matchWidthOrHeight = 0.5f;

                // Declare goldborder and get it's RectTransform
                GameObject goldBorder = GameObject.Find("ui/trade-window/window/trade/gold-border");
                CheckIfObjExists(goldBorder);
                RectTransform goldBorderRectTransform = goldBorder.GetComponent<RectTransform>();
                goldBorderRectTransform.anchorMax = new Vector2(1.234f, goldBorderRectTransform.anchorMax.y);

                // Find window GameObject and set it up
                GameObject window = GameObject.Find("ui/trade-window/window");
                CheckIfObjExists(window);
                RectTransform windowRectTransform = window.GetComponent<RectTransform>();

                // Find up shipCargo and set it up
                GameObject shipCargo = GameObject.Find("ui/trade-window/window/ship-cargo");
                CheckIfObjExists(shipCargo);
                RectTransform shipCargoRectTransform = shipCargo.GetComponent<RectTransform>();
                shipCargoRectTransform.offsetMax = new Vector2(148f, shipCargoRectTransform.offsetMax.y);

                // Find view and set it up
                GameObject view = GameObject.Find("ui/trade-window/window/view");
                CheckIfObjExists(view);
                RectTransform viewRectTransform = view.GetComponent<RectTransform>();
                viewRectTransform.anchoredPosition = new Vector2(80f, viewRectTransform.anchoredPosition.y);

                // Set up windowBase
                GameObject windowBase = GameObject.Find("ui/trade-window/window/window-base");
                CheckIfObjExists(windowBase);
                RectTransform windowBaseRectTransform = windowBase.GetComponent<RectTransform>();
                windowBaseRectTransform.anchorMax = new Vector2(1.2f, windowBaseRectTransform.anchorMax.y);

                // Set up trade
                GameObject trade = GameObject.Find("ui/trade-window/window/trade");
                CheckIfObjExists(trade);
                RectTransform tradeRectTransform = trade.GetComponent<RectTransform>();

                // Find windowRight GameObject and set it up
                GameObject windowRight = GameObject.Find("ui/trade-window/window/trade/window-right");
                CheckIfObjExists(windowRight);
                RectTransform windowRightRectTransform = windowRight.GetComponent<RectTransform>();

                // Set up goods
                GameObject goods = GameObject.Find("ui/trade-window/window/trade/goods");
                CheckIfObjExists(goods);
                RectTransform goodsRectTransform = goods.GetComponent<RectTransform>();

                // Set up tradeToShipHeaderColors
                GameObject tradeToShipHeaderColors = GameObject.Find("ui/trade-window/window/trade/window-right/trade-to-ship-header-colors");
                CheckIfObjExists(tradeToShipHeaderColors);
                RectTransform tradeToShipHeaderColorsRectTransform = tradeToShipHeaderColors.GetComponent<RectTransform>();
                tradeToShipHeaderColorsRectTransform.anchorMax = new Vector2(1.78f, tradeToShipHeaderColorsRectTransform.anchorMax.y);

                // Set up tradeToWarehouseColors
                GameObject tradeToWarehouseColors = GameObject.Find("ui/trade-window/window/trade/window-right/trade-to-warehouse-colors");
                CheckIfObjExists(tradeToWarehouseColors);
                RectTransform tradeToWarehouseColorsRectTransform = tradeToWarehouseColors.GetComponent<RectTransform>();
                tradeToWarehouseColorsRectTransform.anchorMax = new Vector2(1.78f, tradeToWarehouseColorsRectTransform.anchorMax.y);

                // Set up separatorTopSell
                GameObject separatorTopSell = GameObject.Find("ui/trade-window/window/trade/window-right/separator-top-sell");
                CheckIfObjExists(separatorTopSell);
                RectTransform separatorTopSellRectTransform = separatorTopSell.GetComponent<RectTransform>();
                separatorTopSellRectTransform.anchorMax = new Vector2(1.78f, separatorTopSellRectTransform.anchorMax.y);

                // Set up separatorBottomSell
                GameObject separatorBottomSell = GameObject.Find("ui/trade-window/window/trade/window-right/separator-bottom-sell");
                CheckIfObjExists(separatorBottomSell);
                RectTransform separatorBottomSellRectTransform = separatorBottomSell.GetComponent<RectTransform>();
                separatorBottomSellRectTransform.anchorMax = new Vector2(1.78f, separatorBottomSellRectTransform.anchorMax.y);

                // Find destinationObj and make it children of destinationLayout
                GameObject destinationObj = GameObject.Find("ui/trade-window/window/trade/window-right/destination");
                CheckIfObjExists(destinationObj);
                RectTransform destinationObjRectTransform = destinationObj.GetComponent<RectTransform>();
                destinationObjRectTransform.anchorMax = new Vector2(1.75f, destinationObjRectTransform.anchorMax.y);

                // Find averageObj GameObject
                GameObject averageObj = GameObject.Find("ui/trade-window/window/trade/window-right/average");
                CheckIfObjExists(averageObj);

                // Create a new GameObject for the sell profit text, using the averageObj as a template to ensure consistent styling
                GameObject buyProfitLabel = new GameObject("buy-profit-label", typeof(RectTransform));
                CheckIfObjExists(buyProfitLabel);
                buyProfitLabel.transform.parent = windowRight.transform;
                TextMeshProUGUI buyProfitText = buyProfitLabel.AddComponent<TextMeshProUGUI>();
                RectTransform buyProfitRectTransform = buyProfitLabel.GetComponent<RectTransform>();

                // buyProfitText text configuration
                buyProfitText.text = averageObj.GetComponent<TextMeshProUGUI>().text;
                buyProfitText.text = "Buy Profit";
                buyProfitText.fontSize = 16f;
                buyProfitText.fontSizeMax = 72f;
                buyProfitText.fontSizeMin = 18f;

                buyProfitRectTransform.offsetMax = new Vector2(-4f, -29f);
                buyProfitRectTransform.offsetMin = new Vector2(7f, -50f);
                buyProfitRectTransform.anchorMax = new Vector2(1f, 1f);
                buyProfitRectTransform.anchorMin = new Vector2(1f, 1f);

                // Create a new GameObject for the sell profit text, using the averageObj as a template to ensure consistent styling
                GameObject sellProfitLabel = new GameObject("sell-profit-label", typeof(RectTransform));
                CheckIfObjExists(sellProfitLabel);
                sellProfitLabel.transform.parent = windowRight.transform;
                TextMeshProUGUI sellProfitText = sellProfitLabel.AddComponent<TextMeshProUGUI>();
                RectTransform sellProfitRectTransform = sellProfitLabel.GetComponent<RectTransform>();

                // sellProfitText text configuration
                sellProfitText.text = averageObj.GetComponent<TextMeshProUGUI>().text;
                sellProfitText.text = "Sell Profit";
                sellProfitText.fontSize = 16f;
                sellProfitText.fontSizeMax = 72f;
                sellProfitText.fontSizeMin = 18f;

                sellProfitRectTransform.offsetMax = new Vector2(-4f, -29f);
                sellProfitRectTransform.offsetMin = new Vector2(90f, -50f);
                sellProfitRectTransform.anchorMax = new Vector2(1f, 1f);
                sellProfitRectTransform.anchorMin = new Vector2(1f, 1f);

                Type tradeWindow = typeof(TradeWindow);
                FieldInfo goodsInfo = tradeWindow.GetField("goods", BindingFlags.NonPublic | BindingFlags.Instance);
                }
            }
            private static void CheckIfObjExists(GameObject obj)
            {
                if (obj == null)
                {
                    MelonLogger.Msg($"GameObject is null");
                }
            }
        }
    }