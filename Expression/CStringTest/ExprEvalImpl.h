/** \file		ExprEvalImpl.h
 *  \brief		��ѧ���ʽ������ ֧�ֱ��� ֧�ֺ���
 *  \author		Qm
 *  \date		2013-7-22
 *  \details

 sin.cos.asin�Ȳ��û�����,���ýǶ�x����dtor(x)ת���ɻ���
 e��piΪ��������
 ֧��>=, <= , <, >, ==, !=  & | �ж� �淵��1���ٷ���0

 ���ȼ����˳� �� �Ӽ��ͱȽ�
 ����ʱ��ע������ţ�������ܵ��½������ȷ

 function support:
 ����	�������� ʵ��
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
	 *  ������ʽ
	 */
	class ExprEvalImpl
	{
	public:
		ExprEvalImpl();
		virtual ~ExprEvalImpl();
		
	private:		
		std::map<QString, double>	m_variabler;							///< ����
		QString						m_error;

		int Funk(LPTSTR p, int binary, double &x, double &y, double &z);	///< ����������
		double GetNumber(LPTSTR p, int &move);								///< ���㺯��ֵ�������� ��ֵ
		inline bool TestDelimiter(TCHAR c);									///< �ж��Ƿ��������
		double Calc(LPTSTR p, int len);										///< ���㺯�������ڲ�����

	public:
		void Init();														///< ��ʼ��
		double Eval(LPCTSTR psz);										///< ������ֵ,���ʽ����
		LPCTSTR GetError() const;
		bool IsError() const;
	};


	/**
	 *  һ�������ļ��㷽��
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
