using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace WpfTest.Model
{
    public class ProjInfoData : NotificationObject, ICloneable
    {
        public ProjInfoData()
        {
            
        }

        private int numIndex;
        public int NumIndex
        {
            get { return numIndex; }
            set
            {
                numIndex = value;
                RaisePropertyChanged("NumIndex");
            }
        }
        private string projInfo;
        public string ProjInfo
        {
            get { return projInfo; }
            set
            {
                projInfo = value;
                RaisePropertyChanged("ProjInfo");
            }
        }
        private string unit;
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                RaisePropertyChanged("Unit");
            }
        }



        public object Clone()
        {
            return new ProjInfoData()
                {
                    numIndex = this.numIndex,
                    projInfo = this.projInfo,
                    unit = this.unit
                };
        }
    }
}
