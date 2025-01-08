using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace WarAndAiTweaks {
    [HarmonyPatch(typeof(DefaultArmyManagementCalculationModel), "GetMobilePartiesToCallToArmy")]
    [HarmonyPriority(999)]
    public class PreventClanMembersBeingCalledToAiArmiesPatch {
        public static void Postfix(MobileParty leaderParty, List<MobileParty> __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnablePreventClanMembersFromBeingCalledToArmies)
                return;

            //Check for nulls
            if (leaderParty == null || leaderParty.LeaderHero == null || leaderParty.Army == null || __result == null || __result.Count == 0)
                return;

            //Start
            if (leaderParty.LeaderHero != Hero.MainHero) {
                foreach (MobileParty party in __result.ToList()) { if (party.LeaderHero.Clan != null && party.LeaderHero.Clan == Clan.PlayerClan) { __result.Remove(party); } }
            }
            return;
        }
    }

    //Feature for changes the amount of influence clans can spend on armies******************************************
    [HarmonyPatch(typeof(DefaultArmyManagementCalculationModel), "GetMobilePartiesToCallToArmy")]
    [HarmonyPriority(998)]
    public class ArmyCallPartiesPatch {
        public static void Postfix(MobileParty leaderParty, List<MobileParty> __result) {
            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableArmyInfluenceChanges)
                return;

            //Check for nulls
            if (leaderParty == null || leaderParty.LeaderHero == null || leaderParty.LeaderHero.PartyBelongedTo == null || leaderParty.LeaderHero.Clan == null || leaderParty.Army == null)
                return;

            //Start
            List<MobileParty> partiesEligible = new List<MobileParty>();
            List<MobileParty> partiesToCall = new List<MobileParty>();
            float armyLeaderInfuenceToSpend = leaderParty.LeaderHero.Clan.Influence * 0.8f;
            foreach (WarPartyComponent warPartyComponent in leaderParty.MapFaction.WarPartyComponents) {
                MobileParty mobileParty = warPartyComponent.MobileParty;
                Hero leaderHero = mobileParty.LeaderHero;
                if (mobileParty.IsLordParty && mobileParty.Army == null && mobileParty != leaderParty && leaderHero != null && !mobileParty.IsMainParty && leaderHero != leaderHero.MapFaction.Leader && !mobileParty.Ai.DoNotMakeNewDecisions) {
                    Settlement currentSettlement = mobileParty.CurrentSettlement;
                    if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) == null && !mobileParty.IsDisbanding && mobileParty.Food > -(mobileParty.FoodChange * 3f) && leaderHero.CanLeadParty() && mobileParty.MapEvent == null && mobileParty.BesiegedSettlement == null) {
                        partiesEligible.Add(mobileParty);
                    }
                }
            }
            //If prevent clan calls to army is on, remove them here to stop AI from considering them.
            if (WarAndAiTweaks.Settings.EnablePreventClanMembersFromBeingCalledToArmies) {
                foreach (MobileParty party in partiesEligible.ToList()) {
                    if (party.LeaderHero.Clan == Clan.PlayerClan) {
                        partiesEligible.Remove(party);
                    }
                }
            }
            foreach (MobileParty eligibleParty in partiesEligible) {
                float influenceToCall = (float)Campaign.Current.Models.ArmyManagementCalculationModel.CalculatePartyInfluenceCost(leaderParty, eligibleParty);
                if (influenceToCall <= 0) { break; } else if (armyLeaderInfuenceToSpend > (float)influenceToCall) {
                    int num3 = Campaign.Current.Models.ArmyManagementCalculationModel.CalculatePartyInfluenceCost(leaderParty, eligibleParty);
                    float totalStrength = eligibleParty.Party.TotalStrength;
                    float num4 = 1f - (float)(eligibleParty.Party.MemberRoster.TotalWounded / eligibleParty.Party.MemberRoster.TotalManCount);
                    float valueOfEligableParty = totalStrength / ((float)num3 + 0.1f) * num4;
                    float valueThreshold = 0.01f;
                    //If the clan has less than 1000 influence, care about the value, otherwise, do not.
                    if (valueOfEligableParty > valueThreshold && leaderParty.LeaderHero.Clan.Influence <= 1000) {
                        armyLeaderInfuenceToSpend -= influenceToCall;
                        partiesToCall.Add(eligibleParty);
                    } else if (leaderParty.LeaderHero.Clan.Influence > 1000) {
                        armyLeaderInfuenceToSpend -= influenceToCall;
                        partiesToCall.Add(eligibleParty);
                    } else { continue; }
                }
            }
            __result = partiesToCall;
            return;
        }
    }

    //Feature to change the amount of influence it costs to call parties by ruler by 1/2
    [HarmonyPatch(typeof(DefaultArmyManagementCalculationModel), "CalculatePartyInfluenceCost")]
    [HarmonyPriority(999)]
    public class KingdomRulerCallPartyPatch {
        public static void Postfix(MobileParty armyLeaderParty, ref int __result) {

            //If disabled, skip logic
            if (!WarAndAiTweaks.Settings.EnableRulerInfluenceChange)
                return;

            //Check for nulls
            if (armyLeaderParty == null || armyLeaderParty.LeaderHero == null || armyLeaderParty.LeaderHero.Clan == null || armyLeaderParty.LeaderHero.Clan.Kingdom == null)
                return;

            //Start
            if (armyLeaderParty.LeaderHero == armyLeaderParty.LeaderHero.Clan.Kingdom.RulingClan.Leader) {
                __result = __result / 2;

                return;
            }
        }
    }

    //Feature to change the amount of influence it costs to call parties by ruler by 1/2
    [HarmonyPatch(typeof(Kingdom), "CreateArmy")]
    [HarmonyPriority(999)]
    public class CreateArmyPatch {
        public static bool Prefix(Hero armyLeader, Settlement targetSettlement, Army.ArmyTypes selectedArmyType) {
            if (!WarAndAiTweaks.Settings.EnableMilitaryLogicChanges)
                return true;

            if (armyLeader == Hero.MainHero)
                return true;

            if (selectedArmyType != Army.ArmyTypes.Besieger && selectedArmyType != Army.ArmyTypes.Defender)
                return false;

            return true;
        }
    }

    //Feature to change the behavior of faction military
    [HarmonyPatch(typeof(AiMilitaryBehavior), "AiHourlyTick")]
    [HarmonyPriority(999)]
    public class PartyThinkPartyPatch {
        public static void Postfix(MobileParty mobileParty, ref PartyThinkParams p) {
            if (!WarAndAiTweaks.Settings.EnableMilitaryLogicChanges)
                return;

            if (!mobileParty.IsLordParty || mobileParty.LeaderHero == null || mobileParty.LeaderHero.Clan == null || mobileParty.LeaderHero.Clan.Kingdom == null)
                return;

            if (mobileParty.Army != null && mobileParty.Army.LeaderParty != mobileParty)
                return;

            // Dont run changes on parties that are besieging, instead cancel the siege if the parties strength is lower than the settlement.
            if (mobileParty.BesiegedSettlement != null) {
                if (mobileParty == MobileParty.MainParty)
                    return;

                // Get besieged settlement total strength
                float besiegedSettlementStrength = 0;
                foreach (MobileParty party in mobileParty.BesiegedSettlement.Parties)
                    besiegedSettlementStrength += party.Party.TotalStrength;

                // END SIEGE IF SIEGING PARTY IS LESS STRENGTH THAN SETTLEMENT
                if (mobileParty.GetTotalStrengthWithFollowers() <= besiegedSettlementStrength)
                    mobileParty.BesiegerCamp.RemoveAllSiegeParties();

                // End any further logic
                return;
            }

            //If nothing to attack, stop the logic
            List<IFaction> factionsAtWarWithSettlements = new List<IFaction>();
            var EnemyFactions = FactionManager.GetEnemyFactions(mobileParty.LeaderHero.Clan.Kingdom.MapFaction).Where(x => x.Settlements.Count > 0);
            if (EnemyFactions.Count() <= 0)
                return;

            foreach (IFaction faction in EnemyFactions)
                factionsAtWarWithSettlements.Add(faction);

            if (factionsAtWarWithSettlements.Count <= 0)
                return;

            //If we need to send the lords to get troops, stop here
            //if (sendLordsToGetTroops(mobileParty, p)) { return; }

            bool hasArmy = mobileParty.Army != null;
            Clan clan = mobileParty.LeaderHero.Clan;
            Kingdom kingdom = clan.Kingdom;

            List<float> patrolScores = new List<float>();

            List<Settlement> weakSettlements = getWeakSettlementsToPatrol(mobileParty);

            foreach ((AIBehaviorTuple, float) keyValuePair in p.AIBehaviorScores.ToList()) {
                int indexOfBehaviorScore = p.AIBehaviorScores.FindIndex(s => s.Item1 == keyValuePair.Item1 && s.Item2 == keyValuePair.Item2);
                if (indexOfBehaviorScore == -1)
                    continue;

                Settlement targetSettlement = keyValuePair.Item1.Party as Settlement;
                AiBehavior behavior = keyValuePair.Item1.AiBehavior;

                if ((behavior == AiBehavior.BesiegeSettlement || behavior == AiBehavior.AssaultSettlement) && targetSettlement.IsUnderSiege) {
                    p.AIBehaviorScores[indexOfBehaviorScore] = (keyValuePair.Item1, keyValuePair.Item2 * 10f);
                }

                //Update army logic
                if (hasArmy && (behavior == AiBehavior.PatrolAroundPoint && !weakSettlements.Contains(targetSettlement)) || behavior == AiBehavior.RaidSettlement) {
                    p.AIBehaviorScores.Remove(keyValuePair);
                }

                //Clans should defend what is theirs
                if (behavior == AiBehavior.PatrolAroundPoint && (keyValuePair.Item1.Party == null || !(keyValuePair.Item1.Party is Settlement) || (keyValuePair.Item1.Party is Settlement && ((keyValuePair.Item1.Party as Settlement).OwnerClan != mobileParty.LeaderHero.Clan) && !weakSettlements.Contains(targetSettlement)))) {
                    patrolScores.Add(keyValuePair.Item2);
                    p.AIBehaviorScores.Remove(keyValuePair);
                }
                if (behavior == AiBehavior.DefendSettlement && !hasArmy && targetSettlement.OwnerClan == mobileParty.LeaderHero.Clan) {
                    p.AIBehaviorScores[indexOfBehaviorScore] = (keyValuePair.Item1, keyValuePair.Item2 + .50f);
                }
                if (behavior == AiBehavior.DefendSettlement && !targetSettlement.IsVillage && targetSettlement.OwnerClan == mobileParty.LeaderHero.Clan) {
                    p.AIBehaviorScores[indexOfBehaviorScore] = (keyValuePair.Item1, keyValuePair.Item2 * 10f);
                }
            }

            if (!hasArmy && mobileParty.PartySizeRatio > 0.6f && mobileParty.GetNumDaysForFoodToLast() > 3 && patrolScores.Count > 0 && kingdom.Armies.Count > 0) {
                convertPatrolToHelpSiegeOrDefend(mobileParty, patrolScores, p);
            }
        }

        public static List<Settlement> getWeakSettlementsToPatrol(MobileParty party) {
            float strength = 0f;
            List<Settlement> ret = new List<Settlement>();
            foreach (Settlement settlement in party.LeaderHero.Clan.Kingdom.Settlements.Where(x => !x.IsVillage)) {
                foreach (MobileParty Setparty in settlement.Parties) {
                    strength += Setparty.Party.MemberRoster.Count;
                }
                if (strength <= 300f) { ret.Add(settlement); }
            }
            return ret;
        }

        public static void convertPatrolToHelpSiegeOrDefend(MobileParty party, List<float> scores, PartyThinkParams p) {
            foreach (Army army in party.LeaderHero.Clan.Kingdom.Armies) {
                if (army.AIBehavior == Army.AIBehaviorFlags.AssaultingTown || army.AIBehavior == Army.AIBehaviorFlags.Besieging || (army.AIBehavior == Army.AIBehaviorFlags.TravellingToAssignment && army.ArmyType == Army.ArmyTypes.Besieger) || (army.AIBehavior == Army.AIBehaviorFlags.Defending && army.AiBehaviorObject is Settlement && !(army.AiBehaviorObject as Settlement).IsVillage)) {
                    AIBehaviorTuple key = new AIBehaviorTuple(army.LeaderParty, AiBehavior.EscortParty, false);
                    if (p.AIBehaviorScores.Any(m => m.Item1 == key)) { continue; }
                    p.AIBehaviorScores.Add((key, scores.Max()));
                }
            }
        }
    }

    [HarmonyPatch(typeof(AiMilitaryBehavior), "FindBestTargetAndItsValueForFaction")]
    public class FindBestTargetAndItsValueForFactionPatch {
        private static void Postfix(Army.ArmyTypes missionType, PartyThinkParams p) {
            if (!WarAndAiTweaks.Settings.EnableMilitaryLogicChanges) { return; }

            if (missionType == Army.ArmyTypes.Besieger) {
                MobileParty mobileParty = (p != null) ? p.MobilePartyOf : null;
                MobileParty mobileParty2;
                if (mobileParty == null) {
                    mobileParty2 = null;
                } else {
                    Army army = mobileParty.Army;
                    mobileParty2 = ((army != null) ? army.LeaderParty : null);
                }
                if (mobileParty2 == mobileParty) {
                    bool flag;
                    if (mobileParty == null) {
                        flag = (null != null);
                    } else {
                        Hero leaderHero = mobileParty.LeaderHero;
                        if (leaderHero == null) {
                            flag = (null != null);
                        } else {
                            Clan clan = leaderHero.Clan;
                            flag = (((clan != null) ? clan.Kingdom : null) != null);
                        }
                    }
                    if (flag) {
                        Dictionary<AIBehaviorTuple, float> dictionary = new Dictionary<AIBehaviorTuple, float>(10);
                        float num = 99999f;
                        Settlement settlement = mobileParty.LeaderHero.HomeSettlement;
                        if (settlement == null) {
                            settlement = mobileParty.LastVisitedSettlement;
                        }
                        if (settlement == null) {
                            return;
                        }
                        Settlement settlement2 = null;
                        foreach ((AIBehaviorTuple, float) keyValuePair in p.AIBehaviorScores) {
                            if (keyValuePair.Item2 > 0f && keyValuePair.Item1.AiBehavior == AiBehavior.BesiegeSettlement && keyValuePair.Item1.Party != null && keyValuePair.Item1.Party is Settlement) {
                                //dictionary.Add(keyValuePair.Item1, keyValuePair.Item2);
                                dictionary[keyValuePair.Item1] = keyValuePair.Item2;
                                float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, keyValuePair.Item1.Party as Settlement);
                                if (distance < num) {
                                    num = distance;
                                    settlement2 = (keyValuePair.Item1.Party as Settlement);
                                }
                            }
                        }
                        foreach (KeyValuePair<AIBehaviorTuple, float> keyValuePair2 in dictionary) {
                            Settlement settlement3 = keyValuePair2.Key.Party as Settlement;
                            float distance2 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, settlement3);
                            float value = keyValuePair2.Value * 1.2f * Math.Max(0f, 1f - (distance2 - num));

                            AIBehaviorTuple newTuple = keyValuePair2.Key;

                            p.AIBehaviorScores.Remove((keyValuePair2.Key, keyValuePair2.Value));
                            p.AIBehaviorScores.Add((newTuple, value));
                        }
                    }
                }
            }
        }
    }
}