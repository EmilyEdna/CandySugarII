namespace CandySugar.Logic.Entity
{
    public class BaseEntity
    {
        public Guid CandyId { get; set; }
        public long Span { get; set; }
        public void Create()
        {
            this.CandyId = Guid.NewGuid();
            this.Span = DateTime.Now.Ticks;
        }
    }
}
