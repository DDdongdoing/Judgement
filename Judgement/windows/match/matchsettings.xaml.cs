using System;
using System.Collections.Generic;
using System.IO;
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
using Judgement;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Forms;
using System.Drawing;
using Judgement.windows.match;
using Judgement.windows.Judge;
using Judgement.windows.judge_history;

namespace Judgement.windows.match
{
    /// <summary>
    /// matchsettings.xaml 的交互逻辑
    /// </summary>
    public partial class matchsettings : MetroWindow
    {
        public string matchpath, settingpath, stuffpath, problempath,setting;
        int qaq=0,sel=0;

        public matchsettings()
        {
            InitializeComponent();
            //路径初始化
            stuffpath = Directory.GetCurrentDirectory() + "\\" + "stuff";
            Matchname.Content = File.ReadAllText(stuffpath + "\\" + "matchname.txt");
            matchpath = Directory.GetCurrentDirectory() + "\\" + "match"+ "\\" + Matchname.Content;
            settingpath = Directory.GetCurrentDirectory() + "\\" + "settings";
            File.WriteAllText(matchpath + "\\" + "on.txt", "未选中题目");
            //菜单初始化
            settingslist.Items.Add("题目管理");
            settingslist.Items.Add("数据点配置");
            //题目列表初始化
            Refresh_problemlist();
            //改变样式
            Change_style();
        }

        private void Addproblems_Click(object sender, RoutedEventArgs e)
        {
            AddproblemAsync();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            backtomainwindow();
        }



        private void Deletematch_Click(object sender, RoutedEventArgs e)
        {
            Directory.Delete(matchpath, true);
            backtomainwindow();
        }

        private void Deleteproblems_Click(object sender, RoutedEventArgs e)
        {
            //删除
            string problemselected = problemlist.SelectedItem.ToString();
            settings_show.Content = null;
            Directory.Delete(matchpath + "\\" + "problem" + "\\" + problemselected, true);
            int n = problemlist.Items.Count;        
            Refresh_problemlist();
        }

        private void Openpath_Click(object sender, RoutedEventArgs e)
        {
            //打开
            System.Diagnostics.Process.Start("explorer.exe", Directory.GetCurrentDirectory() + "\\" + "match");
        }

        private void Startjudge_Click(object sender, RoutedEventArgs e)
        {
            //开始评测
            judging windowshow = new judging();
            this.Close();
            windowshow.Show();

        }

        private void Judgehistory_Click(object sender, RoutedEventArgs e)
        {
            //评测历史
            Judge_History windowshow = new Judge_History();
            windowshow.Show();
        }

        private void Problemlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选中的题目
            if(qaq==0)//没在删
            {
                File.WriteAllText(matchpath + "\\" + "on.txt", problemlist.SelectedItem.ToString());
                Change_window();
            }
        }

        private void Settingslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //切换设置
        {
            sel = 1;
            Change_window();
        }

        private async Task AddproblemAsync()
        {
            string addname = null;
            //对话框
            addname = await this.ShowInputAsync("新建题目", "题目名称(最终用于评测)：");
            if (addname != null)
            {
                //创建题目目录
                problempath = matchpath + "\\" + "problem" + "\\" + addname;
                Directory.CreateDirectory(problempath);
                Directory.CreateDirectory(problempath + "\\" + "Special Judge");
                Directory.CreateDirectory(problempath + "\\" + "testdata");
                Directory.CreateDirectory(problempath + "\\" + "settings");
                //创建设置文件
                //题目
                File.WriteAllText(problempath + "\\" + "settings" + "\\" + "type.txt","传统题");
                File.WriteAllText(problempath + "\\" + "settings" + "\\" + "SPJ.txt", "Off");
                //测试点
                File.WriteAllText(problempath + "\\" + "settings" + "\\" + "time.txt", "0");
                File.WriteAllText(problempath + "\\" + "settings" + "\\" + "memory.txt", "0");
                File.WriteAllText(problempath + "\\" + "settings" + "\\" + "score.txt", "0");
                File.WriteAllText(problempath + "\\" + "settings" + "\\" + "datanum.txt", "0");
                //链表添加
                problemlist.Items.Add(addname);
                Refresh_problemlist();
            }
        }

        private void Refresh_problemlist()
        {
            qaq = 1;
            problemlist.Items.Clear();
            DirectoryInfo TheFolder = new DirectoryInfo(matchpath + "\\" + "problem");
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
                problemlist.Items.Add(NextFolder.Name);
            File.WriteAllText(matchpath + "\\" + "on.txt", "未选中题目");
            qaq = 0;

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

        private void Change_window()
        {
            //获取列表
            if (File.ReadAllText(matchpath + "\\" + "on.txt") != "未选中题目" && sel==1)
            {
                setting = settingslist.SelectedItem.ToString();
                Page settingpage = null;
                switch (setting)
                {
                    case "题目管理":
                        settingpage = new settings();
                        break;
                    case "数据点配置":
                        settingpage = new data();
                        break;
                }
                //切换
                settings_show.Content = new Frame()
                {
                    Content = settingpage
                };
            }
        }
    }
}
