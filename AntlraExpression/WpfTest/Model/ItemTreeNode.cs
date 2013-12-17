using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using WpfTest.Utils;

namespace WpfTest.Model
{
    public class ItemTreeNode : SelectableTreeNode
    {
        public ItemTreeNode()
        {
            HostProjDataList = new List<ProjInfoData>();
        }


        private string _name;

        /// <summary>
        /// 构件编号名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public int Id { get; set; }
        public int Pid { get; set; }
        public IEnumerable<int> ProjInfoIndexList { get; set; }
        public IList HostProjDataList { get; set; }
        private ListCollectionView hostProjList;
        public ListCollectionView HostProjList
        {
            get
            {
                if (hostProjList == null)
                {
                    hostProjList = new ListCollectionView(HostProjDataList);
                }
                return hostProjList;
            }
        }
    }
}
