using BetterCore.Utils;
using HarmonyLib;
using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using WarAndAiTweaks.FortificationChanges;

namespace WarAndAiTweaks {

    public class WarAndAiTweaks : MBSubModuleBase {
        public static MCMSettings Settings { get; private set; }
        public static string ModName { get; private set; } = "WarAndAiTweaks";

        private bool isInitialized = false;
        private bool isLoaded = false;

        //FIRST
        protected override void OnSubModuleLoad() {
            try {
                base.OnSubModuleLoad();

                if (isInitialized)
                    return;

                Harmony h = new("Bannerlord.Windwhistle." + ModName);

                h.PatchAll();

                isInitialized = true;
            } catch (Exception e) {
                NotifyHelper.ReportError(ModName, "OnSubModuleLoad threw exception " + e);
            }
        }

        //SECOND
        protected override void OnBeforeInitialModuleScreenSetAsRoot() {
            try {
                base.OnBeforeInitialModuleScreenSetAsRoot();

                if (isLoaded)
                    return;
                ModName = base.GetType().Assembly.GetName().Name;

                Settings = MCMSettings.Instance ?? throw new NullReferenceException("Settings are null");

                NotifyHelper.ChatMessage(ModName + " Loaded.", MsgType.Good);
                Integrations.BetterHealthLoaded = true;

                isLoaded = true;
            } catch (Exception e) {
                NotifyHelper.ReportError(ModName, "OnBeforeInitialModuleScreenSetAsRoot threw exception " + e);
            }
        }
        protected override void OnGameStart(Game game, IGameStarter starterObject) {
            base.OnGameStart(game, starterObject);
            if (Settings.PartySpeedModifications)
                starterObject.AddModel(new CustomPartySpeedCalculatingModel());
        }
    }
}