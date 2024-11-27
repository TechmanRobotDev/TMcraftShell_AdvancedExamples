using System;
using System.Windows;
using System.Windows.Controls;

namespace HDProcess
{
    /// <summary>
    /// UserControlBar.xaml 的互動邏輯
    /// </summary>
    public partial class UserControlBar : UserControl
    {

        public UserControlBar()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event EventHandler PlayButtonClicked;
        public event EventHandler PauseButtonClicked;
        public event EventHandler StopButtonClicked;

        public void ChangeBarStatus(bool playBtnStatus, bool pauseBtnStatus, bool stopBtnStatus)
        {
            PlayBtn.IsEnabled = playBtnStatus;
            PauseBtn.IsEnabled = pauseBtnStatus;
            StopBtn.IsEnabled = stopBtnStatus;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayButtonClicked.Invoke(this, EventArgs.Empty);

        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            PauseButtonClicked.Invoke(this, EventArgs.Empty);
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StopButtonClicked.Invoke(this, EventArgs.Empty);
        }
    }
}
