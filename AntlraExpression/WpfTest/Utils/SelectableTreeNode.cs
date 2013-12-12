using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace WpfTest.Utils
{
    /// <summary>
    /// 可选中的树节点
    /// </summary>
    public class SelectableTreeNode : NotificationObject
    {
        #region 属性

        private bool _isSelected;

        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        private bool _isExpanded;

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        /// <summary>
        /// 子节点
        /// </summary>
        public ObservableCollection<SelectableTreeNode> Children
        {
            get;
            private set;
        }

        /// <summary>
        /// 递归寻找该节点下被选中的节点，如果没有被选中则返回null
        /// </summary>
        public SelectableTreeNode SelectedNode
        {
            get
            {
                SelectableTreeNode selectedNode = null;
                if (IsSelected)
                {
                    selectedNode = this;
                }
                else
                {
                    foreach (var item in Children)
                    {
                        selectedNode = item.SelectedNode;
                        if (selectedNode != null)
                        {
                            break;
                        }
                    }
                }

                return selectedNode;
            }
        }

        #endregion

        #region 构造函数

        public SelectableTreeNode()
        {
            Children = new ObservableCollection<SelectableTreeNode>();
        }

        #endregion
    }
}
