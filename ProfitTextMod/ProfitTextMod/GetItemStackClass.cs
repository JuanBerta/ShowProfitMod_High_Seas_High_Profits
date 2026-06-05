using zip.lexy.tgame.state;
using zip.lexy.tgame.state.ship;
using zip.lexy.tgame.ui.widget.trade;

namespace ProfitTextMod.GetItemStackClass
{
    internal static class GetItemStackClass
    {

        // Get the ItemStack
        public static ItemStack GetItemStack(CargoHolder cargoHolder, TradeWindowGood __instance)
        {
            ItemStack itemStack = cargoHolder?.GetGood(__instance.GetGood());
            return itemStack;
        }
    }
}