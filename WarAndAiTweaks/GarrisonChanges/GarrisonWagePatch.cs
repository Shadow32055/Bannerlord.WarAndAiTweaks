using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace WarAndAiTweaks.Militia_And_Garrison_Changes {
    [HarmonyPatch(typeof(DefaultClanFinanceModel), "CalculatePartyWage")]
    public class GarrisonWagePatch {
        private static void Postfix(MobileParty mobileParty, ref int __result) {

            if (WarAndAiTweaks.Settings.EnableGarrisonCostReduction && mobileParty.IsGarrison) {
                __result = (int)(__result * WarAndAiTweaks.Settings.GarrisonCostMultiply);
            }
        }
    }
}
    

