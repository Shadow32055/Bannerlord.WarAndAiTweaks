using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;

namespace WarAndAiTweaks.WarChanges {
    [HarmonyPatch(typeof(KingdomElection), "DetermineInitialSupport")]
    [HarmonyPriority(999)]
    public class KingdomElectionPatch {

        public static bool Prefix(ref float __result, DecisionOutcome possibleOutcome, ref KingdomElection __instance, KingdomDecision ____decision, List<Supporter> ____supporters) {

            if (WarAndAiTweaks.Settings.EnableDeclarePeaceChanges) {
                try {
                    MakePeaceKingdomDecision.MakePeaceDecisionOutcome makePeaceDecisionOutcome = (MakePeaceKingdomDecision.MakePeaceDecisionOutcome)possibleOutcome;
                } catch (Exception ex) {
                    return true;
                }
                bool shouldPeaceBeDeclared = ((MakePeaceKingdomDecision.MakePeaceDecisionOutcome)possibleOutcome).ShouldPeaceBeDeclared;
                if (shouldPeaceBeDeclared) {
                    float num = 0f;
                    foreach (Supporter supporter in from x in ____supporters
                                                    where !x.IsPlayer
                                                    select x) {
                        num += ____decision.DetermineSupport(supporter.Clan, possibleOutcome);
                    }
                    __result = num;
                    return false;
                }
            }

            if (WarAndAiTweaks.Settings.EnableDeclareWarChanges) {
                try {
                    DeclareWarDecision.DeclareWarDecisionOutcome declareWarDecisionOutcome = (DeclareWarDecision.DeclareWarDecisionOutcome)possibleOutcome;
                } catch (Exception ex2) {
                    return true;
                }
                bool shouldWarBeDeclared = ((DeclareWarDecision.DeclareWarDecisionOutcome)possibleOutcome).ShouldWarBeDeclared;
                if (shouldWarBeDeclared) {
                    float num2 = 0f;
                    foreach (Supporter supporter2 in from x in ____supporters
                                                     where !x.IsPlayer
                                                     select x) {
                        num2 += ____decision.DetermineSupport(supporter2.Clan, possibleOutcome);
                    }
                    __result = num2;
                    return false;
                }
            }
            return true;
        }
    }
}
