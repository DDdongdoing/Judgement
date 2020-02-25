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
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using Judgement.windows.match;

namespace Judgement.windows.systemsettings
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : MetroWindow
    {
        string settingpath;

        public Window1()
        {   
            InitializeComponent();
            settingpath = Directory.GetCurrentDirectory() + "\\" + "settings";
            //设置列表初始化
            settingslist.Items.Add("样式");
            //样式切换
            Change_style();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            backtomainwindow();
        }

        

        private void backtomainwindow()
        {
            MainWindow windowshow = new MainWindow();
            this.Close();
            windowshow.Show();
        }

        private void Settingslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string setting = settingslist.SelectedItem.ToString();
            Page settingpage = null;
            switch (setting)//新增功能接口
            {
                case "样式":
                    settingpage = new style();
                    break;
            }
            //切换
            settings_show.Content = new Frame()
            {
                Content = settingpage
            };
        }

        private void Change_style()
        {
            string theme = File.ReadAllText(settingpath + "\\" + "theme.txt");
            if (theme == "On")
                theme = "BaseDark";
            else
                theme = "BaseLight";
            string color = File.ReadAllText(settingpath + "\\" + "color.txt");
            ThemeManager.ChangeAppStyle(this, ThemeManager.GetAccent(color), ThemeManager.GetAppTheme(theme));
        }
    }
}
