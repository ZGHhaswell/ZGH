/* *******************************************************************
created:	2008/03/20
created:	19:3:2008   17:50
filename: 	ExprEvalImpl.cpp
file ext:	cpp
purpose:	数学表达式计算器 支持变量 支持函数
sin.cos.asin等采用弧度制,如用角度x，用dtor(x)转换成弧度
e和pi为保留变量

*********************************************************************/
#include "stdafx.h"
#include <tchar.h>
#include <cmath>
#include <Windows.h>

#pragma warning(disable:4267)
#include "ExprEvalImpl.h"



using namespace std;



#ifdef USING_DEBUG_NEW
#define new DEBUG_NEW
#undef THIS_FILE
static TCHAR THIS_FILE[] = __FILE__;
#endif




// 常量
const double Pi				= 3.1415926535897932384626433832795;
const double PiOver180		= 0.017453292519943295769236907684886;
//const double PiUnder180		= 57.295779513082320876798154814105;


// 定义常数(精度)
const double resabs = 1.0e-5-1.0e-30;	/*9.9999999999999999e-007;*/	/* distance criterion */
//const double resnor = 1.0e-10;			/* relative criterion: the smallest value which can be compared meaningfully with 1 */
//const double resfit = 1.0e-3;			/* default resolution of curve fitting */
const double resmch = 1.0e-10;			/* machine precision */

inline bool is_equal( double f1, double f2, double res = resabs ){
	double f = f1 - f2;
	return (f < res && f > -res);
}

// 更小
inline bool is_less_than( double f1, double f2, double res = resmch ){
	return f1 < f2 - res;
}

// 更大
inline bool is_greater_than( double f1, double f2, double res = resmch ){
	return f1 > f2 + res;
}

// 小于、等于
inline bool is_less_equal_than( double f1, double f2, double res = resmch ){
	return !is_greater_than(f1, f2, res);
}

// 大于、等于
inline bool is_greater_equal_than( double f1, double f2, double res = resmch ){
	return !is_less_than(f1, f2, res);
}

// 匹配更少
inline bool is_between( double val, double f1, double f2, double res = resmch ){
	return is_greater_than(val, min(f1, f2), res) &&
		is_less_than(val, max(f1, f2), res);
}

// 匹配更多
inline bool is_between_equal( double val, double f1, double f2, double res = resmch ){
	return is_greater_equal_than(val, min(f1, f2), res) &&
		is_less_equal_than(val, max(f1, f2), res);
}


namespace BIMCloud{ namespace NativeServices
{
	typedef map<QString, double>::iterator MAPIT;



	ExprEvalImpl::ExprEvalImpl()
	{
		Init();
	}

	ExprEvalImpl::~ExprEvalImpl()
	{
	}

