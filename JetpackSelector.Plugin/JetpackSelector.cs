using System.Collections.Generic;
using NLog;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.Session;

namespace JetpackSelector
{
    public class JetpackSelector : TorchPluginBase
    {
        public static Logger Log = LogManager.GetCurrentClassLogger();
        public static JetpackSelector Instance;
        public Dictionary<ulong, bool> PlayersJetpackState;
        public const ulong ModID = 3035578446UL;

        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            IsReloadable = true;
            Instance = this;

            PlayersJetpackState = new Dictionary<ulong, bool>();

            var sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (sessionManager != null)
                sessionManager.AddOverrideMod(ModID);
            else
                Log.Warn("No session manager loaded!");
        }

        public override void Dispose()
        {
            Instance = null;
            base.Dispose();
        }
    }
}
