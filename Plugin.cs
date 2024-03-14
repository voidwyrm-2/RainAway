using System;
using BepInEx;
using ImprovedInput;
using UnityEngine;

namespace NuclearPasta.RainAway
{
    [BepInDependency("com.dual.improved-input-config", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("nc.rainaway", "Rain Away!", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {

        public static bool debug = true;

        public static bool isenabled = true;
        public static string status = "enabled";

        public static bool isStory = false;
        public static int cycleLength = 0;
        public static int cycleLimit = 0;
        public static float cCycleTime = 0;
        public static bool reset = false;

        public static PlayerKeybind ToggleRainAway;

        public static bool ImprovedInputEnabled;

        public static bool isInit;

        // Add hooks
        public void OnEnable()
        {
            try
            {
                ToggleRainAway = PlayerKeybind.Register("rainaway:togglerainaway", "Rain Away", "Rain Away toggle", KeyCode.Alpha0, KeyCode.JoystickButton3);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            On.RainWorld.PostModsInit += RainWorld_PostModsInit;
            //On.World.ctor += World_ctor;
            On.Player.Update += Player_Update;
            On.GlobalRain.Update += GlobalRain_Update;
            On.RainWorld.OnModsInit += RainWorld_LoadOptions;
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
            PauseMenuText.SetupHooks();
        }

        private void RainWorld_LoadOptions(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            MachineConnector.SetRegisteredOI("nc.rainaway", new Options());
        }

        private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            try
            {
                if (!isInit)
                {
                    isInit = true;
                    ToggleRainAway.Description = "The key pressed to toggle Rain Away";
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            finally
            {
                orig.Invoke(self);
            }
        }

        private void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            ImprovedInputEnabled = ModManager.ActiveMods.Exists((ModManager.Mod mod) => mod.id == "com.dual.improved-input-config");
            if (!ImprovedInputEnabled)
            {
                Logger.LogDebug("no mod with id \"com.dual.improved-input-config\" found, continuing without");
            }
            orig(self);
        }

        private void GlobalRain_Update(On.GlobalRain.orig_Update orig, GlobalRain self)
        {
            if (reset) self.ResetRain(); reset = false;
            orig(self);
        }

        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {

            if (isenabled)
            {
                status = "enabled";
            }
            else
            {
                status = "disabled";
            }

            if (ToggleRainAway.CheckRawPressed(0) && isenabled)
            {
                isenabled = false;
            }
            else if (ToggleRainAway.CheckRawPressed(0) && !isenabled)
            {
                isenabled = true;
            }

            cycleLimit = cycleLength - (cycleLength / 2);
            if (cCycleTime < cycleLimit && isenabled) reset = true;
            if (debug)
            {
                string m1 = "isStory:" + isStory.ToString(); Logger.LogInfo(m1);
                string m2 = "cycleLength:" + cycleLength.ToString(); Logger.LogInfo(m2);
                string m3 = "cycleLimit:" + cycleLimit.ToString(); Logger.LogInfo(m3);
                string m4 = "cCycleTime:" + cCycleTime.ToString(); Logger.LogInfo(m4);
                string m5 = "reset:" + reset.ToString(); Logger.LogInfo(m5);
            }
            try
            {
                isStory = self.room.game.IsStorySession;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            try
            {
                cycleLength = self.room.world.rainCycle.cycleLength;
                cCycleTime = self.room.world.rainCycle.AmountLeft * cycleLength;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            orig(self, eu);
        }

        /*
        private void World_ctor(On.World.orig_ctor orig, World self, RainWorldGame game, Region region, string name, bool singleRoomWorld)
        {
            try
            {
                cycleLength = self.rainCycle.cycleLength;
                cCycleTime = (int) self.rainCycle.AmountLeft;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            
            orig(self, game, region, name, singleRoomWorld);
        }
        */
    }
}