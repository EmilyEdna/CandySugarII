using CandySugar.Logic;
using CandySugar.Logic.Common;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.TempControl
{
    public class CandyImageSearchHandler : SearchHandler
    {
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (newValue.IsNullOrEmpty())
                ItemsSource = null;
            else
            {
                var Const = ChineseImageLabel.ImageLabel.Select(t => new CandyLabel
                {
                    EnLabel = t.Key,
                    ZhLabel = t.Value
                }).Where(t => t.ZhLabel.Contains(newValue) || t.EnLabel.Contains(newValue)).ToList();

                var service = CandyContainer.Instance.Resolve<ICandyService>();
                Const.AddRange(service.SearcheTag(newValue));

                ItemsSource = Const;
            }
        }
        protected override void OnItemSelected(object item)
        {
            this.Query = (item as CandyLabel).EnLabel;
            base.OnItemSelected(item);
            (this.Command as DelegateCommand).Execute();
        }
    }
}
