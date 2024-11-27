using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TMcraft;
using System.Threading;
using static TMcraft.TMcraftShellAPI;
using RunProject;

namespace Shell2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TMcraftShellAPI ShellUI;
        private DispatcherTimer timer;
        private string str1 = "";
        private bool IsHoldLongEnough = false;

        int iCNT_ShutDown = 0;

        bool IsAutoMode = false;

        public MainWindow()
        {
            InitializeComponent();
            SetFullScreen();
            // Initialize and start the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
        }

        private void SetFullScreen()
        {
            // Set full screen
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Update the digital clock TextBlock with the current time
            int iTemp = 0;
            string[] strTempArray;
            uint uiTemp2 = 0;
            
            try
            {
                if (ShellUI.RobotStatusProvider != null)
                {
                    ShellUI.RobotStatusProvider.GetOperationMode(out iTemp);


                    switch (iTemp)
                    {
                        case 0:
                            btn_Mode.Content = "MANUAL";
                            IsAutoMode = false;
                            break;

                        case 1:
                            btn_Mode.Content = "AUTO";
                            IsAutoMode = true;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    ShowError("TMflow status retrieving failed");
                }

                if (ShellUI.ProjectRunProvider != null)
                {
                    ShellUI.ProjectRunProvider.GetDisplayBoardInfo(out str1);

                    DecodeStringtoArray(str1, out strTempArray);

                    label.Content = strTempArray[3];
                    ShowError("");
                }
                else
                {
                    ShowError("TMflow not connected");
                }
                
            }
            catch (Exception ex)
            {
                label_ErrorShow.Content = ex.ToString();
            }

            RobotStatusUpdate();

            ShutDownCount();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ShellUI == null)
            {
                ShellUI = new TMcraftShellAPI();
                ShellUI.InitialTMcraftShell();
            }
            timer.Start();
            UpdateProjectList();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e) { }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ShellUI.CloseShellConnection != null) ShellUI.CloseShellConnection();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //example : using TMcraft Shell API function to show TMflow and hide the Shell GUI
            try
            {
                if (ShellUI.SystemProvider != null)
                {
                    ShellUI.SystemProvider.ShowTMflow();
                    ShowError("");
                }
                else
                {
                    label_ErrorShow.Content = "TMflow not connected";
                    ShowError("TMflow not connected");
                }

            }
            catch (Exception ex)
            {
                label_ErrorShow.Content = ex.ToString();
            }
        }

        private void Window_Close()
        {
            if (ShellUI.CloseShellConnection != null) ShellUI.CloseShellConnection();
            this.Close();
        }

        private void btn_ProjectRun_Click(object sender, RoutedEventArgs e)
        {
            label.Content = "Project Run!";
            try
            {
                if ((ShellUI.ProjectRunProvider != null) && (comboBox_ProjectName.Text.ToString() != ""))
                {
                    ShellUI.ProjectRunProvider.SetCurrentProject(comboBox_ProjectName.Text.ToString());
                    Thread.Sleep(200);
                    ShellUI.ProjectRunProvider.RunProject();


                    ShowError("");
                }
                else
                {
                    ShowError("TMflow not connected");
                }
            }
            catch (Exception ex)
            {
                label_ErrorShow.Content = ex.ToString();
            }
        }

        private void btn_ProjectStop_Click(object sender, RoutedEventArgs e)
        {
            StopProject();
        }

        private void StopProject()
        {
            try
            {
                if (ShellUI.ProjectRunProvider != null)
                {
                    ShellUI.ProjectRunProvider.StopProject();
                    ShowError("");
                }
                else
                {
                    ShowError("TMflow not connected");
                }
            }
            catch (Exception ex)
            {
                label_ErrorShow.Content = ex.ToString();
            }
        }

        private void ShowError(string strInput)
        {
            label_ErrorShow.Content = strInput;
        }

        #region Mouse and touch event 
        
        private void btn_EXIT2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard animation = FindResource("Start_Animation") as Storyboard;
            if (animation != null)
            {
                (sender as FrameworkElement).BeginStoryboard(animation);
            }
            IsHoldLongEnough = true;
        }

        private void btn_EXIT2_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Storyboard animation = FindResource("Stop_Animation") as Storyboard;
            if (animation != null)
            {
                (sender as FrameworkElement).BeginStoryboard(animation);
            }
            IsHoldLongEnough = false;
            iCNT_ShutDown = 2;

        }

        private void btn_EXIT2_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            Storyboard animation = FindResource("Start_Animation") as Storyboard;
            if (animation != null)
            {
                (sender as FrameworkElement).BeginStoryboard(animation);
            }
            IsHoldLongEnough = true;
        }

        private void btn_EXIT2_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            Storyboard animation = FindResource("Stop_Animation") as Storyboard;
            if (animation != null)
            {
                (sender as FrameworkElement).BeginStoryboard(animation);
            }
            IsHoldLongEnough = false;
            iCNT_ShutDown = 2;
        }
        #endregion

        private void ProgressOn_Completed(object sender, EventArgs e)
        {
            if ((IsHoldLongEnough) && (iCNT_ShutDown == 0))
            {
                label.Content = "Exit!";
                StopProject();
                Thread.Sleep(2000);
                if ((ShellUI != null) && (ShellUI.SystemProvider != null))
                {
                    ShellUI.SystemProvider.Shutdown();
                }
                Window_Close();
            }
            else
            {
                label.Content = "";
            }
        }

        private void UpdateProjectList()
        {
            string strTemp = "";
            string[] strTempArray;

            comboBox_ProjectName.Items.Clear();

            if ((ShellUI != null) && (ShellUI.VariableProvider != null))
            {
                ShellUI.VariableProvider.GetGlobalVariableValue("g_MyProjectList", out strTemp);
                ShowError("");
            }
            else
            {
                ShowError("Could not get project list");
            }

            DecodeStringtoArray(strTemp, out strTempArray);

            foreach (var item in strTempArray)
            {
                if (item != "")
                {
                    comboBox_ProjectName.Items.Add(item);
                }
                comboBox_ProjectName.SelectedIndex = 0;
            }


        }

        private void btn_UpdateProject_Click(object sender, RoutedEventArgs e)
        {
            UpdateProjectList();
        }

        private void DecodeStringtoArray(string strInput, out string[] strOut)
        {
            //Remove '{' '}' '"' in array
            strOut = strInput.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("\"", "").Split(',').Select(s => s.Trim()).ToArray();
        }


        private void ShutDownCount()
        {
            //Make sure the button is held long enough before shutdown
            if ((iCNT_ShutDown > 0) && (IsHoldLongEnough))
            {
                iCNT_ShutDown--;
            }
            if (IsHoldLongEnough == false)
            {
                iCNT_ShutDown = 2;
            }
        }
        private void RobotStatusUpdate()
        {
            bool bProjectRun = false;
            bool bRobotError = false;
            bool bRobotESTOP = false;

            var runDotStoryboard = (Storyboard)FindResource("RunDot");


            if ((ShellUI != null) && (ShellUI.RobotStatusProvider != null))
            {
                ShowError("");

                #region Project run check
                ShellUI.RobotStatusProvider.ProjectRunOrNot(out bProjectRun);
                if (bProjectRun)
                {
                    btn_RobotStatus.Content = "RUNNING";
                    if (runDotStoryboard != null)
                    {
                        runDotStoryboard.Resume();
                    }

                    if(IsAutoMode == false)
                    {
                        ShowError("[Waring] Robot is in MANUAL mode, could not change to different project. Please switch to AUTO mode if you want to change project.");
                    }

                }
                else
                {
                    btn_RobotStatus.Content = "STOPPED";
                    if (runDotStoryboard != null)
                    {
                        runDotStoryboard.Pause();
                    }

                }
                #endregion

                #region Robot error check
                ShellUI.RobotStatusProvider.RobotErrorOrNot(out bRobotError);
                ShellUI.RobotStatusProvider.RobotEstopOrNot(out bRobotESTOP);
                if (bRobotError || bRobotESTOP)
                {
                    btn_RobotStatusError.Visibility = Visibility.Visible; 

                    ShowError("Robot error, please go to TMflow for more information");
                }
                else 
                {
                    btn_RobotStatusError.Visibility = Visibility.Hidden;
                }
                #endregion

            }
            else
            {
                ShowError("Robot status update failed");
            }


        }


        private void btn_Mode_Click(object sender, RoutedEventArgs e)
        {
            int iTemp = 0;
            if (ShellUI.RobotStatusProvider != null)
            {
                ShellUI.RobotStatusProvider.GetOperationMode(out iTemp);
                
            }
            else
            {
                ShowError("TMflow status retrieving failed");
            }

            ShellUI.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MALongKey);
            Thread.Sleep(100);

            ShellUI.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.PlusKey);
            Thread.Sleep(100);
            ShellUI.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MinusKey);
            Thread.Sleep(100);
            ShellUI.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.PlusKey);
            Thread.Sleep(100);
            ShellUI.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.PlusKey);
            Thread.Sleep(100);
            ShellUI.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MinusKey);
            Thread.Sleep(100);

            ShellUI.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MAKey);
            Thread.Sleep(100);

            ShellUI.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MAKey);
            Thread.Sleep(100);
        }

        private void btn_LoginAndGetControl_Click(object sender, RoutedEventArgs e)
        {
            GetControl();
        }

        private void GetControl()
        {
            ShellUI.SystemProvider.LogIn("Administrator", "");
            Thread.Sleep(100);
            ShellUI.SystemProvider.GetControl(true);
        }

        private void btn_Prompt_Click(object sender, RoutedEventArgs e)
        {
            Window_Prompt objWindowPrompt = new Window_Prompt();
            objWindowPrompt.Show();
        }


    }
}
