using UnityEngine;
using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod.FindTransformClass
{
    internal static class FindTransformClass
    {

        // Get the Transform
        public static Transform FindTransform(TradeWindowGood __instance, string objName)
        {
            Transform transform = __instance.transform.Find(objName);
            return transform;
        }
    }
}