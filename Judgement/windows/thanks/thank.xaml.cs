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
using Judgement.windows.thanks;

namespace Judgement.windows.thanks
{
    /// <summary>
    /// thank.xaml 的交互逻辑
    /// </summary>
    public partial class thank : MetroWindow
    {
        string settingpath;
        public thank()
        {
            InitializeComponent();
            settingpath = Directory.GetCurrentDirectory() + "\\" + "settings";
            Change_style();
            //感谢名单：
            list.Items.Add("还有更多...");
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
