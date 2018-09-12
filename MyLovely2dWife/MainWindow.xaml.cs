using L2DLib.Framework;
using L2DLib.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyLovely2dWife
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public Trigger Trigger { get; }
        public L2DModel Model => renderView.Model;

        Random rand = new Random();

        Timer idle_motion_timer;

        public MainWindow(Trigger trigger) 
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Trigger = trigger;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            renderView.Model = L2DFunctions.LoadModel(@"Pio\model.json");
            renderView.Model.UseBreath = true;

            Trigger.OnBreak += Trigger_OnBreak;
            Trigger.OnComboRankUp += Trigger_OnComboRankUp;
            Trigger.OnStatus += Trigger_OnStatus;

            idle_motion_timer = new Timer(OnIdleTimer, null, GetIdleNextTime(), Timeout.Infinite);
        }

        private int GetIdleNextTime() => (int)(30000 * (rand.NextDouble()+0.5));

        private void Trigger_OnStatus(bool is_listen)
        {
            if (is_listen)
            {
                ShowReturnMotion();
            }
        }

        private void Trigger_OnComboRankUp(int cur_combo_level)
        {
            ShowComboMotion();
        }

        private void Trigger_OnBreak(int combo_diff)
        {
            if (combo_diff>=100)
            {
                ShowBreakMotion();
            }
        }

        private void OnIdleTimer(object _)
        {
            ShowIdelMotion();

            //next 
            idle_motion_timer.Change(GetIdleNextTime(), Timeout.Infinite);
        }

        #region Show Motion

        private void StartMotion(string motion_name)
        {
            if (!Model.Motion.TryGetValue(motion_name, out var motions))
            {
                Log.Warn($"{motion_name} is not found,now model is idle.");
                return;
            }

            motions[rand.Next(motions.Length)].StartMotion();
            Log.Output($"Start {motion_name} motion.");
        }

        private void ShowBreakMotion()
        {
            StartMotion("fail");
        }

        public void ShowIdelMotion()
        {
            if (!renderView.UpdatedMotion)
                StartMotion("idle");
        }

        private void ShowReturnMotion()
        {
            StartMotion("sleep");
        }

        private void ShowComboMotion()
        {
            StartMotion("combo");
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Output("Not allow close,just hide");
            e.Cancel = true;
            Hide();
        }
    }
}
