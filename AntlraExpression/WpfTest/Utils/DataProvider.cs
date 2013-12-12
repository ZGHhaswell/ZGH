using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using BIMCloud.Framework.Database;
using WpfTest.Model;

namespace WpfTest.Utils
{
    public class DataProvider
    {
        public static ObservableCollection<ItemTreeNode> GetTreeData(string dbName)
        {
            var list = new ObservableCollection<ItemTreeNode>();
            if (File.Exists(dbName))
            {
                using (var deLinker = new DbOperator(dbName, DbCategory.Accdb))
                {
                    DataSet ds = deLinker.ExecuteDataSet("select * from Demo");
                    if (ds.Tables.Count == 0)
                        return null;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var data = new ItemTreeNode
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = row["ItemInfo"].ToString(),
                                Pid = Convert.ToInt32(row["Pid"]),
                            };
                        list.Add(data);
                    }
                }
            }
            return list;
        }
    }
}
