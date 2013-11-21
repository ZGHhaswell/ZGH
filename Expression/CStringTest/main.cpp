#define _AFXDLL
#include <iostream>
#include <afx.h>
#include "ExprEvalImpl.h"

using namespace std;
using BIMCloud::NativeServices::ExprEvalImpl;

int main()
{
	ExprEvalImpl test;
	try
	{
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
		printf(e);
		//CString str = _T(e);
		//cout << str;
	}

	system("pause");
	return 0;
}