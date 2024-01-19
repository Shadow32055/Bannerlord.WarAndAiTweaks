using HarmonyLib;
using TaleWorlds.CampaignSystem.Party;

namespace WarAndAiTweaks.Patches
{
    //Feature integration with fillstacks
    [HarmonyPatch(typeof(MobileParty), "FillPartyStacks")]
    public class SpawnLordPartyInternalPatch
    {

        public static void Prefix(PartyTemplateObject pt, ref int troopNumberLimit, ref MobileParty __instance)
        {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableFillStacks) { return; }


            if (__instance.LeaderHero != null && __instance.LeaderHero.Clan != null) { return; }
            if (!__instance.IsLordParty) { return; }
            if (__instance.LeaderHero.Clan.IsUnderMercenaryService && !WarAndAiTweaks.Settings.FillStackMercenaries) { return; }

            //Start FillStack code if enabled
            if (pt.GetName().ToString().StartsWith("kingdom_hero_party_vlandia") ||
                pt.GetName().ToString().StartsWith("kingdom_hero_party_battania") ||
                pt.GetName().ToString().StartsWith("kingdom_hero_party_khuzait") ||
                pt.GetName().ToString().StartsWith("kingdom_hero_party_empire") ||
                pt.GetName().ToString().StartsWith("kingdom_hero_party_sturgia") ||
                pt.GetName().ToString().StartsWith("kingdom_hero_party_aserai"))
            {
                troopNumberLimit = WarAndAiTweaks.Settings.FillStackTroopCount;
                //InformationManager.DisplayMessage(new InformationMessage(pt.GetName() + "; troop count: " + troopNumberLimit));
            }
        }
    }
}
