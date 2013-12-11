using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.ViewModel;

namespace WpfTest.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class QdItemsViewModel : NotificationObject
    {


        public QdItemsViewModel()
        {
            //MessageBox.Show("QdItemsViewModel");
        }
    }
}
