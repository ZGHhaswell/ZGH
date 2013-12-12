using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
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


        public ObservableCollection<ItemTreeNode> ItemsList { get; set; }

        public ICommand SelectCommand { get; set; }

        public SelectableTreeNode SelectedNode { get; set; }
        public QdItemsViewModel()
        {
            //TreeDataInit();

            var dataList = DataProvider.GetTreeData("MyDemo.accdb");
            RelationShipInit(dataList);
            RegisterCommands();
            
            
            //MessageBox.Show("QdItemsViewModel");
        }

        private void RelationShipInit(ObservableCollection<ItemTreeNode> dataList)
        {
            ItemsList = new ObservableCollection<ItemTreeNode>();
            foreach (var node in dataList)
            {
                if (node.Pid == 0)
                {
                    ItemsList.Add(node);
                }
                else
                {
                    foreach (var node1 in dataList)
                    {
                        if (node1.Id == node.Pid)
                        {
                            node1.Children.Add(node);
                        }
                    }
                }
            }

            foreach (var node in ItemsList)
            {
                ExpandAll(node);
            }
        }

        

        private void RegisterCommands()
        {
            SelectCommand = new DelegateCommand(SelectCommandExecute);
              
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
                MessageBox.Show(itemnode.Name); 
            }
            
        }

        private void TreeDataInit()
        {
            ItemsList = new ObservableCollection<ItemTreeNode>();
            var rootNode = new ItemTreeNode() {Name = "Root"};
            var childNode = new ItemTreeNode() {Name = "Child"};
            var child1Node = new ItemTreeNode() {Name = "Child1"};
            var otherNode = new ItemTreeNode() {Name = "Third"};
            childNode.Children.Add(otherNode);
            rootNode.Children.Add(childNode);
            rootNode.Children.Add(child1Node);

            ExpandAll(rootNode);

            ItemsList.Add(rootNode);
        }

        private void ExpandAll(SelectableTreeNode rootNode)
        {
            rootNode.IsExpanded = true;

            if (rootNode.Children != null)
            {
                foreach (var node in rootNode.Children)
                {
                    ExpandAll(node);
                }
            }
        }

        
    }
}
