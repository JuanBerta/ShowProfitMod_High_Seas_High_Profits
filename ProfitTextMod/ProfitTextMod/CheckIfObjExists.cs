using MelonLoader;
using UnityEngine;

namespace ProfitTextMod.CheckIfObjExistsClass
{
    internal static class CheckIfObjExistsClass
    {
        public static void CheckIfObjExists(GameObject obj)
        {
            if (obj == null)
            {
                MelonLogger.Msg($"GameObject is null");
            }
        }
    }
}