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
using Judgement;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Forms;
using System.Drawing;
using Judgement.windows.match;
using System.IO;

namespace Judgement.windows.systemsettings
{
    /// <summary>
    /// style.xaml 的交互逻辑
    /// </summary>
    public partial class style : Page
    {
        string settingpath;
        public style()
        {
            InitializeComponent();
            //加载颜色
            Initialize_Color();
            settingpath = Directory.GetCurrentDirectory() + "\\" + "settings";
            //控件加载
            theme.Content = File.ReadAllText(settingpath + "\\" + "theme.txt");
            color.Text= File.ReadAllText(settingpath + "\\" + "color.txt");
        }

        private void Theme_Click(object sender, RoutedEventArgs e)
        {
            if (theme.Content == "On")
                theme.Content = "Off";
            else
                theme.Content = "On";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //写入文件
            File.WriteAllText(settingpath + "\\" + "theme.txt", Convert.ToString(theme.Content));
            File.WriteAllText(settingpath + "\\" + "color.txt", color.Text);
            Change_style();
        }

        private void Change_style()
        {
            string theme = File.ReadAllText(settingpath + "\\" + "theme.txt");
            if (theme == "On")
                theme = "BaseDark";
            else
                theme = "BaseLight";
            string color = File.ReadAllText(settingpath + "\\" + "color.txt");
            Window1 win = new Window1();
            ThemeManager.ChangeAppStyle(win, ThemeManager.GetAccent(color), ThemeManager.GetAppTheme(theme));
        }

        private void Initialize_Color()
        {
            color.Items.Add("Red");
            color.Items.Add("Green");
            color.Items.Add("Blue");
            color.Items.Add("Purple");
            color.Items.Add("Orange");
            color.Items.Add("Lime");
            color.Items.Add("Emerald");
            color.Items.Add("Teal");
            color.Items.Add("Cyan");
            color.Items.Add("Cobalt");
            color.Items.Add("Indigo");
            color.Items.Add("Violet");
            color.Items.Add("Pink");
            color.Items.Add("Magenta");
        }

    }
}
