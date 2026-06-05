using UnityEngine;

namespace ProfitTextMod.GetRectTransformClass
{
    internal static class GetRectTransformClass
    {

        public static RectTransform GetRectTransform(GameObject gameObject)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            return rectTransform;
        }
    }
}