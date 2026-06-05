using TMPro;

namespace ProfitTextMod.AreTextLabelsNullClass
{
    internal static class AreTextLabelsNullClass
    {

        // Check if text labels are null, so we don't continue if they don't exist
        public static void AreTextLabelsNull(TMP_Text sellProfit, TMP_Text buyProfit)
        {
            if (sellProfit == null || buyProfit == null) return;
        }
    }
}