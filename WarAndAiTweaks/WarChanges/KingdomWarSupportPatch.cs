using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;

namespace WarAndAiTweaks.WarChanges {

    [HarmonyPatch(typeof(DeclareWarDecision), "DetermineSupport")]
    [HarmonyPriority(999)]
    public class KingdomWarSupportPatch {
        public static void Postfix(Clan clan, ref DeclareWarDecision __instance, DecisionOutcome possibleOutcome, ref float __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableDeclareWarChanges)
                return;

            //Check for nulls
            if (__instance == null || clan == null || possibleOutcome == null)
                return;

            try {
                StanceLink stanceWith = clan.Kingdom.GetStanceWith(__instance.FactionToDeclareWarOn);
                DeclareWarDecision.DeclareWarDecisionOutcome declareWarDecisionOutcome = (DeclareWarDecision.DeclareWarDecisionOutcome)possibleOutcome;
                if (declareWarDecisionOutcome.ShouldWarBeDeclared) {
                    int factionsAtWarWith = 0;
                    foreach (IFaction faction in Campaign.Current.Factions.Where(x => x.IsKingdomFaction && clan.Kingdom.IsAtWarWith(x) && x.Settlements.Count > 0)) {
                        factionsAtWarWith++;
                    }
                    //if the faction is at with 0 faction, find the last peace date
                    if (factionsAtWarWith == 0) {
                        //Faction is now at peace with everyone, lets find out the least amount of days theyve been at peace	
                        List<float> daysSinceLastPeaceList = new List<float>();
                        foreach (IFaction faction in Campaign.Current.Factions.Where(x => x.Settlements.Count > 0)) {
                            StanceLink currentStance = clan.Kingdom.GetStanceWith(faction);
                            daysSinceLastPeaceList.Add(currentStance.PeaceDeclarationDate.ElapsedDaysUntilNow);
                        }
                        float daysSinceLastPeace = daysSinceLastPeaceList.Min();
                        float scoreDefecitStart = float.MaxValue;
                        float scoreIncreaseIncrement = scoreDefecitStart / WarAndAiTweaks.Settings.targetPeaceTime;
                        if ((scoreDefecitStart - (scoreIncreaseIncrement * daysSinceLastPeace)) <= scoreDefecitStart && (scoreDefecitStart - (scoreIncreaseIncrement * daysSinceLastPeace)) > 0f) { __result -= (scoreDefecitStart - (scoreIncreaseIncrement * daysSinceLastPeace)); }
                    }
                    if (factionsAtWarWith > 0) {
                        StanceLink currentStance = clan.Kingdom.GetStanceWith(__instance.FactionToDeclareWarOn);
                        float daysSinceLastPeace = currentStance.PeaceDeclarationDate.ElapsedDaysUntilNow;
                        float scoreDefecitStart = float.MaxValue;
                        float scoreIncreaseIncrement = scoreDefecitStart / WarAndAiTweaks.Settings.targetPeaceTime;
                        if ((scoreDefecitStart - (scoreIncreaseIncrement * daysSinceLastPeace)) <= scoreDefecitStart && (scoreDefecitStart - (scoreIncreaseIncrement * daysSinceLastPeace)) > 0f) { __result -= (scoreDefecitStart - (scoreIncreaseIncrement * daysSinceLastPeace)); }
                    }
                    __result -= factionsAtWarWith * 10000;
                    return;
                }
            } catch (Exception ex) {
                return;
            }
        }
    }
}

