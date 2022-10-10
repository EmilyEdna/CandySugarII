using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Entry.CandyViewModels
{
    public class LabelViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public LabelViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.Brands = new List<string>();
            this.Properties = new List<string>();
        }
        public string Category { get; set; }
        public List<string> Brands { get; set; }
        public List<string> Properties { get; set; }

         #region 命令
        public void CategoryAction(string input)
        {
            this.Category = input;
        }
        public void BrandAction(string input)
        {
            if (!this.Brands.Contains(input))
                this.Brands.Add(input);
        }
        public void PropertyAction(string input)
        {
            if (!this.Properties.Contains(input))
                this.Properties.Add(input);
        }
        #endregion
    }
}
