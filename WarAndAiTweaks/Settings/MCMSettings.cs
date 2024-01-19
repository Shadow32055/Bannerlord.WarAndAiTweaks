using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace WarAndAiTweaks {
    public class MCMSettings : AttributeGlobalSettings<MCMSettings> {

		[SettingPropertyBool("Enable Changes For Declaring War", Order = 0, RequireRestart = false, IsToggle = true, HintText = "If enabled, changes logic so that only rulers can declare war and AI will only consider best target instead of a random one. Default = Enabled.")]
		[SettingPropertyGroup("War and Peace/War Changes")]
		public bool EnableDeclareWarChanges { get; set; } = true;


		[SettingPropertyBool("Enable Changes For Declaring Peace", Order = 1, RequireRestart = false, IsToggle = true, HintText = "If enabled, changes logic so that only rulers can declare peace and *BOTH* sides must agree to peace. (Both sides agreeing does not include player kingdom yet). Default = Enabled")]
		[SettingPropertyGroup("War and Peace/Peace Changes")]
		public bool EnableDeclarePeaceChanges { get; set; } = true;


		[SettingPropertyInteger("Target Peace Time", 1, 120, "0", Order = 1, RequireRestart = false, HintText = "The amount of days an AI faction will consider a good amount of time to be at total peace to recoup loses and build up strength. This does not prevent them from declaring war before this amount of days if the opportunity arises. Default = 40")]
		[SettingPropertyGroup("War and Peace/Peace Changes")]
		public int targetPeaceTime { get; set; } = 60;


		[SettingPropertyBool("Other faction must agree to peace for player faction", Order = 3, RequireRestart = false, HintText = "If enabled, the other faction must agree to peace before the player can propose it to their kingdom. Default = Enabled")]
		[SettingPropertyGroup("War and Peace/Peace Changes")]
		public bool otherFactionMustWantPeace { get; set; } = true;


		[SettingPropertyBool("Enable FillStacks", Order = 0, RequireRestart = false, IsToggle = true, HintText = "This is an integrated mod removes the free troops that the AI gets when they respawn. Default = Enabled.")]
		[SettingPropertyGroup("FillStacks")]
		public bool EnableFillStacks { get; set; } = true;


		[SettingPropertyInteger("FillStacks Troop Count", 1, 100, "0", Order = 1, RequireRestart = false, HintText = "Controls the amount of free troops AI lords get when they respawn. Default = 3. (3 is used so they dont get captured immediately by bandits/looters)")]
		[SettingPropertyGroup("FillStacks")]
		public int FillStackTroopCount { get; set; } = 3;


		[SettingPropertyBool("Mercenary Parties Apply", Order = 2, RequireRestart = false, HintText = "If Enabled, Mercenary clans also follow fillstack rules.")]
		[SettingPropertyGroup("FillStacks")]
		public bool FillStackMercenaries { get; set; } = false;


		[SettingPropertyBool("Enable Miltia Change", Order = 0, RequireRestart = false, IsToggle = true, HintText = "Changes the bonuses to the barracks buildings in Towns/Castles to create more miltia to defend. Default = Enabled")]
		[SettingPropertyGroup("Militia Changes")]
		public bool EnableMiltiaChange { get; set; } = true;


		[SettingPropertyFloatingInteger("Castle Militia Bonus", 0f, 100f, "0.00", Order = 1, RequireRestart = false, HintText = "Control the bonus miltia added to the barracks in a Castle. Default = 1.0; 0 = No Bonus")]
		[SettingPropertyGroup("Militia Changes")]
		public float CastleMiltiaBoost { get; set; } = 1f;


		[SettingPropertyFloatingInteger("Town Militia Bonus", 0f, 100f, "0.00", Order = 2, RequireRestart = false, HintText = "Control the bonus miltia added to the barracks in a Town. Default = 0.5; 0 = No Bonus")]
		[SettingPropertyGroup("Militia Changes")]
		public float TownMiltiaBoost { get; set; } = 0.5f;


		[SettingPropertyBool("Enable Military Logic Changes", Order = 0, RequireRestart = false, HintText = "Changes the way AI behaves in war. Armies are not called for raiding and patrolling, only attacking and defending. Lord refill ranks from garrisons, lords and armies prioritize attacking and defending resulting in much less idling for factions at war. Default = Enabled")]
		[SettingPropertyGroup("Army Changes")]
		public bool EnableMilitaryLogicChanges { get; set; } = true;


		[SettingPropertyBool("Enable Army Influence Spending Chance", Order = 1, RequireRestart = false, HintText = "Changes the way that armies call other lord parties by increasing the amount of influence they can spend. Armies, in vanilla, cannot spend alot of influence on calling armies. Default = Enabled")]
		[SettingPropertyGroup("Army Changes")]
		public bool EnableArmyInfluenceChanges { get; set; } = true;


		[SettingPropertyBool("Enable Ruler Army Influence Cost Reduction", Order = 2, RequireRestart = false, HintText = "If enabled, rulers of kingdoms require 50% of the influence it would normally require to call armies. Default = Enabled")]
		[SettingPropertyGroup("Army Changes")]
		public bool EnableRulerInflunceChange { get; set; } = true;


		[SettingPropertyBool("Enable 'Prevent Clan Members From Being Called To Armies'", Order = 3, RequireRestart = false, HintText = "If enabled, your clan members can no longer be called to ai armies. Default = Enabled")]
		[SettingPropertyGroup("Army Changes")]
		public bool EnablePreventClanMembersFromBeingCalledToArmies { get; set; } = true;


		[SettingPropertyBool("Enable Recruitment Change", Order = 0, RequireRestart = false, HintText = "If enabled, changes the the auto recruitment for castles/towns so that instead of vanilla (+1 troop per day) it recruits from all available troops from local regions (Bound Villages/Town itself). Default = Enabled")]
		[SettingPropertyGroup("Garrison Changes")]
		public bool EnableRecruitmentChange { get; set; } = true;


		[SettingPropertyBool("Enable Garrison Cost Reduction", Order = 1, RequireRestart = false, IsToggle = true, HintText = "Enables the reduction of garrison party costs. Vanilla is 1-1 so you pay the same amount for troops in a garrison that you would in your party. Default = Enabled")]
		[SettingPropertyGroup("Garrison Changes/Cost Reduction")]
		public bool EnableGarrisonCostReduction { get; set; } = true;


		[SettingPropertyFloatingInteger("Garrison Cost Multiplier", 0f, 100f, "0.00", Order = 2, RequireRestart = false, HintText = "Mutiplier for garrison costs. 0.5 = 50% cost; 0 = No Cost; Default = 0.5")]
		[SettingPropertyGroup("Garrison Changes/Cost Reduction")]
		public float GarrisonCostMultiply { get; set; } = 0.5f;


		[SettingPropertyBool("Band Together Logic", Order = 0, RequireRestart = false, IsToggle = true, HintText = "Very weak factions will 'band together' and do what it takes to survive. Calculation is party wage/recruit cost/influence to call armies are reduced by percentage past 10 fiefs. For example, if they have 3 fiefs left they will pay 30% of all before mentioned. This wont stop them from being destroyed by any means. But it will give them a chance. Includes Player Kingdom; Default = Enabled")]
		[SettingPropertyGroup("Band Together Logic")]
		public bool EnableBandTogetherLogic { get; set; } = true;


		[SettingPropertyBool("Player Kingdom is included", Order = 0, RequireRestart = false, HintText = "Determines if the player kingdom is included in the band together logic if you are the ruler of your kingdom; Default = False")]
		[SettingPropertyGroup("Band Together Logic")]
		public bool playerKingodmIsIncluded { get; set; } = false;


		[SettingPropertyBool("Fiefs Create Movement Penalty", Order = 0, RequireRestart = false, IsToggle = true, HintText = "Castles and Towns now add a movement penalty for hostile parties/armies. Towns have a larger sphere influence than castles. Moving through hostile territory is now much more dangerous. Default = Enabled")]
		[SettingPropertyGroup("Fiefs Create Movement Penalty")]
		public bool SlowDownPenalty { get; set; } = true;


		[SettingPropertyBool("Party Speed Modifications", Order = 0, RequireRestart = true, IsToggle = true, HintText = "Toggle this off to prevent this mod from making party speed changes. Implemented for compatibility with Banner Kings. Default = Enabled")]
		[SettingPropertyGroup("Party Speed Modifications")]
		public bool PartySpeedModifications { get; set; } = true;

        public override string Id { get { return base.GetType().Assembly.GetName().Name; } }
        public override string DisplayName { get { return base.GetType().Assembly.GetName().Name; } }
        public override string FolderName { get { return base.GetType().Assembly.GetName().Name; } }
        public override string FormatType { get; } = "xml";
        public bool LoadMCMConfigFile { get; set; } = true;
    }
}
