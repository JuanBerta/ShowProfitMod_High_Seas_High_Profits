using UnityEngine;

namespace ProfitTextMod.ModifyGoodsContainerClass
{
    internal static class ModifyGoodsContainerClass
    {

        public static void ModifyGoodsContainer()
        {
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
                        }
                    }
                }
            }
        }
    }
}