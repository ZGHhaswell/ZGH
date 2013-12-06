using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelProcessTest.Tools
{
    public interface IBridgeToExcel
    {
        IEnumerable<string> GetData();
    }
}
