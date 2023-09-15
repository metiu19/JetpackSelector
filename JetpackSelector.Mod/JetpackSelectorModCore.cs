using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Components;
using VRage.Utils;

namespace JetpackSelector.Mod
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class JetpackSelectorModCore : MySessionComponentBase
    {
        public static JetpackSelectorModCore Instance;
        private bool init = false;


        public override void LoadData()
        {
            Instance = this;
        }

        protected override void UnloadData()
        {
            try
            {
                Instance = null;
                MyAPIGateway.Utilities.MessageEntered -= MessageEntered;
                PluginComunication.Unregister();
            }
            catch {}
        }

        public override void UpdateBeforeSimulation()
        {
            if (init)
                return;

            init = true;
            MyAPIGateway.Utilities.MessageEntered += MessageEntered;
            PluginComunication.Register();
        }

        public void MessageEntered(string messageText, ref bool sendToOthers)
        {
            MyLog.Default.WriteLineAndConsole($"Jetpack Selector: Message recieved!\n{messageText}");
        }
    }
}
