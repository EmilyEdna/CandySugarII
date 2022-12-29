namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyMusic : BaseEntity
    {
        public int Platform { get; set; }
        public string SongId { get; set; }
        public string SongName { get; set; }
        public string SongArtist { get; set; }
        public string AlbumId { get; set; }
        public string AlbumName { get; set; }
        public bool IsComplete { get; set; }
        [SugarColumn(IsNullable = true)]
        public string LocalRoute { get; set; }
        [SugarColumn(IsNullable = true)]
        public string NetRoute { get; set; }
    }
}
