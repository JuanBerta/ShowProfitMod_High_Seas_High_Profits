using TMPro;
using UnityEngine;

namespace ProfitTextMod.FindTmpTextClass
{
    internal static class FindTmpTextClass
    {

        // Get TMP_Text component
        public static TMP_Text FindTmpText(string objName)
        {
            TMP_Text textComponent = GameObject.Find(objName).GetComponent<TMP_Text>();
            return textComponent;
        }
    }
}