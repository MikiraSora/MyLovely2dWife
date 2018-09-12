using OsuRTDataProvider;
using System;
using static OsuRTDataProvider.Listen.OsuListenerManager;

namespace MyLovely2dWife
{
    /// <summary>
    /// 专门触发数值
    /// </summary>
    public class Trigger
    {
        public OsuRTDataProviderPlugin OrtdpPlugin { get; }

        public event Action<int> OnBreak;

        /// <summary>
        /// 指30/60/100/150/200
        /// </summary>
        public event Action<int> OnComboRankUp;

        /// <summary>
        /// bool: IsListen
        /// </summary>
        public event Action<bool> OnStatus;

        public Trigger(OsuRTDataProviderPlugin ortdp_plugin)
        {
            OrtdpPlugin = ortdp_plugin;

            ortdp_plugin.ListenerManager.OnComboChanged += OnComboChanged;
            ortdp_plugin.ListenerManager.OnStatusChanged += OnStatusChanged;
        }

        private void OnStatusChanged(OsuStatus last_status, OsuStatus status)
        {
            //clean
            prev_combo = combo_level = 0;

            if (last_status == OsuStatus.Rank && (status == OsuStatus.SelectSong || status == OsuStatus.MatchSetup))
            {
                OnStatus?.Invoke(true);
            }

            if (status == OsuStatus.Rank && (last_status == OsuStatus.SelectSong || last_status == OsuStatus.MatchSetup))
            {
                OnStatus?.Invoke(false);
            }
        }

        private int prev_combo = 0;
        private int combo_level = 0;
        private const int COMBO_RANK_DEFAULT = 100;

        private void OnComboChanged(int combo)
        {
            if (combo == prev_combo)
                return;

            if (combo < prev_combo)
            {
                combo_level = 0;
                OnBreak?.Invoke(Math.Abs(combo - prev_combo));
            }
            else
            {
                if (combo >= (combo_level + 1) * COMBO_RANK_DEFAULT)
                {
                    combo_level++;
                    OnComboRankUp?.Invoke(combo_level);
                }
            }

            prev_combo = combo;
        }
    }
}