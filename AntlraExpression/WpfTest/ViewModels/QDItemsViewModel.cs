using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using C1.WPF;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using WpfTest.Model;
using WpfTest.Utils;

namespace WpfTest.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class QdItemsViewModel : NotificationObject
    {

        public ICommand SelectCommand { get; set; }
        public ICommand DoubleClickCommand { get; set; }
        public DelegateCommand<string> SearchIndexCommand { get; set; }
        public DelegateCommand<string> SearchTextCommand { get; set; }

        /// <summary>
        /// TreeView Items
        /// </summary>
        public ObservableCollection<ItemTreeNode> ItemsList { get; set; }

        /// <summary>
        /// TreeView SelectedItem
        /// </summary>
        public SelectableTreeNode SelectedNode { get; set; }


        public enum SearchMode
        {
            Index,
            Text
        }

        /// <summary>
        /// Search Condition
        /// </summary>
        private string searchIndexCondition;

        private string searchTextCondition;

        private SearchMode searchMode;

        /// <summary>
        /// SearchBox Timer
        /// </summary>
        private C1.Util.Timer timer;

        /// <summary>
        /// Current ProjInfoItemList FlexGrid
        /// </summary>
        private ListCollectionView selectedProjInfoList;
        public ListCollectionView SelectedProjInfoList
        {
            get { return selectedProjInfoList; }
            set
            {
                selectedProjInfoList = value;
                RaisePropertyChanged("SelectedProjInfoList");
            }
        }

        /// <summary>
        /// selectedProjInfoItem in the flexgrid
        /// </summary>
        private ProjInfoData selectedProjInfoItem;
        public ProjInfoData SelectedProjInfoItem
        {
            get { return selectedProjInfoItem; }
            set 
            { 
                selectedProjInfoItem = value;
                RaisePropertyChanged("SelectedProjInfoItem");
            }
        }
            

        /// <summary>
        /// Double Click Event
        /// </summary>
        public event Action<ProjInfoData> UserDoubleClickEvent;

        

        public QdItemsViewModel()
        {

            var dataList = DataProvider.GetTreeData("MyDemo.accdb");
            var projInfoList = DataProvider.GetProjInfoDatas("MyDemo.accdb");

            RelationShipInit(dataList, projInfoList);

            RegisterCommands();

            SearchBoxInit();

        }

        private void SearchBoxInit()
        {
            timer = new C1.Util.Timer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += TimeTickExecute;
        }

        private void TimeTickExecute(object sender, EventArgs e)
        {
            timer.Stop();
            if (SelectedProjInfoList != null)
            {
                SelectedProjInfoList.Filter = null;
                SelectedProjInfoList.Filter = searchMode == SearchMode.Index ? 
                    GetMatchMethod("NumIndex", searchIndexCondition) : GetMatchMethod("ProjInfo", searchTextCondition);
                //RaisePropertyChanged("SelectedProjInfoList");
            }
            

        }

        private Predicate<object> GetMatchMethod(string property, string searchCondition)
        {
            return item =>
            {
                var searchCon = searchCondition;
                if (string.IsNullOrEmpty(searchCon))
                {
                    return true;
                }
                var proInfo = typeof(ProjInfoData).GetProperty(property);
                var value = proInfo.GetValue(item, null);
                if (value != null && value.ToString().IndexOf(searchCon, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    return true;
                }
                return false;
            };
        }

        private void RelationShipInit(ObservableCollection<ItemTreeNode> dataList,
            IEnumerable<ProjInfoData> projDataList)
        {
            ItemsList = new ObservableCollection<ItemTreeNode>();

            //dataList映射初始化
            foreach (ProjInfoData projInfoData in projDataList)
            {
                int index = projInfoData.NumIndex;
                foreach (var node in dataList)
                {
                    if (node.ProjInfoIndexList.Contains(index))
                    {
                        node.HostProjDataList.Add(projInfoData);
                    }
                }
            }
            //关系绑定
            foreach (var node in dataList)
            {
                if (node.Pid == 0)
                {
                    ItemsList.Add(node);
                }
                else
                {
                    foreach (var pnode in dataList)
                    {
                        if (pnode.Id == node.Pid)
                        {
                            pnode.Children.Add(node);
                        }
                    }
                }
            }
            //默认全部展开
            foreach (var node in ItemsList)
            {
                ExpandAll(node);
            }
        }

        

        private void RegisterCommands()
        {
            SelectCommand = new DelegateCommand(SelectCommandExecute);
            DoubleClickCommand = new DelegateCommand(DoubleClickCommandExecute);
            SearchIndexCommand = new DelegateCommand<string>(SearchIndexCommandExecute);
            SearchTextCommand = new DelegateCommand<string>(SearchTextCommandExecute);
        }

        private void SelectCommandExecute()
        {
            // 设置selectedNode
            foreach (var node in ItemsList)
            {
                if (node.SelectedNode != null)
                {
                    SelectedNode = node.SelectedNode;
                    break;
                }
            }
            var itemnode = SelectedNode as ItemTreeNode;
            if (itemnode != null)
            {
                SelectedProjInfoList = itemnode.HostProjList;
            }
            
        }

        private void DoubleClickCommandExecute()
        {
            RaiseUserDoubleClickEvent();
        }

        private void RaiseUserDoubleClickEvent()
        {
            if (SelectedProjInfoItem == null)
            {
                return;
            }
            UserDoubleClickEvent.Invoke(SelectedProjInfoItem.Clone() as ProjInfoData);
        }

        
        
        private void SearchIndexCommandExecute(string condition)
        {
            searchIndexCondition = condition;
            searchMode = SearchMode.Index;
            // start ticking
            timer.Stop();
            timer.Start();
        }

        private void SearchTextCommandExecute(string condition)
        {
            searchTextCondition = condition;
            searchMode = SearchMode.Text;
            // start ticking
            timer.Stop();
            timer.Start();
        }

        private void ExpandAll(SelectableTreeNode rootNode)
        {
            rootNode.IsExpanded = true;
            foreach (var node in rootNode.Children)
            {
                ExpandAll(node);
            }
            
        }

        
    }
}
