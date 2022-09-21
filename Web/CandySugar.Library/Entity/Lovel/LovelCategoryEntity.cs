using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Lovel
{
    public class LovelCategoryEntity:BaseEntity
    {
        /// <summary>
        /// 分类路由
        /// </summary>
        public string CategoryRoute { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 出版社
        /// </summary>
        public string Press { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        public string DetailAddress { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string BookName { get; set; }
    }
}
