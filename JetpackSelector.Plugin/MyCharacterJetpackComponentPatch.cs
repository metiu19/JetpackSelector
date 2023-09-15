using System;
using System.Reflection;
using NLog;
using Torch.Managers.PatchManager;
using Sandbox.Game.Entities.Character.Components;
using JetpackSelector.Mod;

namespace JetpackSelector
{
    [PatchShim]
    public static class MyCharacterJetpackComponentPatch
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private static MethodInfo turnOnJetpack =
            typeof(MyCharacterJetpackComponent).GetMethod(nameof(MyCharacterJetpackComponent.TurnOnJetpack), BindingFlags.Instance | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        private static MethodInfo turnOnJetpackPrefix =
            typeof(MyCharacterJetpackComponentPatch).GetMethod(nameof(MyCharacterJetpackComponentPatch.TurnOnJetpackPrefix), BindingFlags.Static | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        public static void TurnOnJetpackPrefix(MyCharacterJetpackComponent __instance, ref bool newState)
        {
            var steamID = __instance?.Character?.ControlSteamId ?? 12345UL;
            if (steamID == 12345UL)
                return;

            if (!JetpackSelector.Instance.PlayersJetpackState.TryGetValue(steamID, out bool jetpackEnabled))
            {
                Log.Info($"Player {steamID} not present in state list, using default!");
                JetpackSelector.Instance.PlayersJetpackState.Add(steamID, false);
            }

            if (newState && !jetpackEnabled)
                PluginComunication.SendDisableJetpackCommand(steamID);
        }

        public static void Patch(PatchContext ctx)
        {
            ctx.GetPattern(turnOnJetpack).Prefixes.Add(turnOnJetpackPrefix);
            Log.Info("Patching Successful MyCharacterJetpackComponent!");
        }
    }
}
