using UnityEngine;

namespace ProfitTextMod.GetDisplayModeClass
{
    internal static class GetDisplayModeClass
    {

        public static int GetDisplayMode()
        {
            int displayMode = PlayerPrefs.GetInt("mod.profit_text.mode", 0);
            return displayMode;
        }
    }
}