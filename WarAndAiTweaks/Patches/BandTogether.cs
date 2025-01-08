using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace WarAndAiTweaks {
    //Band together logic for reducing troop recruitment cost
    [HarmonyPatch(typeof(DefaultPartyWageModel), "GetTroopRecruitmentCost")]
    public class PartyRecruitmentPatch {
        static void Postfix(CharacterObject troop, Hero buyerHero, ref int __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableBandTogetherLogic)
                return;

            try {
                //Check for nulls
                if (buyerHero == null || buyerHero.Clan == null || buyerHero.Clan.Kingdom == null || buyerHero.Clan.Kingdom.Fiefs.Count <= 0 || buyerHero.Clan.Kingdom.Fiefs.Count > 10 || troop == null)
                    return;

                if (buyerHero.Clan.Kingdom.RulingClan == Clan.PlayerClan && !WarAndAiTweaks.Settings.playerKingdomIsIncluded)
                    return;

                //Get the percentage
                int percent = (int)((buyerHero.Clan.Kingdom.Fiefs.Count * 100.0f) / 10);
                __result = (__result * percent) / 100;
                return;
            } catch (Exception ex) {
                return;
            }

        }
    }

    //Band together logic for reducing garrison costs
    [HarmonyPatch(typeof(DefaultClanFinanceModel), "CalculatePartyWage")]
    public class garrisonWagePatch {
        //Changes for Garrison cost calculation
        static void Postfix(MobileParty mobileParty, ref int __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableBandTogetherLogic)
                return;

            try {
                //Check for nulls
                if (mobileParty == null || !mobileParty.IsLordParty || mobileParty.LeaderHero == null || mobileParty.LeaderHero.Clan == null || mobileParty.LeaderHero.Clan.Kingdom == null || mobileParty.LeaderHero.Clan.Kingdom.Fiefs.Count <= 0 || mobileParty.LeaderHero.Clan.Kingdom.Fiefs.Count > 10)
                    return;

                if (mobileParty.LeaderHero.Clan.Kingdom.RulingClan == Clan.PlayerClan && !WarAndAiTweaks.Settings.playerKingdomIsIncluded)
                    return;

                //Get the percentage
                int percent = (int)((mobileParty.LeaderHero.Clan.Kingdom.Fiefs.Count * 100.0f) / 10);
                __result = (__result * percent) / 100;
                return;
            } catch (Exception ex) {
                return;
            }
        }
    }

    //Feature to change how much it costs to call parties to armies in band together logic.
    [HarmonyPatch(typeof(DefaultArmyManagementCalculationModel), "CalculatePartyInfluenceCost")]
    [HarmonyPriority(999)]
    public class BandTogetherInfluencePatch {
        public static void Postfix(MobileParty armyLeaderParty, ref int __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableBandTogetherLogic)
                return;

            try {
                //Check for nulls
                if (armyLeaderParty == null || armyLeaderParty.LeaderHero == null || armyLeaderParty.LeaderHero.Clan == null || armyLeaderParty.LeaderHero.Clan.Kingdom == null || armyLeaderParty.LeaderHero.Clan.Kingdom.Fiefs.Count <= 0 || armyLeaderParty.LeaderHero.Clan.Kingdom.Fiefs.Count > 10)
                    return;

                if (armyLeaderParty.LeaderHero.Clan.Kingdom.RulingClan == Clan.PlayerClan && !WarAndAiTweaks.Settings.playerKingdomIsIncluded)
                    return;

                //Start
                //Get the percentage
                int percent = (int)((armyLeaderParty.LeaderHero.Clan.Kingdom.Fiefs.Count * 100.0f) / 10);
                __result = (__result * percent) / 100;
                return;
            } catch (Exception ex) {
                return;
            }
        }
    }
}
