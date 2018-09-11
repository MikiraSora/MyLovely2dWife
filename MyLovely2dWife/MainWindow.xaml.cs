using L2DLib.Utility;
using System.Collections.Generic;
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
        public MainWindow() 
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            renderView.Model = L2DFunctions.LoadModel(@"Pio\model.json");
            renderView.Model.UseBreath = true;
        }

        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (!renderView.Model.Motion.TryGetValue((e as SelectionChangedEventArgs).AddedItems[0].ToString(), out var motions))
                return;
            motions[0].StartMotion();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Output("Not allow close,just hide");
            e.Cancel = true;
            Hide();
        }
    }
}
