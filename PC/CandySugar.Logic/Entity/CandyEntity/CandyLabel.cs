namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyLabel : BaseEntity
    {
        /// <summary>
        /// 英文标签
        /// </summary>
        public string EnLabel { get; set; }
        /// <summary>
        /// 中文标签
        /// </summary>
        public string ZhLabel { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? Modify { get; set; }
    }
}
