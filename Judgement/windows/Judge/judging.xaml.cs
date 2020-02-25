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
using System.Diagnostics;
using System.Security.Cryptography;

namespace Judgement.windows.Judge
{
    /// <summary>
    /// judging.xaml 的交互逻辑
    /// </summary>
    public partial class judging : MetroWindow
    {
        //最硬核的部分，评测机的灵魂，VS之神保佑

        private string matchname, matchpath,problempath, playerpath, scorepath,settingpath, s_problem,runpath;
        int problemnum = 0, playernum = 0;//题目个数、选手个数，读取行数

        public judging()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Action handler = new Action(this.LoadData);
            IAsyncResult result = handler.BeginInvoke(new AsyncCallback(this.LoadDataComplete), 1);
        }

        private void LoadData()
        {
            //初始化
            Initialize();
            //目录扫描
            scan();
            //开始评测
            judge();
        }

        private void LoadDataComplete(IAsyncResult result)
        {
            this.BeginInvoke(new Action(this.show_button_info));
        }

        private void show_button_info()
        {
            back.Content = "评测完成，确认退出";
            back.IsEnabled = true;
        }

        private void clear()
        {
            //进度条清零
            pathload.Value = 0;
            judgeload.Value = 0;
        }

        private void load_Initialize()
        {
            //路径进度条初始化
            pathload.Maximum = playernum;
            pathload.Minimum = 0;
            judgeload.Maximum = playernum * problemnum;
            judgeload.Minimum = 0;
        }

        private void judge_load_plus()
        {
            judgeload.Value++;
        }

        private void path_load_plus()
        {
            pathload.Value++;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            matchsettings windowshow = new matchsettings();
            this.Close();
            windowshow.Show();
        }

