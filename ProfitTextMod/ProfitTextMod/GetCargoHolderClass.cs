using zip.lexy.tgame.state;
using zip.lexy.tgame.state.ship;
using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod.GetCargoHolderClass
{
    internal static class GetCargoHolderClass
    {

        // Get the cargo holder
        public static CargoHolder GetCargoHolder(TradeDestination destination, GameState gameState)
        {
            CargoHolder cargoHolder = destination.GetCargoHolder(gameState);
            return cargoHolder;
        }
    }
}