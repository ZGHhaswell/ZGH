/** \file		ExprEvalImpl.h
 *  \brief		数学表达式计算器 支持变量 支持函数
 *  \author		Qm
 *  \date		2013-7-22
 *  \details

 sin.cos.asin等采用弧度制,如用角度x，用dtor(x)转换成弧度
 e和pi为保留变量
 支持>=, <= , <, >, ==, !=  & | 判断 真返回1，假返回0

 优先级：乘除 ， 加减和比较
 运用时请注意加括号，否则可能导致结果不正确

 function support:
 名称	参数个数 实现
 "abs"  0, return abs(x);
 "asin" 0, return asin(x);
 "acos" 0, return acos(x);
 "atan" 0, return atan(x);
 "cos"  0, return cos(x);
 "cosh" 0, return cosh(x);
 "dtor" 0, x = x * Pi/180.0; return x;
 "exp"  0, return exp(x);
 "int"  0, if(x<0) return ceil(x); return floor(x);
 "iif"  2, if(x) return y; return z;
 "ln"   0, return log(x);
 "log"  0, return log10(x);
 "max"  1, if(x>y) return x; return y;
 "min"  1, if(x<y) return x; return y;
 "pow"  1, return pow(x,y);
 "sqr"  0, return x*x;
 "sqrt" 0, return sqrt(x);
 "sin"  0, return sin(x);
 "sqrn" 1, return std::pow(x,1/y);
 "sinh" 0, return sinh(x);
 "tan"  0, return tan(x);
 "tanh" 0, return tanh(x);

 \example
 try
 {
	 ExprEvalImpl test;
	 test.Init();
	 test.EvalGivenExpr("x = 5");
	 test.EvalGivenExpr("x1 = 2*x");
	 double result = test.EvalGivenExpr("x+x1*2");
 }
 catch (LPCTSTR e)
 {
	//ErrorMessage: e
 }

 */


/* ********************************************************************/

#pragma once
#include <map>
#include <string>
#include <Windows.h>



namespace BIMCloud{ namespace NativeServices
{
	typedef std::basic_string<TCHAR, std::char_traits<TCHAR>, std::allocator<TCHAR> > QString;
	
	/**
	 *  计算表达式
	 */
	class ExprEvalImpl
	{
	public:
		ExprEvalImpl();
		virtual ~ExprEvalImpl();
		
	private:		
		std::map<QString, double>	m_variabler;							///< 变量
		QString						m_error;

		int Funk(LPTSTR p, int binary, double &x, double &y, double &z);	///< 处理函数括号
		double GetNumber(LPTSTR p, int &move);								///< 计算函数值，参数， 数值
		inline bool TestDelimiter(TCHAR c);									///< 判断是否操作符号
		double Calc(LPTSTR p, int len);										///< 计算函数，供内部调用

	public:
		void Init();														///< 初始化
		double Eval(LPCTSTR psz);										///< 变量赋值,表达式输入
		LPCTSTR GetError() const;
		bool IsError() const;
	};


	/**
	 *  一个参数的计算方法
	 */
	template<class T>
	inline T OneParamExprCalc(LPCTSTR strExpr, LPCTSTR strParam, double dblParam, T _defaultValue)
	{
		ExprEvalImpl test;
		CString strTemp;

		test.Init();
		try
		{
			if(NULL != strParam && TEXT('\0') != strParam[0])
			{
				strTemp.Format(TEXT("%s=%f"), strParam, dblParam);
				test.EvalGivenExpr(strTemp);
			}
			return (T)test.EvalGivenExpr(strExpr);
		}
		catch (LPCTSTR /*eMsg*/)
		{
			return _defaultValue;
		}
	}

}}




/** \version $Id: ExprEvalImpl.h 4875 2011-05-14 07:16:35Z qmroom $*/
