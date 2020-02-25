using System;
using System.IO;
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

namespace Judgement.windows.judge_history
{
    /// <summary>
    /// Judge_History.xaml 的交互逻辑
    /// </summary>
    public partial class Judge_History : Window
    {
        List<User> users = new List<User>();
        //向DataGrid中添加数据
        private void GetDataGrid()
        {
            string matchpath = Directory.GetCurrentDirectory() + "\\" + "match";
            string matchname = File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + "stuff" + "\\" + "matchname.txt");
            string problem = matchpath + "\\" + matchname + "\\" + "score" + "\\" + "problem";
            string[] files = Directory.GetFiles(problem);
            List<List<string>> problemResultList = new List<List<string>>();
            string content;
            for (int i = 0; i < files.Length; i++)
            {
                StreamReader problemSR = new StreamReader(files[i], Encoding.Default);
                List<string> problemResult = new List<string>();
                while ((content = problemSR.ReadLine()) != null)
                {
                    problemResult.Add(content);
                }
                problemResultList.Add(problemResult);
            }

            StreamReader numReader = new StreamReader(matchpath + "\\" + matchname + "\\" + "score" + "\\" + "players.txt", Encoding.Default);
            int currentCardNum = 0;
            while ((content = numReader.ReadLine()) != null)
            {
                User user = new User();
                user.num = content;
                user.score = "";
                int addscore = 0;
                for (int i = 0; i < problemResultList.Count; i++)
                {
                    user.score += problemResultList[i][currentCardNum].PadLeft(8, ' ') + " ";
                    addscore += Convert.ToInt32(problemResultList[i][currentCardNum]);
                }
                user.add = addscore;
                users.Add(user);
                currentCardNum++;
            }
            users.RemoveAt(users.Count - 1);
            datagrid.ItemsSource = users;
        }

        //定义要绑定的类
        private class User
        {
            public string num { get; set; }
            public string score { get; set; }
            public int add { get; set; }
        }
        public Judge_History()
        {
            InitializeComponent();
        }

        private void Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.GetDataGrid();
        }
    }
}
