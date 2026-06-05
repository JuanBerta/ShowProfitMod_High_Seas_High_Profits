using TMPro;
using UnityEngine;

namespace ProfitTextMod.GetTmpTextComponentClass
{
    internal static class GetTmpTextComponentClass
    {

        public static TMP_Text GetTmpTextComponent(GameObject gameObject)
        {
            TMP_Text tmpText = gameObject.GetComponent<TMP_Text>();
            return tmpText;
        }
    }
}