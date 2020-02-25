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
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Forms;
using System.Drawing;
using Judgement.windows.match;
using System.IO;

namespace Judgement.windows.match
{
    /// <summary>
    /// settings.xaml 的交互逻辑
    /// </summary>
    public partial class settings : Page
    {
        string matchname,problem,problempath,settingpath;

        private void Spj_Click(object sender, RoutedEventArgs e)
        {
            if (spj.Content == "On")
                spj.Content = "Off";
            else
                spj.Content = "On";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //bug-need-fix
            File.WriteAllText(settingpath + "\\" + "type.txt",type.Text);
            File.WriteAllText(settingpath + "\\" + "SPJ.txt", Convert.ToString(spj.Content));
        }

        public settings()
        {
            InitializeComponent();
            //combobox初始化
            type.Items.Add("传统题");
            type.Items.Add("提交答案题");
            //名称初始化
            matchname = File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + "stuff" + "\\" + "matchname.txt");
            problem = File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + "match" + "\\" + matchname + "\\" + "on.txt");
            problempath = Directory.GetCurrentDirectory() + "\\" + "match" + "\\" + matchname + "\\" + "problem" + "\\" +problem;
            settingpath = problempath + "\\" + "settings";
            //控件初始化
            type.Text = File.ReadAllText(settingpath + "\\" + "type.txt");
            //bug:当初始化时，togglebutton永远没有开启
            spj.Content = File.ReadAllText(settingpath + "\\" + "SPJ.txt");
        }

    }
}
