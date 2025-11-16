namespace EngWordApp.Entity
{
    public class Mean
    {
        public int MeanId { get; set; }
        public string TrMean { get; set; }
        public int WordId { get; set; }
        public Word Word { get; set; }
    }
}
