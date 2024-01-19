using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy;
using TaleWorlds.Library;

namespace WarAndAiTweaks.WarChanges {

    [HarmonyPatch(typeof(KingdomDiplomacyVM), "OnDeclarePeace")]
    [HarmonyPriority(999)]
    public class KingdomPlayerPeacePatch {

        public static bool Prefix(KingdomWarItemVM item, ref KingdomDiplomacyVM __instance, KingdomDecision ____currentItemsUnresolvedDecision, Action<KingdomDecision> ____forceDecision) {
            bool result;
            if (!(WarAndAiTweaks.Settings.otherFactionMustWantPeace) || !(WarAndAiTweaks.Settings.EnableDeclarePeaceChanges)) {
                result = true;
            } else {
                bool flag2 = ____currentItemsUnresolvedDecision != null;
                if (flag2) {
                    ____forceDecision(____currentItemsUnresolvedDecision);
                    result = false;
                } else {
                    object[] array = new object[5];
                    array[0] = (item.Faction2 as Kingdom).RulingClan;
                    array[1] = Hero.MainHero.Clan.Kingdom.RulingClan;
                    array[2] = (item.Faction2 as Kingdom).MapFaction;
                    array[3] = Hero.MainHero.Clan.Kingdom.MapFaction;
                    object[] parameters = array;
                    bool flag3 = (bool)typeof(KingdomDecisionProposalBehavior).GetMethod("ConsiderPeace", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(new KingdomDecisionProposalBehavior(), parameters);
                    bool flag4 = !flag3;
                    if (flag4) {
                        InformationManager.DisplayMessage(new InformationMessage((item.Faction2 as Kingdom).Name.ToString() + " does not want to make peace with you at this time."));
                        result = false;
                    } else {
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}
