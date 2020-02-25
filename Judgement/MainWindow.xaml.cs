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
using MahApps.Metro.Controls;
using MahApps.Metro;
using MahApps.Metro.IconPacks;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using Judgement.windows.match;
using Judgement.windows.systemsettings;
using Judgement.windows.thanks;

namespace Judgement
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public string matchselected;
        public string matchpath,settingpath,stuffpath;
        public MainWindow()
        {
            InitializeComponent();
            //路径检索与创建
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + "match");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + "run");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + "settings");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + "stuff");
            matchpath = Directory.GetCurrentDirectory() + "\\" + "match";
            settingpath = Directory.GetCurrentDirectory() + "\\" + "settings";
            stuffpath = Directory.GetCurrentDirectory() + "\\" + "stuff";
            Refresh_matchlist();
            //样式改变
            Change_style();
        }

        private void Addmatch_Click(object sender, RoutedEventArgs e)
        {
            //新建比赛
            AddmatchAsync();
        }

        private async Task AddmatchAsync()
        {
            string addname=null;
            //对话框
            addname=await this.ShowInputAsync("新建比赛","比赛名称：");
            if(addname!=null)
            {
                //比赛目录创建
                Directory.CreateDirectory(matchpath+"\\"+addname);
                string nowpath = matchpath + "\\" + addname;
                Directory.CreateDirectory(nowpath + "\\" + "problem");
                Directory.CreateDirectory(nowpath + "\\" + "player");
                Directory.CreateDirectory(nowpath + "\\" + "score");
                //刷新列表
                Refresh_matchlist();
            }
        }

        private void Deleteallmatch_Click(object sender, RoutedEventArgs e)
        {
            Directory.Delete(matchpath, true);
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + "match");
            Refresh_matchlist();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Window1 ssshow = new Window1();
            this.Close();
            ssshow.Show();
        }

        private void Thank_Click(object sender, RoutedEventArgs e)
        {
            thank tkshow = new thank();
            this.Close();
            tkshow.Show();
        }

        private void Matchlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选中比赛存档
            matchselected = matchlist.SelectedItem.ToString();
            File.WriteAllText(stuffpath + "\\" + "matchname.txt", matchselected);
            //窗口切换
            matchsettings msshow = new matchsettings();
            this.Close();
            msshow.Show();
        }

        private void Refresh_matchlist()
        {
            matchlist.Items.Clear();
            DirectoryInfo TheFolder = new DirectoryInfo(matchpath);
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
                matchlist.Items.Add(NextFolder.Name);
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