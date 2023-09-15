using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using JetpackSelector.Mod;

namespace JetpackSelector
{
    [Category("jetpack")]
    public class JetpackSelectorCommands : CommandModule
    {
        [Command("on", "Enables a player jetpack\nLeave playerName empty for acting on your self!")]
        [Permission(MyPromoteLevel.Admin)]
        public void EnableJetpack(string playerName = null)
        {
            if (!TryGetPlayer(ref playerName, out IMyPlayer player))
            {
                Context.Respond($"Player '{playerName}' not found! Check that the name is correct!");
                return;
            }

            ulong steamID = player.SteamUserId;
            JetpackSelector.Instance.PlayersJetpackState[steamID] = true;
            Context.Respond($"Enabling jetpack for player '{playerName}'");
        }

        [Command("off", "Disable a player jetpack\nLeave playerName empty for acting on your self!")]
        [Permission(MyPromoteLevel.Admin)]
        public void DisableJetpack(string playerName = null)
        {
            if (!TryGetPlayer(ref playerName, out IMyPlayer player))
            {
                Context.Respond($"Player '{playerName}' not found! Check that the name is correct!");
                return;
            }

            ulong steamID = player.SteamUserId;
            JetpackSelector.Instance.PlayersJetpackState[steamID] = false;
            PluginComunication.SendDisableJetpackCommand(steamID);
            Context.Respond($"Disabling jetpack for player '{playerName}'");
        }

        [Command("switch", "Switches a player jetpack\nLeave playerName empty for acting on your self!")]
        [Permission(MyPromoteLevel.Admin)]
        public void ToggleJetpack(string playerName = null)
        {
            if (!TryGetPlayer(ref playerName, out IMyPlayer player))
            {
                Context.Respond($"Player '{playerName}' not found! Check that the name is correct!");
                return;
            }

            ulong steamID = player.SteamUserId;
            JetpackSelector.Instance.PlayersJetpackState[steamID] = !JetpackSelector.Instance.PlayersJetpackState[steamID];
            if (!JetpackSelector.Instance.PlayersJetpackState[steamID])
                PluginComunication.SendDisableJetpackCommand(steamID);
            Context.Respond($"Switching jetpack for player '{playerName}'");
        }

        private bool TryGetPlayer(ref string playerName, out IMyPlayer player)
        {
            if (string.IsNullOrEmpty(playerName))
                playerName = Context.Player.DisplayName;

            player = Context.Torch.CurrentSession.KeenSession.Players.GetPlayerByName(playerName);
            if (player == null)
                return false;

            return true;
        }
    }
}
