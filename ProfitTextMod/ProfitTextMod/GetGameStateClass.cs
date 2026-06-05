using HarmonyLib;
using zip.lexy.tgame.state;

namespace ProfitTextMod.GetGameStateClass
{
    internal static class GetGameStateClass
    {

        public static GameState GetGameState(Traverse trv)
        {
            var gameState = trv.Property("gameState").GetValue<GameState>();
            return gameState;
        }
    }
}