namespace XtraBlogWebsite.Models
{
    public class TagPost :BaseEntity
    {
        public int TagId { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public Tag? Tag { get; set; }
    }
}
