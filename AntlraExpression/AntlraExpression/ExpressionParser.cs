using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCalc;
using NCalc.Domain;


namespace AntlraExpression
{

    /// <summary>
    /// 表达式解析器（包含计算器）
    /// 使用示例：
    /// var expressionParser = new ExpressionParser("iif(a = '上', A, B)");
    /// 
    /// expressionParser.SetEnabledAttributes(new List(){"A", "B", "a"});     //设置许可变量
    /// if(expressionParser.HasError()) Console.WriteLine(expressionParser.Error);   //检测表达式是否合法
    /// 
    /// //计算
    /// expressionParser["a"] = "上";
    /// expressionParser["A"] = 1;
    /// expressionParser["B"] = 1;
    /// Console.WriteLine(expressionParser.Evaluate());
    /// </summary>
    public class ExpressionParser
    {
        /// <summary>
        /// 源表达式
        /// </summary>
        private string _expression;

        /// <summary>
        /// 辅助解析计算的对象
        /// </summary>
        private NCalc.Expression _assist;

        /// <summary>
        /// 变量许可范围
        /// </summary>
        private IList<string> _enabledAttributes;

        /// <summary>
        /// 在表达式通过常规合法检查后的变脸列表
        /// </summary>
        private IList<string> _unCheckedAttributes;

        /// <summary>
        /// 错误信息
        /// </summary>
        private string _errorInfomation;

        /// <summary>
        /// 转换后的表达式
        /// </summary>
        private LogicalExpression ParsedExpression
        {
            get { return _assist.ParsedExpression; }
        }

        /// <summary>
        /// 错误属性
        /// </summary>
        public string Error
        {
            get { return _errorInfomation; }
        }

        /// <summary>
        /// 参数字典，用于计算
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get { return _assist.Parameters; }
            set { _assist.Parameters = value; }
        }


        /// <summary>
        /// 计算
        /// </summary>
        /// <returns>计算结果</returns>
        public object Evaluate()
        {
            return _assist.Evaluate();
        }

        /// <summary>
        /// 表达式解析（计算）构造器
        /// </summary>
        /// <param name="expression">表达式</param>
        public ExpressionParser(string expression)
        {
            _expression = expression;
            _assist = new NCalc.Expression(expression, EvaluateOptions.IgnoreCase);
        }

        /// <summary>
        /// 设置变量许可范围
        /// </summary>
        /// <param name="attributes"></param>
        public void SetEnabledAttributes(List<string> attributes)
        {
            _enabledAttributes = attributes;
        }

        /// <summary>
        /// 检查表达式是否有错误
        /// </summary>
        /// <returns>是否有错误</returns>
        public bool HasErrors()
        {
            //常规检查
            if (_assist.HasErrors())
            {
                _errorInfomation = _assist.Error;
                return true;
            }
            //变量检查

            //获得常规检查后的变量列表
            ParserVars();

            //检查变量许可
            if (CheckVarsIsValid())
            {
                return true;
            }

            //模拟运算
            try
            {
                Simulate();
            }

            catch (Exception e)
            {
                _errorInfomation = e.ToString();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 模拟运算
        /// </summary>
        public string Simulate()
        {
            foreach (var att in _unCheckedAttributes)
            {
                _assist.Parameters[att] = 1;
            }
            return _assist.Evaluate().ToString();
        }

        /// <summary>
        /// 检查变量是否在许可范围内
        /// </summary>
        /// <returns>是否有错误</returns>
        private bool CheckVarsIsValid()
        {
            foreach (var unCheckedAttribute in _unCheckedAttributes)
            {
                if (!_enabledAttributes.Contains(unCheckedAttribute))
                {
                    _errorInfomation = "错误的组合表达式,变量: " + unCheckedAttribute + " 必须是已有属性变量。";
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 拆分转换表达式
        /// </summary>
        private void ParserVars()
        {
            _unCheckedAttributes = new List<string>();
            string tempExpression = ParsedExpression.ToString();
            for (int i = 0; i < tempExpression.Length; i++)
            {
                int front = i;
                int back = 0;

                if (tempExpression[front] == '[')
                {
                    for (int j = front + 1; j < tempExpression.Length; j++)
                    {
                        if (tempExpression[j] == ']')
                        {
                            back = j;
                            break;
                        }
                    }
                }
                if (front < back)
                {
                    _unCheckedAttributes.Add(tempExpression.Substring(front + 1, back - front - 1));
                }
            }
        }
    }
}
