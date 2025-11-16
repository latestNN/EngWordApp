using System.ComponentModel.DataAnnotations;

namespace EngWordApp.Entity
{
    public class Word
    {
        [Key]
        public int WordId { get; set; }
        public string  EngWord { get; set; }
        public List<Mean> Means { get; set; }
        public decimal ScAnswerCount { get; set; } = 0;
        public decimal WrongAnswerCount { get; set; } = 0;
        public decimal SuccessPercent { get; set; } = 0;
        public int AskCount { get; set; } = 0;
        public bool IsAsked { get; set; } = false;
        public DateTime? LastAsked { get; set; }
        public string? WordLevel { get; set; }
        public string? Weak { get; set; }
        public bool? Connective { get; set; } = false;
        public int? Type { get; set; }

    }

    
}
