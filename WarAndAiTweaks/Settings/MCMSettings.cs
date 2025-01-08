using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace WarAndAiTweaks {
    public class MCMSettings : AttributeGlobalSettings<MCMSettings> {

        [SettingPropertyGroup("{=WAI_V8krxN}War and Peace/War Changes")]
        [SettingPropertyBool("{=WAI_Mf0D9r}Enable Changes For Declaring War", Order = 0, RequireRestart = false, IsToggle = true, HintText = "{=WAI_yXzbqm}If enabled, changes logic so that only rulers can declare war and AI will only consider best target instead of a random one. Default = Enabled.")]
		public bool EnableDeclareWarChanges { get; set; } = true;

        [SettingPropertyGroup("{=WAI_V8krxN}War and Peace/Peace Changes")]
        [SettingPropertyBool("{=WAI_3xcrKW}Enable Changes For Declaring Peace", Order = 1, RequireRestart = false, IsToggle = true, HintText = "{=WAI_v92Ex5}If enabled, changes logic so that only rulers can declare peace and *BOTH* sides must agree to peace. (Both sides agreeing does not include player kingdom yet). Default = Enabled")]
		public bool EnableDeclarePeaceChanges { get; set; } = true;

        [SettingPropertyGroup("{=WAI_V8krxN}War and Peace/Peace Changes")]
        [SettingPropertyInteger("{=WAI_YtXewV}Target Peace Time", 1, 120, "0", Order = 1, RequireRestart = false, HintText = "{=WAI_i0FVps}The amount of days an AI faction will consider a good amount of time to be at total peace to recoup loses and build up strength. This does not prevent them from declaring war before this amount of days if the opportunity arises. Default = 40")]
		public int targetPeaceTime { get; set; } = 60;

        [SettingPropertyGroup("{=WAI_V8krxN}War and Peace/Peace Changes")]
        [SettingPropertyBool("{=WAI_YdzGi4}Other faction must agree to peace for player faction", Order = 3, RequireRestart = false, HintText = "{=WAI_qDobnE}If enabled, the other faction must agree to peace before the player can propose it to their kingdom. Default = Enabled")]
		public bool otherFactionMustWantPeace { get; set; } = true;



        [SettingPropertyGroup("{=WAI_K6krtu}FillStacks")]
        [SettingPropertyBool("{=WAI_naGmUB}Enable FillStacks", Order = 0, RequireRestart = false, IsToggle = true, HintText = "{=WAI_naGmUg}This is an integrated mod removes the free troops that the AI gets when they respawn. Default = Enabled.")]
		public bool EnableFillStacks { get; set; } = true;

        [SettingPropertyGroup("{=WAI_K6krtu}FillStacks")]
        [SettingPropertyInteger("{=WAI_ZYM5T1}FillStacks Troop Count", 1, 100, "0", Order = 1, RequireRestart = false, HintText = "{=WAI_7yzj1P}Controls the amount of free troops AI lords get when they respawn. Default = 3. (3 is used so they don't get captured immediately by bandits/looters)")]
		public int FillStackTroopCount { get; set; } = 3;

        [SettingPropertyGroup("{=WAI_K6krtu}FillStacks")]
        [SettingPropertyBool("{=WAI_zH5MWH}Mercenary Parties Apply", Order = 2, RequireRestart = false, HintText = "{=WAI_z99bZB}If Enabled, Mercenary clans also follow fillstack rules.")]
		public bool FillStackMercenaries { get; set; } = false;



        [SettingPropertyGroup("{=WAI_sVRwEA}Militia Changes")]
        [SettingPropertyBool("{=WAI_GaAeaG}Enable Militia Change", Order = 0, RequireRestart = false, IsToggle = true, HintText = "{=WAI_WODJA5}Changes the bonuses to the barracks buildings in Towns/Castles to create more militia to defend. Default = Enabled")]
		public bool EnableMilitiaChange { get; set; } = true;

        [SettingPropertyGroup("{=WAI_sVRwEA}Militia Changes")]
        [SettingPropertyFloatingInteger("{=WAI_6Eyxyw}Castle Militia Bonus", 0f, 100f, "0.00", Order = 1, RequireRestart = false, HintText = "{=WAI_dka4CP}Control the bonus militia added to the barracks in a Castle. Default = 1.0; 0 = No Bonus")]
		public float CastleMilitiaBoost { get; set; } = 1f;

        [SettingPropertyGroup("{=WAI_sVRwEA}Militia Changes")]
        [SettingPropertyFloatingInteger("{=WAI_GINfAP}Town Militia Bonus", 0f, 100f, "0.00", Order = 2, RequireRestart = false, HintText = "{=WAI_5bULNj}Control the bonus militia added to the barracks in a Town. Default = 0.5; 0 = No Bonus")]
		public float TownMilitiaBoost { get; set; } = 0.5f;



        [SettingPropertyGroup("{=WAI_vBH4P5}Army Changes")]
        [SettingPropertyBool("{=WAI_35dS0f}Enable Military Logic Changes", Order = 0, RequireRestart = false, HintText = "{=WAI_354j0g}Changes the way AI behaves in war. Armies are not called for raiding and patrolling, only attacking and defending. Lord refill ranks from garrisons, lords and armies prioritize attacking and defending resulting in much less idling for factions at war. Default = Enabled")]
		public bool EnableMilitaryLogicChanges { get; set; } = true;

        [SettingPropertyGroup("{=WAI_vBH4P5}Army Changes")]
        [SettingPropertyBool("{=WAI_354j0f}Enable Army Influence Spending Chance", Order = 1, RequireRestart = false, HintText = "{=WAI_35dS78}Changes the way that armies call other lord parties by increasing the amount of influence they can spend. Armies, in vanilla, cannot spend a lot of influence on calling armies. Default = Enabled")]
		public bool EnableArmyInfluenceChanges { get; set; } = true;

        [SettingPropertyGroup("{=WAI_vBH4P5}Army Changes")]
        [SettingPropertyBool("{=WAI_35d5rt}Enable Ruler Army Influence Cost Reduction", Order = 2, RequireRestart = false, HintText = "{=WAI_hetS0f}If enabled, rulers of kingdoms require 50% of the influence it would normally require to call armies. Default = Enabled")]
		public bool EnableRulerInfluenceChange { get; set; } = true;

        [SettingPropertyGroup("{=WAI_vBH4P5}Army Changes")]
        [SettingPropertyBool("{=WAI_hasd0f}Enable 'Prevent Clan Members From Being Called To Armies'", Order = 3, RequireRestart = false, HintText = "{=WAI_as48wd}If enabled, your clan members can no longer be called to ai armies. Default = Enabled")]
		public bool EnablePreventClanMembersFromBeingCalledToArmies { get; set; } = true;



        [SettingPropertyGroup("{=WAI_5AKRK0}Garrison Changes")]
        [SettingPropertyBool("{=WAI_em1a0r}Enable Recruitment Change", Order = 0, RequireRestart = false, HintText = "{=WAI_gh3y7a}If enabled, changes the the auto recruitment for castles/towns so that instead of vanilla (+1 troop per day) it recruits from all available troops from local regions (Bound Villages/Town itself). Default = Enabled")]
		public bool EnableRecruitmentChange { get; set; } = true;

        [SettingPropertyGroup("{=WAI_8g0U40}Garrison Changes/Cost Reduction")]
        [SettingPropertyBool("{=WAI_2h95na}Enable Garrison Cost Reduction", Order = 1, RequireRestart = false, IsToggle = true, HintText = "{=WAI_dl59ac}Enables the reduction of garrison party costs. Vanilla is 1-1 so you pay the same amount for troops in a garrison that you would in your party. Default = Enabled")]
		public bool EnableGarrisonCostReduction { get; set; } = true;

        [SettingPropertyGroup("{=WAI_8g0U40}Garrison Changes/Cost Reduction")]
        [SettingPropertyFloatingInteger("{=WAI_3jf8sn}Garrison Cost Multiplier", 0f, 100f, "0.00", Order = 2, RequireRestart = false, HintText = "{=WAI_53ksnq}Multiplier for garrison costs. 0.5 = 50% cost; 0 = No Cost; Default = 0.5")]
		public float GarrisonCostMultiply { get; set; } = 0.5f;



        [SettingPropertyGroup("{=WAI_3F0Jal}Band Together Logic")]
        [SettingPropertyBool("{=WAI_346heg}Band Together Logic", Order = 0, RequireRestart = false, IsToggle = true, HintText = "{=WAI_OG9G6x}Very weak factions will 'band together' and do what it takes to survive. Calculation is party wage/recruit cost/influence to call armies are reduced by percentage past 10 fiefs. For example, if they have 3 fiefs left they will pay 30% of all before mentioned. This wont stop them from being destroyed by any means. But it will give them a chance. Includes Player Kingdom; Default = Enabled")]
		public bool EnableBandTogetherLogic { get; set; } = true;

        [SettingPropertyGroup("{=WAI_3F0Jal}Band Together Logic")]
        [SettingPropertyBool("{=WAI_K2bc20}Player Kingdom is included", Order = 0, RequireRestart = false, HintText = "{=WAI_AgQghj}Determines if the player kingdom is included in the band together logic if you are the ruler of your kingdom; Default = False")]
		public bool playerKingdomIsIncluded { get; set; } = false;



        [SettingPropertyGroup("{=WAI_OUkZom}Fiefs Create Movement Penalty")]
        [SettingPropertyBool("{=WAI_OUkZom}Fiefs Create Movement Penalty", Order = 0, RequireRestart = false, IsToggle = true, HintText = "{=WAI_UnjNW8}Castles and Towns now add a movement penalty for hostile parties/armies. Towns have a larger sphere influence than castles. Moving through hostile territory is now much more dangerous. Default = Enabled")]
		public bool SlowDownPenalty { get; set; } = true;



        [SettingPropertyGroup("{=WAI_N7DNB0}Party Speed Modifications")]
        [SettingPropertyBool("{=WAI_N7DNB0}Party Speed Modifications", Order = 0, RequireRestart = true, IsToggle = true, HintText = "{=WAI_LMYUJp}Toggle this off to prevent this mod from making party speed changes. Implemented for compatibility with Banner Kings. Default = Enabled")]
		public bool PartySpeedModifications { get; set; } = true;

        public override string Id { get { return base.GetType().Assembly.GetName().Name; } }
        public override string DisplayName { get { return base.GetType().Assembly.GetName().Name; } }
        public override string FolderName { get { return base.GetType().Assembly.GetName().Name; } }
        public override string FormatType { get; } = "xml";
        public bool LoadMCMConfigFile { get; set; } = true;
    }
}
