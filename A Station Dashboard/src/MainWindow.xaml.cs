using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TMcraft;
using System.Globalization;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json;
//using ctrl_vision;
using System.Diagnostics;

namespace HDProcess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Project Info

        private readonly string projectName = "ShellSampleProject";
        private readonly string loginAccount = "Administrator";
        private readonly string loginPassword = "";

        #endregion

        #region  Variables

        private string sScrewStage = "";
        private string sScrewProg = "";
        private string sScrewTimes = "";
        private string sTorque = "";
        private string sTorqueUnit = "";
        private string sScrewCounter = "";
        private string sGlobalYield = "";
        private string sYield = "";

        private string sOK = "";
        private string sGlobalOK = "";
        private string sNG = "";
        private string sGlobalNG = "";
        private string sResult = "";

        private string sNo = "";
        private string sID = "";
        private string sJointCover = "";
        private string sHD = "";
        private string sKey = "";

        //Toggle Button String
        private enum ScrewStageEnum { HD, FG };

        #endregion

        #region Basic variables

        private UserControlPD pdPage = new UserControlPD();    // Production Page
        private UserControlPF pfPage = new UserControlPF();    // Performance Page

        public TMcraftShellAPI TMShellEditor;    //API

        private DispatcherTimer timer;    //Timer

        uint message = (uint)TMcraftErr.OK;    //API Error Message

        //private CtrlVision ctrlVision;    //Vision

        #region Color def.

        Color BgLightRed = Color.FromRgb(255, 123, 124);
        Color BgLightGreen = Color.FromRgb(168, 227, 93);
        Color BgLightBlue = Color.FromRgb(134, 197, 228);
        Color BgLightYellow = Color.FromRgb(250, 222, 103);
        Color BgLightBlack = Color.FromRgb(0, 0, 0);
        Color BgLightWhite = Color.FromRgb(255, 255, 255);
        Color BgLightGray = Color.FromRgb(199, 199, 199);
        //Color DefaultGray = Color.FromRgb(68, 68, 68);
        Color LigthEnable = Color.FromRgb(119, 183, 31);
        Color LightDisabled = Color.FromRgb(0x95, 0x95, 0x95);

        #endregion

        private bool isFirstTime = true;    // Is it the first time to start


        #endregion

        public MainWindow()
        {
            InitializeComponent();

            ShowBorder();

            DataContext = this;

            user_bar.PlayButtonClicked += UCBarPlayBtn_Click;
            user_bar.PauseButtonClicked += UCBarPauseBtn_Click;
            user_bar.StopButtonClicked += UCBarStopBtn_Click;

            pdPage.LockStatusBtnChecked += UCPDLockStatusBtn_Checked;
            pdPage.LockStatusBtnUnchecked += UCPDLockStatusBtn_Unchecked;

            pdPage.Txt_sNoTextChanged += UCPDtxt_sNo_TextChanged;
            pdPage.Txt_sIDTextChanged += UCPDtxt_sID_TextChanged;
            pdPage.Txt_sJointCoverTextChanged += UCPDtxt_sJointCover_TextChanged;
            pdPage.Txt_sHDTextChanged += UCPDtxt_sHD_TextChanged;
            pdPage.Txt_sKeyTextChanged += UCPDtxt_sKey_TextChanged;

            //ctrlVision = new CtrlVision();
            //pdPage.camera.Children.Add(ctrlVision);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += InitSettings;
            timer.Tick += GetVariablesList;

        }

        [Conditional("DEBUG")]
        private void ShowBorder()
        {
            WindowStyle = WindowStyle.ThreeDBorderWindow;
        }

        private static void Logger(string log)
        {
            DateTime dt = DateTime.Now;
            string logFilePath = string.Format("./shell_{0}.log", dt.ToString("yyyyMMdd"));
            string logMsg = string.Format("[{0}] | [{1}]", dt.ToString("yyyy-MM-dd HH:mm:ss.fff"), log);

            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(logMsg);
            }
        }

        private void ShowErrorMsg(string eventName, uint msgNum = 0)
        {
            if (TMShellEditor != null)
            {
                string tmflowMessage = "";
                TMcraftErr mcraftErr = TMShellEditor.GetErrMsg(msgNum, out tmflowMessage);
                string msg = string.Format("Message: {0}! {1}", eventName, tmflowMessage);
                Logger(msg);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TMShellEditor == null)
            {
                TMShellEditor = new TMcraftShellAPI();
                TMShellEditor.InitialTMcraftShell();
            }

            if (TMShellEditor == null)
            {
                Logger("Message: InitialTMcraftShell, Initialization Failed");
            }

            InputLanguageManager.SetInputLanguage(pdPage.txt_sNo, CultureInfo.CreateSpecificCulture("en-US"));
            InputLanguageManager.SetInputLanguage(pdPage.txt_sID, CultureInfo.CreateSpecificCulture("en-US"));
            InputLanguageManager.SetInputLanguage(pdPage.txt_sJointCover, CultureInfo.CreateSpecificCulture("en-US"));
            InputLanguageManager.SetInputLanguage(pdPage.txt_sHD, CultureInfo.CreateSpecificCulture("en-US"));
            InputLanguageManager.SetInputLanguage(pdPage.txt_sKey, CultureInfo.CreateSpecificCulture("en-US"));

            timer.Start();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (TMShellEditor != null)
            {
                TMShellEditor.CloseShellConnection();
            }

        }


        #region  Predecessor Tasks
        private void LogInInBackground()
        {
            //pdPage.txt_errInfo.Text += string.Format(">>>> {0} Logging in >>>>\n", loginAccount);
            try
            {
                if (TMShellEditor != null)
                {
                    message = TMShellEditor.SystemProvider.LogIn(loginAccount, loginPassword);
                }
            }
            catch
            {
            }
            if ((message != 0) && (message != 4026532065))  //logged in:4026532065
            {
                ShowErrorMsg("LogIn", message);
            }
            //else
            //{
            //    pdPage.txt_errInfo.Text += string.Format("[{0} Logged In]\n\n", loginAccount);
            //}
        }

        private void GetControlInBackground()
        {
            //pdPage.txt_errInfo.Text += ">>>> GetControl >>>>\n";
            try
            {
                if (TMShellEditor != null)
                {
                    message = TMShellEditor.SystemProvider.GetControl();
                }
            }
            catch
            {
            }
            if (message != 0)
            {
                ShowErrorMsg("GetControl", message);
            }
            //else
            //{
            //    pdPage.txt_errInfo.Text += "[Got Control]\n\n";
            //}
        }

        private void SetAutoModeInBackground()
        {
            //pdPage.txt_errInfo.Text += ">>>> Set Auto Mode >>>>\n";
            int mode = 0;
            try
            {
                if (TMShellEditor != null)
                {
                    message = TMShellEditor.RobotStatusProvider.GetOperationMode(out mode); //Auto:1 Manual:0
                    if (mode != 1)
                    {
                        TMShellEditor.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MALongKey);
                        Thread.Sleep(500);
                        TMShellEditor.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.PlusKey);
                        Thread.Sleep(500);
                        TMShellEditor.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MinusKey);
                        Thread.Sleep(500);
                        TMShellEditor.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.PlusKey);
                        Thread.Sleep(500);
                        TMShellEditor.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.PlusKey);
                        Thread.Sleep(500);
                        TMShellEditor.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MinusKey);
                        Thread.Sleep(500);
                        TMShellEditor.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MAKey);
                        Thread.Sleep(500);
                        TMShellEditor.RobotStickProvider.RobotVirtualStickKeyEvent(VirtualKeyEvent.MAKey);
                        Thread.Sleep(500);
                    }
                    //else
                    //{
                    //    pdPage.txt_errInfo.Text += "[Set Auto Mode]\n\n";
                    //    return;
                    //}
                    message = TMShellEditor.RobotStatusProvider.GetOperationMode(out mode); //Auto:1 Manual:0
                }
            }
            catch
            {
            }
            if (mode == 0)
            {
                Logger("Message: SetAutoMode, Setting Failed");
            }
        }

        private void SetProjectInBackground()
        {
            //pdPage.txt_errInfo.Text += string.Format(">>>> Set project to {0} >>>>\n",projectName);
            string currentProject = "";
            try
            {
                if (TMShellEditor != null)
                {
                    message = TMShellEditor.ProjectRunProvider.SetCurrentProject(projectName);
                }
            }
            catch
            {
            }
            if (message != 0 && message != 262184)  //Project is running:262184
            {
                string txt = string.Format("Set Project to {0}", projectName);
                ShowErrorMsg(txt, message);
            }

            if (TMShellEditor != null)
            {
                Thread.Sleep(500);
                message = TMShellEditor.ProjectRunProvider.GetCurrentProject(out currentProject);

            }
            if (message != 0)
            {
                ShowErrorMsg("Get Current Project", message);
            }
            txt_title.Text = currentProject.ToString();

            //if (currentProject.Equals(projectName))
            //{
            //    pdPage.txt_errInfo.Text += string.Format("[Set project to {0}]\n\n", projectName);
            //}
        }

        #endregion


        #region  Timer Event
        /// <summary>
        /// When the Shell starts, it automatically logs in, connects and sets up the specified Project.
        /// </summary>
        private void InitSettings(object sender, EventArgs e)
        {
            if (isFirstTime)
            {
                isFirstTime = false;
                LogInInBackground();
                GetControlInBackground();
                SetAutoModeInBackground();
                SetProjectInBackground();
            }
        }


        /// <summary>
        /// This function is used to update the currently displayed value.
        /// </summary>
        private void GetVariablesList(object sender, EventArgs e)
        {
            try
            {
                if (TMShellEditor != null)
                {
                    // input box value
                    sNo = GetValue("var_sNo");
                    sID = GetValue("var_sID");
                    sJointCover = GetValue("var_sJointCover");
                    sHD = GetValue("var_sHD");
                    sKey = GetValue("var_sKey");  //new

                    // Production Page get value
                    sScrewStage = GetValue("var_sScrewStage");
                    sScrewProg = GetValue("var_sScrewProg");
                    sScrewTimes = GetValue("var_sScrewTimes");
                    sTorque = GetValue("var_sTorque");
                    sTorqueUnit = GetValue("var_sTorqueUnit");

                    // Performance Page get value
                    sScrewCounter = GetValue("var_sScrewCounter");
                    sYield = GetValue("var_sYield");
                    sGlobalYield = GetValue("g_gfYield", false);

                    sResult = GetValue("var_sResult");
                    sOK = GetValue("var_sOK");
                    sNG = GetValue("var_sNG");
                    sGlobalOK = GetValue("g_gfOK", false);
                    sGlobalNG = GetValue("g_gfNG", false);

                    // DisplayInfo
                    message = TMShellEditor.ProjectRunProvider.GetDisplayBoardInfo(out string displayInfo);
                    //ShowErrorMsg(message, "displayInfo");

                    // Speed
                    message = TMShellEditor.RobotStatusProvider.GetCurrentSpeedPercentage(out float speed);
                    message = TMShellEditor.RobotStatusProvider.GetCurrentToolSpeed(out string toolSpeed);
                    //ShowErrorMsg(message, "GetCurrentToolSpeed");

                    // Robot Status
                    message = TMShellEditor.RobotStatusProvider.RobotErrorOrNot(out bool errorStatus);
                    message = TMShellEditor.RobotStatusProvider.RobotEstopOrNot(out bool estopStatus);

                    if (errorStatus == false && estopStatus == false)
                    {
                        LightAuto.Fill = new SolidColorBrush(LigthEnable);
                    }
                    else
                    {
                        LightAuto.Fill = new SolidColorBrush(LightDisabled);
                        user_bar.ChangeBarStatus(true, true, true);
                    }

                    //title
                    message = TMShellEditor.ProjectRunProvider.GetCurrentProject(out string currentProject);
                    txt_title.Text = currentProject.ToString();

                    //RunOrNot
                    message = TMShellEditor.RobotStatusProvider.ProjectRunOrNot(out bool isRun);
                    message = TMShellEditor.RobotStatusProvider.ProjectPauseOrNot(out bool isPause);
                    if (isRun)
                    {
                        if (isPause)
                        {
                            user_bar.ChangeBarStatus(true, false, true);
                        }
                        else
                        {
                            user_bar.ChangeBarStatus(false, true, true);
                        }
                    }
                    else
                    {
                        user_bar.ChangeBarStatus(true, true, false);
                        return;
                    }

                    Dispatcher.Invoke(() =>
                    {
                        // Show correct values at TxetBox
                        ShowInputPartValue(pdPage.txt_sNo, sNo);
                        ShowInputPartValue(pdPage.txt_sID, sID);
                        ShowInputPartValue(pdPage.txt_sJointCover, sJointCover);
                        ShowInputPartValue(pdPage.txt_sHD, sHD);
                        ShowInputPartValue(pdPage.txt_sKey, sKey);

                        // Check correct values at TxetBox
                        CompareInputPartValue(pdPage.txt_sNo, sNo);
                        CompareInputPartValue(pdPage.txt_sID, sID);
                        CompareInputPartValue(pdPage.txt_sJointCover, sJointCover);
                        CompareInputPartValue(pdPage.txt_sHD, sHD);
                        CompareInputPartValue(pdPage.txt_sKey, sKey);


                        // Production Page update
                        if (sScrewStage == "\"\"")
                        {
                            //If the value of sScrewStage in TMflow is null, Shell will pass value("HD") ​​to TMflow.
                            message = TMShellEditor.VariableProvider.ChangeVariableRuntimeValue("var_sScrewStage", ScrewStageEnum.HD.ToString());
                        }
                        UpdateToggleBtn(pdPage.LockStatusBtn, sScrewStage, ScrewStageEnum.FG.ToString());
                        UpdateValue(pdPage.txt_sScrewProg, sScrewProg);

                        UpdateValue(pdPage.txt_sScrewTimes, sScrewTimes);
                        UpdateValue(pdPage.txt_sTorque, sTorque);
                        UpdateValue(pdPage.txt_sTorqueUnit, sTorqueUnit);

                        if (displayInfo != "null")
                        {
                            UpdateValue(pdPage.txt_info, displayInfo, true);
                        }

                        // Performance Page update
                        UpdateValue(pfPage.txt_sScrewCounter, sScrewCounter);
                        UpdateValue(pfPage.txt_sYield, sYield);
                        UpdateValue(pfPage.txt_sGlobalYield, sGlobalYield);

                        UpdateValue(pfPage.txt_sResult, sResult);
                        UpdateValue(pfPage.txt_sOK, sOK);
                        UpdateValue(pfPage.txt_sNG, sNG);
                        UpdateValue(pfPage.txt_sGlobalNG, sGlobalNG);
                        UpdateValue(pfPage.txt_sGlobalOK, sGlobalOK);


                        //speed update
                        string speedTxt = string.Format("{0} %", speed);
                        UpdateValue(txt_robotForce, speedTxt);
                        string toolSpeedTxt = string.Format("{0} mm/s", toolSpeed);
                        UpdateValue(txt_tcpSpeed, toolSpeedTxt);
                    });

                }
            }
            catch
            {

            }
        }

        #endregion


        #region  Function
        /// <summary>
        /// This function get the value of a global variable or a project variable from TMflow.
        /// </summary>
        /// <param name="varName">The name of the variable in the TMflows</param>
        /// <param name="isRuntime">Is runtime value</param>
        /// <returns>Value</returns>
        private string GetValue(string varName, bool isRuntime = true)
        {
            if (TMShellEditor != null)
            {
                if (isRuntime)
                {
                    message = TMShellEditor.VariableProvider.GetVariableRuntimeValue(varName, out var result);
                    return result.ToString();
                }
                else
                {
                    message = TMShellEditor.VariableProvider.GetGlobalVariableValue(varName, out var result);
                    return result.ToString();
                }
            }
            return "";
        }


        /// <summary>
        /// This function checks if the content of a TextBox contains an incorrectly entered value. 
        /// If an incorrect value is found, it changes the Border of the TextBox to red for visual indication.
        /// </summary>
        private static void CompareInputPartValue(TextBox displayTextbox, string runtimeValue)
        {
            if (displayTextbox.Text == runtimeValue.Replace("\"", ""))
            {
                displayTextbox.BorderBrush = Brushes.Gray;
            }
            else
            {
                displayTextbox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// This function checks whether the content of a TextBox contains an incorrectly entered value. 
        /// If an incorrect value is detected, it automatically corrects the TextBox to the appropriate value.
        /// </summary>
        private void ShowInputPartValue(TextBox displayTextbox, string runtimeValue)
        {
            if (displayTextbox.Text == "" && !displayTextbox.IsFocused)
            {
                UpdateValue(displayTextbox, runtimeValue);
            }
            if (displayTextbox.Text != runtimeValue.Replace("\"", "") && !displayTextbox.IsFocused)
            {
                UpdateValue(displayTextbox, runtimeValue);
            }
        }

        /// <summary>
        ///  This function is used to update the value displayed in a TextBlock.
        /// </summary>
        private static void UpdateValue(TextBlock displayTextblock, string getValue)
        {
            if (getValue != null)
            {
                displayTextblock.Text = getValue.Replace("\"", "");
            }
        }

        /// <summary>
        /// This function is used to update the value displayed in the Textbox.
        /// </summary>
        private void UpdateValue(TextBox displayTextbox, string getValue, bool isBoardInfo = false)
        {
            //LightAuto.Fill = new SolidColorBrush(LigthEnable);
            if (getValue != null)
            {
                if (isBoardInfo)
                {
                    try
                    {
                        List<string> strList = JsonConvert.DeserializeObject<List<string>>(getValue);
                        //getValue = string.Format("Title:{0}\nContent:{1}", strList[2], strList[3]);
                        getValue = strList[2];
                        // Change the background color
                        switch (strList[0])
                        {
                            case "Red":
                                pdPage.scroll_viewer.Background = new SolidColorBrush(BgLightRed);
                                break;
                            case "Green":
                                pdPage.scroll_viewer.Background = new SolidColorBrush(BgLightGreen);
                                break;
                            case "Blue":
                                pdPage.scroll_viewer.Background = new SolidColorBrush(BgLightBlue);
                                break;
                            case "Yellow":
                                pdPage.scroll_viewer.Background = new SolidColorBrush(BgLightYellow);
                                break;
                            case "Black":
                                pdPage.scroll_viewer.Background = new SolidColorBrush(BgLightBlack);
                                break;
                            case "White":
                                pdPage.scroll_viewer.Background = new SolidColorBrush(BgLightWhite);
                                break;
                            case "Gray":
                                pdPage.scroll_viewer.Background = new SolidColorBrush(BgLightGray);
                                break;
                            default:
                                pdPage.scroll_viewer.Background = new SolidColorBrush(BgLightWhite);
                                break;
                        }
                        // Change the text color
                        //switch (strList[1])
                        //{
                        //    case "Red":
                        //        pdPage.txt_info.Foreground = new SolidColorBrush(BgLightRed);
                        //        break;
                        //    case "Green":
                        //        pdPage.txt_info.Foreground = new SolidColorBrush(BgLightGreen);
                        //        break;
                        //    case "Blue":
                        //        pdPage.txt_info.Foreground = new SolidColorBrush(BgLightBlue);
                        //        break;
                        //    case "Yellow":
                        //        pdPage.txt_info.Foreground = new SolidColorBrush(BgLightYellow);
                        //        break;
                        //    case "Black":
                        //        pdPage.txt_info.Foreground = new SolidColorBrush(BgLightBlack);
                        //        break;
                        //    case "White":
                        //        pdPage.txt_info.Foreground = new SolidColorBrush(BgLightWhite);
                        //        break;
                        //    case "Gray":
                        //        pdPage.txt_info.Foreground = new SolidColorBrush(BgLightGray);
                        //        break;
                        //    default:
                        //        pdPage.txt_info.Foreground = new SolidColorBrush(DefaultGray);
                        //        break;
                        //}
                    }
                    catch { }
                    displayTextbox.Text = getValue;
                }
                else
                {
                    displayTextbox.Text = getValue.Replace("\"", "");
                }
            }

        }

        /// <summary>
        ///  This function is used to update the value displayed in the Toggle Button
        /// </summary>
        private static void UpdateToggleBtn(ToggleButton toggleBtn, string getValue, string checkedValue)
        {
            if (getValue != null)
            {
                getValue = getValue.ToUpper();
                toggleBtn.IsChecked = getValue.Contains(checkedValue); // Check whether getValue includes the IsChecked Value of ToggleBtn.
            }
        }
        #endregion


        #region MainWindow Button Events

        /// <summary>
        /// This function is used to go back to TMflow216
        /// </summary>
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (TMShellEditor != null)
            {
                message = TMShellEditor.SystemProvider.ShowTMflow();
            }
        }

        private void ProductionButton_Click(object sender, RoutedEventArgs e)
        {
            UserContent = pdPage;
            ProductionButton.IsEnabled = false;
            PerformanceButton.IsEnabled = true;
        }

        private void PerformanceButton_Click(object sender, RoutedEventArgs e)
        {
            UserContent = pfPage;
            ProductionButton.IsEnabled = true;
            PerformanceButton.IsEnabled = false;
        }

        #endregion



        #region  UCBar Events

        public void UCBarPlayBtn_Click(object sender, EventArgs e)
        {
            if (TMShellEditor != null)
            {
                timer.Start();

                message = TMShellEditor.ProjectRunProvider.RunProject();
                if (message != 0)
                {
                    ShowErrorMsg("RunProject", message);
                }
                else
                {
                    user_bar.ChangeBarStatus(false, true, true);
                }

                pdPage.txt_sNo.Focus();
            }
        }

        public void UCBarStopBtn_Click(object sender, EventArgs e)
        {
            bool isRun = false;
            if (TMShellEditor != null)
            {
                message = TMShellEditor.RobotStatusProvider.ProjectRunOrNot(out isRun);
                if (isRun)
                {
                    message = TMShellEditor.ProjectRunProvider.StopProject();
                    //timer.Stop();
                    if (message != 0)
                    {
                        ShowErrorMsg("StopProject", message);
                    }
                    else
                    {
                        user_bar.ChangeBarStatus(true, true, false);
                    }
                }
            }
        }

        public void UCBarPauseBtn_Click(object sender, EventArgs e)
        {
            bool isRun = false;
            if (TMShellEditor != null)
            {
                message = TMShellEditor.RobotStatusProvider.ProjectRunOrNot(out isRun);
                if (isRun)
                {
                    message = TMShellEditor.ProjectRunProvider.PauseProject();
                    if (message != 0)
                    {
                        ShowErrorMsg("PauseProject", message);
                    }
                    else
                    {
                        user_bar.ChangeBarStatus(true, false, true);
                    }
                    pdPage.txt_sNo.Focus();
                }
            }
        }

        #endregion


        #region  Production UCPD Events

        public void UCPDLockStatusBtn_Checked(object sender, EventArgs e)
        {
            //FG
            if (TMShellEditor != null)
            {
                message = TMShellEditor.VariableProvider.ChangeVariableRuntimeValue("var_sScrewStage", ScrewStageEnum.FG.ToString());
            }
        }

        public void UCPDLockStatusBtn_Unchecked(object sender, EventArgs e)
        {
            //HD
            if (TMShellEditor != null)
            {
                message = TMShellEditor.VariableProvider.ChangeVariableRuntimeValue("var_sScrewStage", ScrewStageEnum.HD.ToString());
            }
        }


        public void UCPDtxt_sNo_TextChanged(object sender, EventArgs e)
        {
            if (TMShellEditor != null)
            {
                string sNo_value = pdPage.txt_sNo.Text.Trim();
                message = TMShellEditor.VariableProvider.ChangeVariableRuntimeValue("var_sNo", sNo_value);
                if (message != 0)
                {
                    ShowErrorMsg("TextChanged: sNo", message);
                }
            }
        }


        public void UCPDtxt_sID_TextChanged(object sender, EventArgs e)
        {
            if (TMShellEditor != null)
            {
                string sID_value = pdPage.txt_sID.Text.Trim();
                message = TMShellEditor.VariableProvider.ChangeVariableRuntimeValue("var_sID", sID_value);
                if (message != 0)
                {
                    ShowErrorMsg("TextChanged: sID", message);
                }
            }
        }


        public void UCPDtxt_sJointCover_TextChanged(object sender, EventArgs e)
        {
            if (TMShellEditor != null)
            {
                string sJointCover_value = pdPage.txt_sJointCover.Text.Trim();
                message = TMShellEditor.VariableProvider.ChangeVariableRuntimeValue("var_sJointCover", sJointCover_value);
                if (message != 0)
                {
                    ShowErrorMsg("TextChanged: JointCover", message);
                }
            }
        }


        public void UCPDtxt_sHD_TextChanged(object sender, EventArgs e)
        {
            if (TMShellEditor != null)
            {
                string sHD_value = pdPage.txt_sHD.Text.Trim();
                message = TMShellEditor.VariableProvider.ChangeVariableRuntimeValue("var_sHD", sHD_value);
                if (message != 0)
                {
                    ShowErrorMsg("TextChanged: sHD", message);
                }
            }
        }

        public void UCPDtxt_sKey_TextChanged(object sender, EventArgs e)    //new
        {
            if (TMShellEditor != null)
            {
                string sKey_value = pdPage.txt_sKey.Text.Trim();
                message = TMShellEditor.VariableProvider.ChangeVariableRuntimeValue("var_sKey", sKey_value);
                if (message != 0)
                {
                    ShowErrorMsg("TextChanged: sKey", message);
                }
            }
        }

        #endregion


        #region Switch Content

        private UserControl _content;

        public UserControl UserContent
        {
            get
            {
                if (_content != null)
                    return _content;
                else
                {
                    ProductionButton.IsChecked = true;     // Default page
                    return pdPage;
                }
            }
            set
            {
                _content = value;
                OnPropertyChanged("UserContent");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }


}
