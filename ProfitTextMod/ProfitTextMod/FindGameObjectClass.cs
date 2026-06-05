using UnityEngine;

namespace ProfitTextMod.FindGameObjectClass
{
    internal static class FindGameObjectClass
    {

        public static GameObject FindGameObject(string objName)
        {
            GameObject gameObject = GameObject.Find(objName);
            return gameObject;
        }
    }
}