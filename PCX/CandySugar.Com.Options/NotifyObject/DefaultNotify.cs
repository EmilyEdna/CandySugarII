using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CandySugar.Com.Options.NotifyObject
{
    public class DefaultNotify
    {
        /// <summary>
        /// 通知类型
        /// </summary>
        public EDefaultNotify Module { get; set; }
        /// <summary>
        /// 组件查询检索通知参数
        /// </summary>
        public object ComponentQueryWord { get; set; }
        /// <summary>
        /// 组件查询检索通知的标识组件
        /// </summary>
        public Control CurrentComponent { get; set; }
    }
}
