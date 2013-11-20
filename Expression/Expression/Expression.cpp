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
		test.Eval(_T("x = 5")); 
		test.Eval(_T("y = 2*x"));
		double result = test.Eval(_T("iif((x==5 ) | (x == 1), y, 0) + 5"));
		cout<< result;
	}
	catch (LPCTSTR e)
	{
	}
	return 0;
}

