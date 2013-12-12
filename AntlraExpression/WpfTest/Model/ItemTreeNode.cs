using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfTest.Utils;

namespace WpfTest.Model
{
    public class ItemTreeNode : SelectableTreeNode
    {
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
    }
}