	double ExprEvalImpl::Calc(LPTSTR p, int len)
	{
		
		int pos = 0;
		int move = 0;		//移动的长度
		bool neg = false;	//是否负数

		while(p[pos] == TEXT('+') || p[pos] == TEXT('-')) //处理前面连着的+-符号
		{
			if(p[pos] == TEXT('-'))
			{
				neg = !neg;
			}
			pos++;
		}

		double d1 = GetNumber(p + pos, move);
		if(neg)
		{
			d1 = -d1;
		}
		pos += move;

		while(pos < len)
		{
			TCHAR c = p[pos];

			if (c == TEXT('&'))	//处理&
			{
				pos += 1;
				double d2 = Calc(p + pos, len - pos);
				return  d1 && d2;
			}

			if (c == TEXT('|'))	//处理|
			{
				pos += 1;
				double d2 = Calc(p + pos, len - pos);
				return  d1 || d2;
			}

			if ((c == TEXT('=') && p[pos+1] == TEXT('='))  )	//处理==比较
			{
				pos += 2;
				double d2 = Calc(p + pos, len - pos);
				return  is_equal(d1, d2);
			}

			if ((c == TEXT('!') && p[pos+1] == TEXT('='))  || (c == TEXT('<') && p[pos+1] == TEXT('>')))	//处理!=比较
			{
				pos += 2;
				double d2 = Calc(p + pos, len - pos);
				return  !is_equal(d1, d2);
			}

			if ((c == TEXT('>') || c == TEXT('<'))  )	//处理大于小于
			{
				double d2 = 0;
				if ((p[pos+1] == TEXT('='))) //>= <=
				{
					pos += 2;
					d2 = Calc(p + pos, len - pos);
					if (c == TEXT('>'))
					{
						//return (!is_less_than(d1, d2));
						return (is_greater_equal_than(d1, d2));
					}
					if (c == TEXT('<'))
					{
						//return (!is_greater_than(d1, d2));
						return (is_less_equal_than(d1, d2));
					}
				}
				else
				{
					pos ++;
					d2 = Calc(p + pos, len - pos);
					if (c == TEXT('>'))
					{
						return is_greater_than(d1, d2);
					}
					if (c == TEXT('<'))
					{
						return is_less_than(d1, d2);
					}
				}
			}

			if(c == TEXT('+') || c == TEXT('-'))
			{
				return d1 + Calc( p + pos, len - pos );
			}

			if(c == TEXT('*') || c == TEXT('/') || c == TEXT('%'))
			{
				pos++;
				neg = false;
				while(p[pos] == TEXT('+') || p[pos] == TEXT('-'))
				{
					if(p[pos] == TEXT('-'))
					{
						neg =!neg;
					}
					pos++;
				}

				double d2 = GetNumber(p + pos, move);
				if(neg)
				{
					d2 = -d2;
				}
				pos += move;

				if(c == TEXT('*'))
				{
					d1 *= d2;
				}
				else if(abs(d2) <= resabs)//is_zero(d2)
				{
					m_error = TEXT("除数不能为零！ ");
					throw m_error.c_str();
				}
				else if(c == TEXT('/'))
				{
					d1 /= d2;
				}
				else
				{
					d1 = fmod(d1,d2);
				}
			}
			else
			{
				TCHAR err[255];
				_stprintf_s(err, _T("不可识别的符号： '%c'！"), p[pos]);
				m_error = err;
				throw m_error.c_str();
			}
		}
		return d1;
	}

	int ExprEvalImpl::Funk(LPTSTR p, int binary, double &x, double &y, double &z)
	{
		int size = 0;
		int nr = 1;		//左括号数,原本带一个
		while(nr != 0)
		{
			if(!p[size])
			{
				m_error = TEXT("缺失右括号！");
				throw m_error.c_str();
			}
			if( p[size] == TEXT('('))
			{
				nr++;
			}
			if( p[size] == TEXT(')'))
			{
				nr--;
			}
			size++;
		}
		size--;

		if(binary == 1)	//如果函数带两个参数
		{
			int i=0 ;
			for(; i < size; ++i)
			{
				if(p[i] == TEXT('('))
				{
					int nr = 1;
					while(nr && i < size)
					{
						i++;
						if(p[i] == TEXT('(')) nr++;
						if(p[i] == TEXT(')')) nr--;
					}
				}
				if(p[i] == TEXT(','))
				{
					break;
				}
			}
			if(i == size)
			{
				m_error = TEXT("缺失参数列表间隔符号： TEXT(',')！");
				throw m_error.c_str();
			}
			x = Calc(p, i);
			y = Calc(p + i + 1, size - i - 1);
		}
		else if(binary == 2)//如果函数带三个参数
		{
			int number = 0;
			int first_pos = 0;
			int second_pos = 0;
			int i=0 ;
			for(; i < size; ++i)
			{
				if(p[i] == TEXT('('))
				{
					int nr = 1;
					while(nr && i < size)
					{
						i++;
						if(p[i] == TEXT('(')) nr++;
						if(p[i] == TEXT(')')) nr--;
					}
				}
				if(p[i] == TEXT(','))
				{
					number++;
					if (number == 1)
					{
						first_pos = i;
					}
					else if (number == 2)
					{
						second_pos = i;
					}
				}
			}
			if(number != 2)
			{
				m_error = TEXT("缺失参数列表间隔符号： TEXT(',')！");
				throw m_error.c_str();
			}
			x = Calc(p, first_pos);
			y = Calc(p + first_pos + 1, second_pos - first_pos - 1);
			z = Calc(p + second_pos + 1, size - second_pos - 1);
		}
		else
		{
			x = Calc(p, size);
		}
		return size + 1;
	}
	
