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
    /// data.xaml 的交互逻辑
    /// </summary>
    public partial class data : Page
    {
        string matchname, problem, problempath, settingpath, datapath;
        public data()
        {
            InitializeComponent();
            //路径初始化
            matchname = File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + "stuff" + "\\" + "matchname.txt");
            problem = File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + "match" + "\\" + matchname + "\\" + "on.txt");
            problempath = Directory.GetCurrentDirectory() + "\\" + "match" + "\\" + matchname + "\\" + "problem" + "\\" + problem;
            settingpath = problempath + "\\" + "settings";
            datapath = problempath + "\\" + "testdata";
            //控件初始化
            score.Text = File.ReadAllText(settingpath + "\\" + "score.txt");
            datanum.Content = File.ReadAllText(settingpath + "\\" + "datanum.txt");
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            int i=1,count=0;
            //遍历文件夹
            while(1==1)
            {
                FileInfo testdatain = new FileInfo(datapath + "\\" + problem + i + ".in");
                FileInfo testdataans = new FileInfo(datapath + "\\" + problem + i + ".ans");
                if (testdataans.Exists)
                {
                    if (File.ReadAllText(settingpath + "\\" + "type.txt") == "传统题")
                        if (!testdatain.Exists)
                            break;
                    i++;
                    count++;
                }
                else
                    break;
            }
            File.WriteAllText(settingpath + "\\" + "datanum.txt", Convert.ToString(count));
            datanum.Content = File.ReadAllText(settingpath + "\\" + "datanum.txt");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(settingpath + "\\" + "score.txt", Convert.ToString(score.Text));
        }
    }
}
