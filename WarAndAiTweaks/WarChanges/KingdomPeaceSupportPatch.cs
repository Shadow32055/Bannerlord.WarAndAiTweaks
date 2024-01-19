using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;

namespace WarAndAiTweaks.WarChanges {

    [HarmonyPatch(typeof(MakePeaceKingdomDecision), "DetermineSupport")]
    [HarmonyPriority(999)]
    public class KingdomPeaceSupportPatch {

        public static void Postfix(Clan clan, ref MakePeaceKingdomDecision __instance, DecisionOutcome possibleOutcome, ref float __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableDeclarePeaceChanges)
                return;

            //Check for nulls
            if (__instance == null || clan == null || possibleOutcome == null)
                return;

            try {
                StanceLink stanceWith = clan.Kingdom.GetStanceWith(__instance.FactionToMakePeaceWith);
                float elapsedDaysUntilNow = stanceWith.WarStartDate.ElapsedDaysUntilNow;
                MakePeaceKingdomDecision.MakePeaceDecisionOutcome makePeaceDecisionOutcome = (MakePeaceKingdomDecision.MakePeaceDecisionOutcome)possibleOutcome;
                if (makePeaceDecisionOutcome.ShouldPeaceBeDeclared) {
                    int factionsAtWarWith = 0;
                    foreach (IFaction faction in Campaign.Current.Factions) {
                        if (faction.IsKingdomFaction && clan.Kingdom.IsAtWarWith(faction) && faction.Settlements.Count > 0) { factionsAtWarWith++; }
                    }
                    //only apply this rule after 30 days at least
                    if (elapsedDaysUntilNow > 60) { __result += elapsedDaysUntilNow * 30; }
                    if (factionsAtWarWith > 1) { __result += factionsAtWarWith * 500; }
                }
                return;
            } catch (Exception ex) {
                return;
            }
        }
    }
}

