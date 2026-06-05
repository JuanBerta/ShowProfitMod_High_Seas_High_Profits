using TMPro;
using UnityEngine;

namespace ProfitTextMod.SyncUiPropertiesClass
{
    internal static class SyncUiPropertiesClass
    {

        public static void SyncUiProperties(RectTransform sourceRect, TMP_Text sourceText, TMP_Text targetText)
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
}