	//strnicmp _memicmp
	//定义表达式中的函数处理宏
#define FX(FUNK, LEN, BIN, DO)							\
	{													\
		if(!_tcsnicmp(p, TEXT(FUNK), LEN))				\
		{												\
			move = LEN + Funk(p + LEN, BIN, x, y, z);	\
			{											\
				DO;										\
			}											\
		}												\
	}

	double ExprEvalImpl::GetNumber(LPTSTR p, int &move)
	{
		double x = 0;
		double y = 0;
		double z = 0;
		switch (_totlower(*p))
		{
		case TEXT('a'):
			FX("abs("  ,4,0, return abs(x););
			FX("asin(" ,5,0, return asin(x););
			FX("acos(" ,5,0, return acos(x););
			FX("atan(" ,5,0, return atan(x););
			break;
		case TEXT('c'):
			FX("cos("  ,4,0, return cos(x););
			FX("cosh(" ,5,0, return cosh(x););
			break;
		case TEXT('d'):
			FX("dtor(" ,5,0, x = x * PiOver180; return x;);
		case TEXT('e'):
			FX("exp("  ,4,0, return exp(x););
			break;
		case TEXT('i'):
			FX("int("  ,4,0, if(x<0) return ceil(x); return floor(x););
			FX("iif("  ,4,2, if(x) return y; return z;);
			break;
		case TEXT('l'):
			FX("ln("   ,3,0, return log(x););
			FX("log("  ,4,0, return log10(x););
			break;
		case TEXT('m'):
			FX("max("  ,4,1, if(x>y) return x; return y;);
			FX("min("  ,4,1, if(x<y) return x; return y;);
			break;
		case TEXT('p'):
			FX("pow("  ,4,1, return pow(x,y););
			break;
		case TEXT('s'):
			FX("sqr("  ,4,0, return x*x;);
			FX("sqrt("  ,5,0, return sqrt(x););
			FX("sin("  ,4,0, return sin(x););
			FX("sqrn(" ,5,1, return std::pow(x,1/y););
			FX("sinh(" ,5,0, return sinh(x););
			break;
		case TEXT('t'):
			FX("tan("  ,4,0, return tan(x););
			FX("tanh(" ,5,0, return tanh(x););
			break;
		case TEXT('('):
			FX("("     ,1,0, return x;)
			break;
		}

		if(p[0] == TEXT('0') && (p[1] == TEXT('x') || p[1] == TEXT('X')) )
		{
			int i = 0;
			TCHAR c = 0;
			move = 1;
			_stscanf_s(p, TEXT("%x"), &i);
			do
			{
				move++;
				c = _totlower(p[move]);
			}while( (c >= TEXT('0') && c <= TEXT('9')) || (c >= TEXT('a') && c <= TEXT('f')));
			return (double)i;
		}

		if(p[0] == TEXT('0') && (p[1] == TEXT('b') || p[1] == TEXT('B')) )
		{
			int i=0; move = 2;
			while(p[move] == TEXT('0') || p[move] == TEXT('1'))
			{
				i *= 2;
				if(p[move] == TEXT('1'))
				{
					i++;
				}
				move++;
			}
			return (double)i;
		}

		if( (*p >= TEXT('0') && *p <= TEXT('9')) || *p == TEXT('.') )
		{
			move = 0;
			double d = _ttof(p);
			if(p[move] == TEXT('+') || p[move] == TEXT('-'))
			{
				move++;
			}
			while(p[move] >= TEXT('0') && p[move] <= TEXT('9'))
			{
				move++;
			}
			if(p[move] == TEXT('.'))
			{
				move++;
			}
			while(p[move] >= TEXT('0') && p[move] <= TEXT('9'))
			{
				move++;
			}
			if   (p[move] == TEXT('e') || p[move] == TEXT('E'))
			{
				move++;
				if (p[move] == TEXT('+') || p[move] == TEXT('-'))
				{
					move++;
				}
				while(p[move] >= TEXT('0') && p[move] <= TEXT('9'))
				{
					move++;
				}
			}
			return d;
		}

		bool found = false;
		move = 0;
		for(MAPIT it = m_variabler.begin(); it != m_variabler.end(); ++it)
		{
			if(_tcsncmp( p, it->first.c_str(), it->first.size()) == 0)
			{
				found = true;
				if(it->first.size() > (unsigned int)move)
				{
					move = it->first.size();
					x = it->second;
				}
			}
		}
		if(found)	//如果有此变量
		{
			return x;
		}

		if(_tcsnicmp( p, TEXT("pi"), 2) == 0 && TestDelimiter(p[2]) )
		{
			move = 2;
			return Pi;
		}

		// 	if(p[0] == TEXT('e') && TestDelimiter(p[1]) )
		// 	{
		// 		move = 1;
		// 		return 2.71828182845904523536;
		// 	}

		int i=0;
		TCHAR tmp[MAX_PATH];
		while( !TestDelimiter(p[i]) )
		{
			tmp[i] = p[i];
			++i;
		}

		tmp[i]=0;
		if(i)
		{
			TCHAR err[MAX_PATH + 100];
			_stprintf_s(err, TEXT("表达式语法错误： '%s'！"),tmp);
			m_error = err;
			throw m_error.c_str();
		}
		m_error = TEXT("表达式语法错误！");
		throw m_error.c_str();
	}

