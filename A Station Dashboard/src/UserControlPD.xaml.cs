using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HDProcess
{
    /// <summary>
    /// UserControlPD.xaml 的互動邏輯
    /// </summary>
    public partial class UserControlPD : UserControl
    {
        public UserControlPD()
        {
            InitializeComponent();
        }

        public event EventHandler LockStatusBtnChecked;
        public event EventHandler LockStatusBtnUnchecked;

        public event EventHandler Txt_sNoTextChanged;
        public event EventHandler Txt_sIDTextChanged;
        public event EventHandler Txt_sJointCoverTextChanged;
        public event EventHandler Txt_sHDTextChanged;
        public event EventHandler Txt_sKeyTextChanged;

        private void LockStatusBtn_Checked(object sender, RoutedEventArgs e)
        {
            //FG
            LockStatusBtnChecked.Invoke(this, EventArgs.Empty);
        }

        private void LockStatusBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            //HD
            LockStatusBtnUnchecked.Invoke(this, EventArgs.Empty);
        }

        #region Text Changed
        private void Txt_sNo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                Txt_sNoTextChanged.Invoke(this, EventArgs.Empty);
                txt_sID.Focus();

            }
        }

        private void Txt_sID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Txt_sIDTextChanged.Invoke(this, EventArgs.Empty);
                txt_sJointCover.Focus();
            }
        }

        private void Txt_sJointCover_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                Txt_sJointCoverTextChanged.Invoke(this, EventArgs.Empty);
                txt_sHD.Focus();
            }

        }

        private void Txt_sHD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Txt_sHDTextChanged.Invoke(this, EventArgs.Empty);
                txt_sKey.Focus();
            }

        }

        private void Txt_sKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Txt_sKeyTextChanged.Invoke(this, EventArgs.Empty);
                txt_sNo.Focus();
            }

        }

        #endregion


        #region Focus
        private void Txt_sNo_Loaded(object sender, RoutedEventArgs e)
        {
            txt_sNo.Focus();
        }

        private void Txt_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private void Txt_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                textBox.Focus();
                textBox.SelectAll();
                e.Handled = false;
            }
        }

        #endregion



    }


}
