using XtraBlogWebsite.Models;

namespace XtraBlogWebsite.ViewsModel
{
    public class PostVM
    {
        public Post Post { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Post>? RelatiedPosts { get; set; }
        public Comment? Comment { get; set; }
    }
}
