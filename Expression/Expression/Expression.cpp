// Expression.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "ExprEvalImpl.h"
#include <iostream>

using namespace BIMCloud::NativeServices;
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	try
	{
		ExprEvalImpl test;
		test.Init();
		double result0 = test.Eval(_T("x = ZGH")); 
		//test.Eval(_T("y = 2*x"));
		double result1   = test.Eval(_T("iif((5==5 ) | (5 == 1), 10, 0) + 5"));
		//double result2 = test.Eval(_T("iif(5==5  | 5 == 1, 10, 0) + 5"));

		cout << result0 << "\n";
		cout << result1 << "\n";
	}
	catch (LPCTSTR e)
	{
		//CString str = _T("中文");
		//cout << str;
	}
	return 0;
}

