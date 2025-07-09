using CodePulseAPI.Data;
using CodePulseAPI.Models.Domain;
using CodePulseAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulseAPI.Repositories.Implementaton
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public BlogPostRepository(ApplicationDbContext dbContext) 
        {
            this.dbcontext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbcontext.BlogPosts.AddAsync(blogPost);
            await dbcontext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbcontext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if(existingBlogPost!=null)
            {
                dbcontext.BlogPosts.Remove(existingBlogPost);
                await dbcontext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var blogPosts = await dbcontext.BlogPosts.Include(x=>x.Categories).ToListAsync();
            return blogPosts;
        }

        public async Task<BlogPost> GetByIdAsync(Guid id)
        {
            var blogPost = await dbcontext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == id);
            return blogPost;
        }

        public async Task<BlogPost> UpdateBlogPostAsync(BlogPost blogPost)
        {
            var existingBlogPost = await dbcontext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost == null)
                return null;

            // Update BlogPost
            dbcontext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            // Update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await dbcontext.SaveChangesAsync();

            return blogPost;
        }
    }
}