	double ExprEvalImpl::Eval(LPCTSTR psz)
	{
		int i=0;
		int k=0;
		TCHAR p[5000];
		while(psz[k])		//清除括号
		{
			if(!_istspace(psz[k]))
			{
				p[i] = psz[k];
				i++;
			}
			k++;
		}

		if(i == 0)
		{
			return 0;
		}

		p[i] = 0;

		for(k  = 0; k < i; ++k)//如果中间有单个等号，认为变量赋值
		{
			if(p[k] == TEXT('=')
				&& (p[k-1] != TEXT('<') && p[k-1] != TEXT('>')			//>= <=
				&& (p[k+1] != TEXT('=')&& p[k-1] != TEXT('='))			// ==
				&& (p[k-1] != TEXT('!'))))						//!=
			{
				break;
			}
		}

		if(k >= i)	//如果没有等号
			k = -1;

		double d = Calc(p+k+1,i-k-1);	//计算表达式值


		if(k >= 0)			//如果赋值
		{
			if( (!_istalpha(p[0]) ) )
			{
				m_error = TEXT("变量名称必须以字母开头");
				throw m_error.c_str();
			}
			i=1;
			for(; i < k; ++i)
			{
				if( !_istalnum(p[i]))
				{
					m_error = TEXT("变量名称必须为字母或数字的组合");
					throw m_error.c_str();
				}
			}
			QString s;
			s.append(p, k);
			if(/*s == TEXT("e") || */s == TEXT("PI") || s == TEXT("pi") )
			{
				m_error = TEXT("保留变量名称");
				throw m_error.c_str();
			}
			if(m_variabler[s] != d)
			{
				m_variabler[s] = d;
			}
		}
		return d;
	}

	bool ExprEvalImpl::TestDelimiter(TCHAR c)
	{
		return (c == TEXT('+')
			|| c == TEXT('-')
			|| c == TEXT('*')
			|| c == TEXT('/')
			|| c == TEXT('%')
			|| c == TEXT(',')
			|| c == TEXT(')')
			|| c == TEXT('>')
			|| c == TEXT('<')
			|| _istspace(c)
			|| c == 0 );
	}

	void ExprEvalImpl::Init()
	{
		m_error.clear();
		m_variabler.clear();
	}

	LPCTSTR ExprEvalImpl::GetError() const
	{
		return m_error.c_str();
	}

	bool ExprEvalImpl::IsError() const
	{
		return !m_error.empty();
	}

}}

/** \version $Id: ExprEvalImpl.cpp 9229 2011-12-01 03:17:25Z qmroom $*/
