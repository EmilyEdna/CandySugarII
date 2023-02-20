using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.Com.Library.HotKey
{
    public class HotKeyAction
    {
        public Func<Window,bool> ActionEvent { get; set; }
    }
}
