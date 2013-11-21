using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AntlraExpression;


namespace ExpressionTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //增加
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var Var = VarBox.Text;
            if (!VarslistBox.Items.Contains(Var))
            {
                VarslistBox.Items.Add(Var);
            }
            
        }

        //清空
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ExpressionBox.Clear();
            VarBox.Clear();
            VarslistBox.Items.Clear();
            listBox2.Items.Clear();
        }

        //解析
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            listBox2.Items.Clear();

            var exp = new ExpressionParser(ExpressionBox.Text);
            var varlist = new List<string>();
            foreach (var str in VarslistBox.Items)
            {
                var vars = str as string;
                if (vars != null)
                {
                    varlist.Add(vars);
                }
            }
            exp.SetEnabledAttributes(varlist);
            if (exp.HasErrors())
            {
                listBox2.Items.Add(exp.Error);
            }
            else
            {
                listBox2.Items.Add("表达式无误！ 假设变量都为1\n 计算结果为： " + exp.Simulate());

            }


            
        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
