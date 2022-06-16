namespace CandySugar.Logic.Entity
{
    public class BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid CandyId { get; set; }
        [SugarColumn(ColumnDataType = "bigint", IsNullable = true)]
        public long Span { get; set; }
        public void Create()
        {
            this.CandyId = Guid.NewGuid();
            this.Span = DateTime.Now.Ticks;
        }
    }
}
