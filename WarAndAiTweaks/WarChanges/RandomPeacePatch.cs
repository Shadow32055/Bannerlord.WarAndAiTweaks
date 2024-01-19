using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Election;

namespace WarAndAiTweaks.WarChanges {

    [HarmonyPatch(typeof(KingdomDecisionProposalBehavior), "GetRandomPeaceDecision")]
    [HarmonyPriority(999)]
    public class RandomPeacePatch {

        public static void Postfix(ref Clan clan, ref KingdomDecision __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableDeclarePeaceChanges) 
                return; 

            __result = null;
            if (clan == null || clan.Kingdom == null || clan.Leader == null || clan.Kingdom.RulingClan == null || clan.Kingdom.MapFaction == null) { return; }
            if (clan.Kingdom.UnresolvedDecisions.FirstOrDefault((KingdomDecision x) => x is DeclareWarDecision) != null) { return; }
            if (clan.Kingdom.RulingClan == Clan.PlayerClan || clan != clan.Kingdom.RulingClan) { return; }
            Kingdom clanKingdom = clan.Kingdom;
            List<Kingdom> list = new List<Kingdom>();
            foreach (Kingdom kingdom2 in Campaign.Current.Kingdoms) {
                bool flag5 = kingdom2 != null && kingdom2.MapFaction.IsKingdomFaction && clanKingdom.IsAtWarWith(kingdom2);
                if (flag5) {
                    list.Add(kingdom2);
                }
            }
            foreach (Kingdom kingdom3 in list) {
                bool flag6 = kingdom3.RulingClan == Hero.MainHero.Clan;
                if (flag6) {
                    MakePeaceKingdomDecision makePeaceKingdomDecision = RandomPeacePatch.checkBothPartiesAgreeToPeace(clanKingdom, kingdom3);
                    bool flag7 = makePeaceKingdomDecision != null;
                    if (flag7) {
                        __result = makePeaceKingdomDecision;
                        break;
                    }
                } else {
                    MakePeaceKingdomDecision makePeaceKingdomDecision2 = RandomPeacePatch.checkBothPartiesAgreeToPeace(clanKingdom, kingdom3);
                    MakePeaceKingdomDecision makePeaceKingdomDecision3 = RandomPeacePatch.checkBothPartiesAgreeToPeace(kingdom3, clanKingdom);
                    bool flag8 = makePeaceKingdomDecision2 != null && makePeaceKingdomDecision3 != null;
                    if (flag8) {
                        __result = makePeaceKingdomDecision2;
                        break;
                    }
                }
            }
        }

        public static MakePeaceKingdomDecision checkBothPartiesAgreeToPeace(Kingdom askingKingdom, Kingdom receivingKingdom) {
            Object[] parametersArray = new object[] { askingKingdom.RulingClan, receivingKingdom.RulingClan, askingKingdom.MapFaction, receivingKingdom.MapFaction, null };
            bool ConsiderPeaceResult = (bool)typeof(KingdomDecisionProposalBehavior).GetMethod("ConsiderPeace", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new KingdomDecisionProposalBehavior(), parametersArray);
            MakePeaceKingdomDecision decision = (MakePeaceKingdomDecision)parametersArray[4];
            if (ConsiderPeaceResult == true) { return decision; }
            return null;
        }
    }
}

