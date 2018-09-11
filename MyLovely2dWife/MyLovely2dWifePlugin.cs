using Sync.Command;
using Sync.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyLovely2dWife
{
    public class MyLovely2dWifePlugin : Plugin
    {
        public MainWindow window { get;private set; }
        private Application application;

        public MyLovely2dWifePlugin() : base("MyLovely2dWifePlugin","MikiraSora")
        {
            EventBus.BindEvent<PluginEvents.LoadCompleteEvent>(OnLoadComplete);
            EventBus.BindEvent<PluginEvents.InitCommandEvent>(OnInitCommand);
        }

        private void OnLoadComplete(PluginEvents.LoadCompleteEvent e)
        {

        }

        private void OnInitCommand(PluginEvents.InitCommandEvent e)
        {
            e.Commands.Dispatch.bind("wife", OnCommand, "MyLovely2dWifePlugin's command");
        }

        private bool OnCommand(Arguments args)
        {
            switch (args.FirstOrDefault()??string.Empty)
            {
                case "show":
                    ShowWindow();
                    break;

                case "hide":
                    HideWindow();
                    break;

                default:
                    ShowHelp();
                    return false;
            }

            return true;
        }

        private void ShowHelp()
        {

        }

        private void ShowWindow()
        {
            Application.Current?.Dispatcher.InvokeAsync(() => {
                window = window ?? new MainWindow();
                window.Show();
                Log.Output("Show window");
            });
        }

        private void HideWindow()
        {
            Application.Current?.Dispatcher.Invoke(() => {
                if (window?.Visibility==Visibility.Visible)
                    window.Hide();
                Log.Output("Hide window");
            });
        }
    }
}
