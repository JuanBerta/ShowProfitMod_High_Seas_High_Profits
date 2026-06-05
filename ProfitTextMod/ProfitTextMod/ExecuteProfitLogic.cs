using HarmonyLib;
using MelonLoader;
using Mono.Security.Authenticode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using zip.lexy.tgame.constants;
using zip.lexy.tgame.saves;
using zip.lexy.tgame.state;
using zip.lexy.tgame.state.ship;
using zip.lexy.tgame.ui.gamegeneration;
using zip.lexy.tgame.ui.widget.trade;
using static MelonLoader.MelonLogger;

namespace ProfitTextMod.ExecuteProfitLogicClass
{
    internal class ExecuteProfitLogicClass
    {
        public static void ExecuteProfitLogic(TradeWindowGood __instance)
        {
            int displayMode = ProfitTextMod.GetDisplayModeClass.GetDisplayModeClass.GetDisplayMode();

            var trv = Traverse.Create(__instance);
            var gameState = GetGameStateClass.GetGameStateClass.GetGameState(trv);
            var priceCalculator = GetPriceCalculatorClass.GetPriceCalculatorClass.GetPriceCalculator(trv);
            var priceCalculatorTraverse = GetPriceCalculatorTraverseClass.GetPriceCalculatorTraverseClass.GetPriceCalculatorTraverse(priceCalculator);
            var destination = GetDestinationClass.GetDestinationClass.GetDestination(trv);

            string averagePriceObjPath = "ui/trade-window/window/trade/goods/ale/avg-price";
            string buyProfitObjName = "buy-profit";
            string sellProfitObName = "sell-profit";
            IfComponentsExistClass.IfComponentsExistClass.IfComponentsExist(gameState, priceCalculator);

            // --- UI SETUP ---
            GameObject averagePriceObj = FindGameObjectClass.FindGameObjectClass.FindGameObject(averagePriceObjPath);
            if (averagePriceObj == null) return;

            // Get the templates once to avoid repeated calls
            TMP_Text avgTextTemplate = GetTmpTextComponentClass.GetTmpTextComponentClass.GetTmpTextComponent(averagePriceObj);
            RectTransform avgRectTemplate = GetRectTransformClass.GetRectTransformClass.GetRectTransform(averagePriceObj);

            // 1. Handle SELL PROFIT
            TMP_Text sellProfit = FindTmpTextClass.FindTmpTextClass.FindTmpText(sellProfitObName);
            Transform sellTransform = FindTransformClass.FindTransformClass.FindTransform(__instance, sellProfitObName);
            GenerateSellProfitLabelClass.
                        GenerateSellProfitLabelClass.GenerateSellProfitLabel(sellTransform, sellProfit, averagePriceObj, __instance, avgTextTemplate);

            // --- GAMEOBJECTS ---
            TMP_Text buyProfit = FindTmpTextClass.FindTmpTextClass.FindTmpText("buy-profit");
            Transform buyTransform = FindTransformClass.FindTransformClass.FindTransform(__instance, buyProfitObjName);
            GenerateBuyProfitLabelClass.GenerateBuyProfitLabelClass.

            // --- GENERATE LABELS FOR BUY PROFIT --- 
            GenerateBuyProfitLabel(__instance, averagePriceObj, avgTextTemplate, buyProfit, buyTransform);
            AreTextLabelsNullClass.AreTextLabelsNullClass.

            // We check if text labels are null so we skip the process if they don't exist
            AreTextLabelsNull(sellProfit, buyProfit);

            // --- DATA FETCHING ---
            string goodId = GetGoodIdClass.GetGoodIdClass.GetGoodId(__instance);
            CargoHolder cargoHolder = GetCargoHolderClass.GetCargoHolderClass.GetCargoHolder(destination, gameState); // Get the cargo holder ship
            ItemStack itemStack = GetItemStackClass.GetItemStackClass.GetItemStack(cargoHolder, __instance); // Get ItemStack to know the  data
            int tradeAmt = GetTradeAmountClass.GetTradeAmountClass.GetTradeAmount(trv); // The 1, 10, 100 multiplier [cite: 1]
            SellProfitDisplayModeClass.SellProfitDisplayModeClass.

            // --- SELL PROFIT LOGIC ---
            SellProfitDisplayMode(itemStack, priceCalculator, __instance, gameState, tradeAmt, displayMode, sellProfit);

            // --- BUY PROFIT DISPLAY LOGIC ---
            BuyProfitDisplayModeClass.BuyProfitDisplayModeClass.BuyProfitDisplayMode(gameState, goodId, priceCalculator, tradeAmt, displayMode, buyProfit);
            ModifyGoodsContainerClass.ModifyGoodsContainerClass.

            // 1. Find the parent container
            ModifyGoodsContainer();
        }
    }
}
