using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace WarAndAiTweaks.Militia_And_Garrison_Changes {
    // Token: 0x02000016 RID: 22
    [HarmonyPatch(typeof(Town), "DailyGarrisonAdjustment")]
    public class GarrisonPatch {
        // Token: 0x0600005C RID: 92 RVA: 0x000043B0 File Offset: 0x000025B0
        public static void Prefix(ref Town __instance) {
            if (WarAndAiTweaks.Settings.EnableRecruitmentChange) {
                int num = __instance.GarrisonAutoRecruitmentIsEnabled ? __instance.GarrisonChangeAutoRecruitment : 0;
                bool flag2 = num > 0 && !__instance.IsUnderSiege;
                if (flag2) {
                    Settlement currentSettlement = __instance.GarrisonParty.CurrentSettlement;
                    Hero leader = currentSettlement.OwnerClan.Leader;
                    int num2 = 0;
                    foreach (Hero hero in currentSettlement.Notables) {
                        int num3 = Campaign.Current.Models.VolunteerModel.MaximumIndexHeroCanRecruitFromHero(leader, hero, leader.GetRelation(hero));
                        for (int i = 0; i < num3; i++) {
                            bool flag3 = hero.VolunteerTypes[i] != null;
                            if (flag3) {
                                __instance.GarrisonParty.MemberRoster.AddToCounts(hero.VolunteerTypes[i], 1, false, 0, 0, true, -1);
                                num2++;
                                leader.Clan.AutoRecruitmentExpenses += Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(hero.VolunteerTypes[i], leader, false);
                                hero.VolunteerTypes[i] = null;
                            }
                        }
                    }
                    foreach (Village village in currentSettlement.BoundVillages) {
                        bool flag4 = village.VillageState == Village.VillageStates.Normal;
                        if (flag4) {
                            foreach (Hero hero2 in village.Settlement.Notables) {
                                int num4 = Campaign.Current.Models.VolunteerModel.MaximumIndexHeroCanRecruitFromHero(leader, hero2, leader.GetRelation(hero2));
                                for (int j = 0; j < num4; j++) {
                                    bool flag5 = hero2.VolunteerTypes[j] != null;
                                    if (flag5) {
                                        __instance.GarrisonParty.MemberRoster.AddToCounts(hero2.VolunteerTypes[j], 1, false, 0, 0, true, -1);
                                        num2++;
                                        leader.Clan.AutoRecruitmentExpenses += Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(hero2.VolunteerTypes[j], leader, false);
                                        hero2.VolunteerTypes[j] = null;
                                    }
                                }
                            }
                        }
                    }
                    bool flag6 = currentSettlement.OwnerClan == Clan.PlayerClan;
                    if (flag6) {
                        InformationManager.DisplayMessage(new InformationMessage(num2.ToString() + " were added to " + currentSettlement.Name.ToString() + "'s Garrison from local regions."));
                    }
                }
            }
        }
    }
}
