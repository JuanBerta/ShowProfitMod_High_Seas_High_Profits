using MelonLoader;
using zip.lexy.tgame.state;
using zip.lexy.tgame.ui.gamegeneration;

namespace ProfitTextMod.IfComponentsExistClass
{
    internal static class IfComponentsExistClass
    {

        public static void IfComponentsExist(GameState gameState, PriceCalculator priceCalculator)
        {
            if (gameState == null || priceCalculator == null)
            {
                // Send a log message if we fail to get the necessary components, but don't spam if they are just missing
                MelonLogger.Msg("Failed to access gameState or priceCalculator. Profit text will not be shown.");
                return;
            }
        }
    }
}