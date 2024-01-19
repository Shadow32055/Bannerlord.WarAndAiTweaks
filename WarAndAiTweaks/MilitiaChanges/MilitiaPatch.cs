using HarmonyLib;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace WarAndAiTweaks.MilitiaChanges {

    [HarmonyPatch(typeof(Building), "GetBuildingEffectAmount")]
    public class MilitiaPatch {

        private static void Postfix(Building __instance, BuildingEffectEnum effect, ref float __result) {

            if (WarAndAiTweaks.Settings.EnableMiltiaChange) {
                bool flag2 = effect == BuildingEffectEnum.Militia && __instance.Name.ToString() == "Militia Grounds";
                if (flag2) {
                    bool isCastle = __instance.Town.IsCastle;
                    if (isCastle) {
                        __result += WarAndAiTweaks.Settings.CastleMiltiaBoost;
                    }
                    bool isTown = __instance.Town.IsTown;
                    if (isTown) {
                        __result += WarAndAiTweaks.Settings.TownMiltiaBoost;
                    }
                }
            }
        }
    }
}
