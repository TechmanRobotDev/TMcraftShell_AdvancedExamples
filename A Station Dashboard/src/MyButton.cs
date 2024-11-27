using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HDProcess
{
    /// <summary>
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    ///要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:HDProcess"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    ///要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:HDProcess;assembly=HDProcess"
    ///
    /// 您還必須將 XAML 檔所在專案的專案參考加入
    /// 此專案並重建，以免發生編譯錯誤: 
    ///
    ///     在 [方案總管] 中以滑鼠右鍵按一下目標專案，並按一下
    ///     [加入參考]->[專案]->[瀏覽並選取此專案]
    ///
    ///
    /// 步驟 2)
    /// 開始使用 XAML 檔案中的控制項。
    ///
    ///     <MyNamespace:MyButton/>
    ///
    /// </summary>
    public class MyButton : Button
    {
        public string IconNormalUri
        {
            get { return (string)GetValue(IconNormalUriProperty); }
            set { SetValue(IconNormalUriProperty, value); }
        }

        public static readonly DependencyProperty IconNormalUriProperty =
            DependencyProperty.Register("IconNormalUri", typeof(string), typeof(MyButton), new UIPropertyMetadata(string.Empty,
              (o, e) =>
              {
                  try
                  {
                      Uri uriSource = new Uri((string)e.NewValue, UriKind.RelativeOrAbsolute);
                      if (uriSource != null)
                      {
                          var button = o as MyButton;
                          BitmapImage img = new BitmapImage(uriSource);
                          button.SetValue(IconNormalProperty, img);
                      }
                  }
                  catch //(Exception ex)
                  {
                      throw;
                  }
              }));

        public BitmapImage IconNormal
        {
            get { return (BitmapImage)GetValue(IconNormalProperty); }
            set { SetValue(IconNormalProperty, value); }
        }

        public static readonly DependencyProperty IconNormalProperty =
            DependencyProperty.Register("IconNormal", typeof(BitmapImage), typeof(MyButton), new UIPropertyMetadata(null));

        public string IconPressedUri
        {
            get { return (string)GetValue(IconPressedUriProperty); }
            set
            {
                SetValue(IconPressedUriProperty, value);
            }
        }
        public static readonly DependencyProperty IconPressedUriProperty =
            DependencyProperty.Register("IconPressedUri", typeof(string), typeof(MyButton), new UIPropertyMetadata(string.Empty,
              (o, e) =>
              {
                  try
                  {
                      Uri uriSource = new Uri((string)e.NewValue, UriKind.RelativeOrAbsolute);
                      if (uriSource != null)
                      {
                          var button = o as MyButton;
                          BitmapImage img = new BitmapImage(uriSource);
                          button.SetValue(IconPressedProperty, img);
                      }
                  }
                  catch //(Exception ex)
                  {
                      throw;
                  }
              }));

        public BitmapImage IconPressed
        {
            get { return (BitmapImage)GetValue(IconPressedProperty); }
            set { SetValue(IconPressedProperty, value); }
        }
        public static readonly DependencyProperty IconPressedProperty =
            DependencyProperty.Register("IconPressed", typeof(BitmapImage), typeof(MyButton), new UIPropertyMetadata(null));

        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register(
                "IconVisibility",
                typeof(Visibility),
                typeof(MyButton),
                new FrameworkPropertyMetadata(Visibility.Collapsed) { BindsTwoWayByDefault = true }
                );

        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }


        static MyButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyButton), new FrameworkPropertyMetadata(typeof(MyButton)));
        }
    }
}