        private delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);

        public void Initialize()
        {
            //更换主题
            this.BeginInvoke(new Action(this.Change_style));
            //路径定义
            matchname = File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + "stuff" + "\\" + "matchname.txt");
            matchpath = Directory.GetCurrentDirectory() + "\\" + "match" + "\\" + matchname ;
            problempath = matchpath + "\\" + "problem";
            playerpath = matchpath + "\\" + "player";
            scorepath = matchpath + "\\" + "score";
            settingpath = Directory.GetCurrentDirectory() + "\\" + "settings";
            s_problem = scorepath + "\\" + "problem";
            runpath = Directory.GetCurrentDirectory();
            //清空score文件
            if (Directory.Exists(scorepath))
                Directory.Delete(scorepath, true);
            Directory.CreateDirectory(scorepath);
            Directory.CreateDirectory(s_problem);
            //创建score文件(用create进程会导致后期无法调用)
            File.WriteAllText(scorepath + "\\" + "problemsname.txt",null);
            File.WriteAllText(scorepath + "\\" + "players.txt", null);
            File.WriteAllText(scorepath + "\\" + "error.txt", null);
            this.BeginInvoke(new Action(this.clear));
        }

        private void scan()
        {
            //创建score文件的filestream与StreamReader、StreamWriter
            FileStream pbnames = new FileStream(scorepath + "\\" + "problemsname.txt", FileMode.OpenOrCreate);
            FileStream players = new FileStream(scorepath + "\\" + "players.txt", FileMode.OpenOrCreate);
            FileStream error = new FileStream(scorepath + "\\" + "error.txt", FileMode.OpenOrCreate);

            StreamReader pbnamesread = new StreamReader(pbnames);
            StreamWriter pbnameswrite = new StreamWriter(pbnames);

            StreamReader playersread = new StreamReader(players);
            StreamWriter playerswrite = new StreamWriter(players);

            StreamReader errorread = new StreamReader(error);
            StreamWriter errorwrite = new StreamWriter(error);

            //获取题目个数与名称并添加文件
            DirectoryInfo test = new DirectoryInfo(problempath);
            DirectoryInfo[] test1 = test.GetDirectories();
            foreach(DirectoryInfo dri in test1)
            {
                problemnum++;
                pbnameswrite.WriteLine(dri.Name);
                File.WriteAllText(s_problem + "\\" + dri.Name + ".txt", null);
            }
            pbnameswrite.Close();
            //获取选手个数与名称并添加文件
            DirectoryInfo test3 = new DirectoryInfo(playerpath);
            DirectoryInfo[] test4 = test3.GetDirectories();
            foreach(DirectoryInfo dri in test4)
            {
                playernum++;
                playerswrite.WriteLine(dri.Name);
            }
            playerswrite.Close();
            //路径进度条初始化
            this.BeginInvoke(new Action(this.load_Initialize));
            //检查各个选手的文件夹是否完整
            foreach (DirectoryInfo dri in test4)
            {
                bool lost = false;
                string check = playerpath + "\\" + dri.Name;
                foreach (DirectoryInfo dri2 in test1)
                {
                    if (!Directory.Exists(check + "\\" + dri2.Name))
                    {
                        errorwrite.WriteLine(true);
                        lost = true;
                        break;
                    }
                    this.BeginInvoke(new Action(this.path_load_plus));
                }
                if (lost!=true)
                    errorwrite.WriteLine(false);
            }
            errorwrite.Close();
        }

        private void judge()
        {
            DirectoryInfo test = new DirectoryInfo(problempath);
            DirectoryInfo[] test1 = test.GetDirectories();
            foreach (DirectoryInfo problemname in test1)
            {
                //定义+初始化变量
                string now_problempath = problempath + "\\" + problemname.Name;
                string now_testdatapath = now_problempath + "\\" + "testdata";
                string now_settingpath = now_problempath + "\\" + "settings";
                string now_type = File.ReadAllText(now_settingpath + "\\" + "type.txt");
                string now_problemname = problemname.Name;

                //获取题目类型并选择评测类型
                if (now_type == "传统题")
                    traditional_judge(now_testdatapath,now_settingpath,now_problemname);
                else
                    ans_judge(now_testdatapath, now_settingpath,now_problemname);
            }
        }

        private void traditional_judge(string testdatapath,string settingpath,string name)
        {
            //初始化reader与writer
            FileStream error = new FileStream(scorepath + "\\" + "error.txt", FileMode.OpenOrCreate);
            StreamReader errorread = new StreamReader(error);
            //初始化路径
            DirectoryInfo DI_playerpath = new DirectoryInfo(playerpath);
            DirectoryInfo[] test2 = DI_playerpath.GetDirectories();
            //针对每个选手的评测
            foreach (DirectoryInfo i in test2)
            {
                //根据error情况决定是否评测
                string error_ = errorread.ReadLine();
                if (error_ == "False")
                {
                    //初始化路径
                    string playername = i.Name;
                    string now_playerpath = playerpath + "\\" + playername + "\\" + name;
                    string programpath = now_playerpath + "\\" + name + ".cpp";
                    //检查有无程序文件
                    if (File.Exists(programpath))
                    {
                        //移动
                        File.Copy(programpath, runpath + "\\" + name + ".cpp");
                        string now_programpath = runpath + "\\" + name + ".cpp";
                        string now_exepath = runpath + "\\" + name + ".exe";
                        //开始编译
                        int exitcode;
                        Process compileProcess = new Process();

                        //无内嵌版本：
                       // compileProcess.StartInfo.FileName = "g++.exe";
                        //内嵌版本:
                        compileProcess.StartInfo.FileName = Directory.GetCurrentDirectory()+ "//"+"TDM-GCC"+"//"+"bin"+"//"+"g++.exe";
                        compileProcess.StartInfo.Arguments = now_programpath + " -o " + now_exepath;
                        compileProcess.StartInfo.UseShellExecute = false;
                        compileProcess.StartInfo.RedirectStandardInput = true;
                        compileProcess.StartInfo.RedirectStandardOutput = true;
                        compileProcess.StartInfo.RedirectStandardError = true;
                        compileProcess.StartInfo.CreateNoWindow = true;
                        compileProcess.Start();
                        compileProcess.WaitForExit();
                        exitcode = compileProcess.ExitCode;
                        compileProcess.Close();
                        if (0 != exitcode)//编译成功的正常返回值为0，不为0说明CE，跳过该选手评测
                            score_judge(0, name);
                        else
                        {
                            DirectoryInfo DI_testdatapath = new DirectoryInfo(testdatapath);
                            FileInfo[] test3 = DI_testdatapath.GetFiles("*.ans",SearchOption.TopDirectoryOnly);
                            int j = 0;//数据点个数
                            int program_score = 0;//本题得分
                            foreach(FileInfo testdatanum in test3)
                            {
                                j++;
                                //文件拷贝并重命名
                                File.Copy(testdatapath + "\\" + name + Convert.ToString(j) + ".in", runpath + "\\" + name + ".in",true);
                                //开始执行
                                Process exeProcess = new Process();
                                exeProcess.StartInfo.FileName = now_exepath;
                                exeProcess.StartInfo.UseShellExecute = false;
                                exeProcess.StartInfo.RedirectStandardError = true;
                                exeProcess.StartInfo.CreateNoWindow = true;
                                exeProcess.Start();
                                exeProcess.WaitForExit();
                                exitcode = exeProcess.ExitCode;
                                exeProcess.Close();
                                //检测输出文件是否存在
                                if(File.Exists(Directory.GetCurrentDirectory()+"\\"+name+".out"))
                                {
                                    //移动文件
                                    File.Move(Directory.GetCurrentDirectory() + "\\" + name + ".out", runpath + "\\" + name + ".out");
                                    File.Copy(testdatapath + "\\" + name + Convert.ToString(j) + ".ans", runpath + "\\" + name + ".ans", true);
                                    //比较函数
                                    program_score += compare(runpath,settingpath,name);
                                    //删除文件 
                                    File.Delete(runpath + "\\" + name + ".in");
                                    File.Delete(runpath + "\\" + name + ".ans");
                                    File.Delete(runpath + "\\" + name + ".out");
                                }
                                else
                                {
                                    score_judge(0, name);
                                    //删除文件
                                    File.Delete(runpath + "\\" + name + ".in");
                                    File.Delete(runpath + "\\" + name + ".ans");
                                    break;
                                }
                                
                            }
                            score_judge(program_score, name);
                        }

                    }
                    else
                        score_judge(0, name);//直接判0
                }
                else
                    score_judge(0, name);//直接判0
                this.BeginInvoke(new Action(this.judge_load_plus));
                File.Delete(runpath + "\\" + name + ".cpp");
                File.Delete(runpath + "\\" + name + ".exe");//将本选手的程序删除，以备后续评测
            }
            errorread.Close();
        }

        private void ans_judge(string testdatapath, string settingpath, string name)
        {
            //初始化reader与writer
            FileStream error = new FileStream(scorepath + "\\" + "error.txt", FileMode.OpenOrCreate);
            StreamReader errorread = new StreamReader(error);

            //初始化路径
            DirectoryInfo DI_playerpath = new DirectoryInfo(playerpath);
            DirectoryInfo[] test2 = DI_playerpath.GetDirectories();
            //针对每个选手的评测
            foreach(DirectoryInfo i in test2)
            {
                //根据error情况决定是否评测
                string error_ = errorread.ReadLine();
                if (error_ == "False")
                {
                    //初始化路径
                    bool exist = true;
                    int j = 0;//选手输出文件个数
                    int testdatanum = Convert.ToInt32(File.ReadAllText(settingpath + "\\" + "datanum.txt"));//测试点个数
                    string playername = i.Name;
                    string now_playerpath = playerpath + "\\" + playername + "\\" + name;
                    DirectoryInfo DI_outpath = new DirectoryInfo(now_playerpath);
                    FileInfo[] test3 = DI_outpath.GetFiles("*.out", SearchOption.TopDirectoryOnly);
                    foreach (FileInfo outnum in test3)//输出文件格式与标号是否正确
                    {
                        j++;
                        if (!File.Exists(now_playerpath + "\\" + name + Convert.ToString(j) + ".out"))
                            exist = false;
                    }
                    if (exist&&j==testdatanum)//路径完整无误且答案个数与测试点数相同
                    {
                        int score = 0;
                        for(int n=1;n<=testdatanum;n++)//循环测试点个数次
                        {
                            //文件拷贝并重命名
                            File.Copy(testdatapath + "\\" + name + Convert.ToString(n) + ".ans", runpath + "\\" + name + ".ans",true);
                            File.Copy(now_playerpath + "\\" + name + Convert.ToString(n) + ".out", runpath + "\\" + name + ".out",true);
                            //比较函数
                            score += compare(runpath, settingpath, name);
                            //删除文件
                            File.Delete(runpath + "\\" + name + ".ans");
                            File.Delete(runpath + "\\" + name + ".out");
                        }
                        score_judge(score, name);
                    }
                    else
                        score_judge(0, name);
                }
                else
                    score_judge(0, name);

                this.BeginInvoke(new Action(this.judge_load_plus));
            }
            errorread.Close();
        }

        private void score_judge(int score,string name)
        {
            FileStream FS_score = new FileStream(s_problem + "\\" + name + ".txt", FileMode.Append);
            StreamWriter SR_score = new StreamWriter(FS_score);
            SR_score.WriteLine(score);
            SR_score.Close();
        }

        private int compare(string runpath,string settingpath,string name)
        {
            //获取AC分数
            int ACscore=Convert.ToInt32(File.ReadAllText(settingpath+"\\"+ "score.txt"));
            //是否开启spj
            bool spj = false;
            if (File.ReadAllText(settingpath + "\\" + "SPJ.txt") == "On")
                spj = true;

            int score=0;//定义分数变量

            if(spj)
            {
                //执行spj程序
                Process exeProcess = new Process();
                exeProcess.StartInfo.FileName = runpath + "\\" + name + "-score" + ".exe";
                exeProcess.StartInfo.UseShellExecute = false;
                exeProcess.StartInfo.RedirectStandardInput = true;
                exeProcess.StartInfo.RedirectStandardOutput = true;
                exeProcess.StartInfo.RedirectStandardError = true;
                exeProcess.StartInfo.CreateNoWindow = true;
                exeProcess.Start();
                exeProcess.WaitForExit();
                exeProcess.Close();
                //读取分数
                score = Convert.ToInt32(File.ReadAllText(runpath + "\\" + name + ".score"));
            }
            else
            {
                //预处理(去除所有空格与回车)
                string qaq = File.ReadAllText(runpath + "\\" + name + ".out");
                qaq = qaq.Replace(" ", "");
                qaq = qaq.Replace("\n", "");
                qaq = qaq.Replace("\r", "");
                string qwq = File.ReadAllText(runpath + "\\" + name + ".ans");
                qwq = qwq.Replace(" ", "");
                qwq = qwq.Replace("\n", "");
                qwq = qwq.Replace("\r", "");
                if (qaq==qwq)
                    score = ACscore;
                else
                    score = 0;
            }
            return score;
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
