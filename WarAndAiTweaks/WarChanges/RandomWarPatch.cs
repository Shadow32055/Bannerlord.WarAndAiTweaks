using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Localization;

namespace WarAndAiTweaks.WarChanges {

    [HarmonyPatch(typeof(KingdomDecisionProposalBehavior), "GetRandomWarDecision")]
    [HarmonyPriority(999)]
    public class RandomWarPatch {

        public static void Postfix(Clan clan, ref KingdomDecision __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableDeclareWarChanges)
                return;

            __result = null;

            if (clan.Kingdom.RulingClan == Clan.PlayerClan || clan != clan.Kingdom.RulingClan)
                return;

            if (clan.Kingdom.UnresolvedDecisions.FirstOrDefault((KingdomDecision x) => x is DeclareWarDecision) != null) 
                return;

            IFaction clanMapFaction = clan.Kingdom.MapFaction;
            IFaction BestFaction = null;
            BestFaction = Campaign.Current.Factions.Where(x => (x.IsRebelClan || x.IsKingdomFaction) && x.Settlements.Count > 0 && x != clanMapFaction).Aggregate((x, y) => getScoreOfDeclaringWar(clanMapFaction, x, clan) > getScoreOfDeclaringWar(clanMapFaction, y, clan) ? x : y);
            
            if (BestFaction == null) 
                return;

            //InformationManager.DisplayMessage(new InformationMessage(clan.Kingdom.ToString() + " is considering war with: " + BestFaction.Name.ToString() + " with a distance deficit of: " + (distance*1000f).ToString() + " and final score of " + getScoreOfDeclaringWar(clanMapFaction, BestFaction, clan).ToString()));
            Object[] parametersArray = new object[] { clan, clan.Kingdom, BestFaction };
            bool ConsiderWarResult = (bool)typeof(KingdomDecisionProposalBehavior).GetMethod("ConsiderWar", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new KingdomDecisionProposalBehavior(), parametersArray);
            if (ConsiderWarResult) {
                __result = new DeclareWarDecision(clan, BestFaction);
            } else {
                __result = null;
            }
            return;
        }

        public static float getScoreOfDeclaringWar(IFaction factionConsidering, IFaction targetFaction, Clan evaluatingClan) {
            TextObject textObject = null;
            float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(factionConsidering.FactionMidSettlement, targetFaction.FactionMidSettlement);
            return Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(factionConsidering, targetFaction, evaluatingClan, out textObject) - distance * 1000f;
        }
    }
}


