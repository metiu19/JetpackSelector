using Sandbox.ModAPI;
using VRage.Utils;
using VRage.Game.ModAPI;

namespace JetpackSelector.Mod
{
    public static class PluginComunication
    {
        private const ushort netID = 1901;
        private static bool closing;

        public static void Register()
        {
            MyLog.Default.WriteLineAndConsole("Jetpack Selector: Registering mod communication.");

            MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(netID, MessageHandler);
            closing = false;

            MyLog.Default.WriteLineAndConsole("Jetpack Selector: Mod communication registered successfully.");
        }

        public static void Unregister()
        {
            MyLog.Default.WriteLineAndConsole("Jetpack Selector: Unregistering mod communication.");
            MyAPIGateway.Multiplayer?.UnregisterSecureMessageHandler(netID, MessageHandler);
            closing = true;
        }

        public static void MessageHandler(ushort netID, byte[] binaryData, ulong senderStemaID, bool reliable)
        {
            if (closing)
                return;

            IMyCharacter character = MyAPIGateway.Session.LocalHumanPlayer?.Character;
            if (character == null || !character.IsPlayer)
                return;

            if (character.EnabledThrusts)
                character.SwitchThrusts();
        }

        public static void SendDisableJetpackCommand(ulong playerSteamID)
        {
            MyAPIGateway.Multiplayer.SendMessageTo(netID, new byte[0], playerSteamID);
        }
    }
}
