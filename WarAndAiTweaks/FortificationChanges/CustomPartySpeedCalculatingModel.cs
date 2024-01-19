using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace WarAndAiTweaks.FortificationChanges {
    public class CustomPartySpeedCalculatingModel : DefaultPartySpeedCalculatingModel {

        public override ExplainedNumber CalculateFinalSpeed(MobileParty mobileParty, ExplainedNumber explanation) {
            ExplainedNumber result = base.CalculateFinalSpeed(mobileParty, explanation);
            if (!WarAndAiTweaks.Settings.SlowDownPenalty) { return result; }

            if (mobileParty == null || mobileParty.LeaderHero == null || mobileParty.LeaderHero.Clan == null || mobileParty.LeaderHero.Clan.Kingdom == null || FactionManager.GetEnemyKingdoms(mobileParty.LeaderHero.Clan.Kingdom).Count() <= 0) { return result; }
            if (GetsSpeedPenalty(mobileParty)) { result.AddFactor(-.30f, new TaleWorlds.Localization.TextObject("Nearby Enemy Fortifications")); }
            return result;
        }

        public static bool GetsSpeedPenalty(MobileParty party) {
            foreach (Kingdom kingdom in FactionManager.GetEnemyKingdoms(party.LeaderHero.Clan.Kingdom)) {
                if (kingdom.Settlements.Where(x => !x.IsVillage && ((Campaign.Current.Models.MapDistanceModel.GetDistance(party, x) <= 20f && x.IsCastle) || (Campaign.Current.Models.MapDistanceModel.GetDistance(party, x) <= 30f && x.IsTown))).Count() > 0) { return true; }
            }
            return false;
        }
    }
}
