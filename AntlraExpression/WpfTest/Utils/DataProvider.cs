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
                        return list;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var data = new ItemTreeNode
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = row["ItemInfo"].ToString(),
                                Pid = Convert.ToInt32(row["Pid"]),
                                ProjInfoIndexList = ExplainListInfo(row["ProjIndexList"].ToString()),
                            };
                        list.Add(data);
                    }
                }
            }
            return list;
        }

        private static IEnumerable<int> ExplainListInfo(string listInfo)
        {
            try
            {
                string[] spiltedStr = listInfo.Split(new char[] { ',' });
                var IndexArray = from str in spiltedStr
                                 let index = int.Parse(str)
                                 select index;
                return IndexArray;
            }
            catch (Exception)
            {
                return new List<int>();
            }
            
        }

        public static ObservableCollection<ProjInfoData> GetProjInfoDatas(string dbName)
        {
            var list = new ObservableCollection<ProjInfoData>();
            if (File.Exists(dbName))
            {
                using (var deLinker = new DbOperator(dbName, DbCategory.Accdb))
                {
                    DataSet ds = deLinker.ExecuteDataSet("select * from ItemToData");
                    if (ds.Tables.Count == 0)
                        return list;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var data = new ProjInfoData
                        {
                            NumIndex = Convert.ToInt32(row["NumIndex"]),
                            ProjInfo = row["ProjInfo"].ToString(),
                            Unit = row["Unit"].ToString(),
                        };
                        list.Add(data);
                    }
                }
            }
            return list;
        }
    }
}
