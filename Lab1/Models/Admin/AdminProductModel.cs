using Lab1.Data.Models;

namespace Lab1.Models.Admin
{
    public class AdminProductModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string User { get; set; }
        public DateTime UpdatedOn { get; set; }
        public ActivityEnum Activity { get; set; }
    }
}
