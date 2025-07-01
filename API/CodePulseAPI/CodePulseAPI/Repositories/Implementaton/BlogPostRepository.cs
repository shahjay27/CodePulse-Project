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

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var blogPosts = await dbcontext.BlogPosts.ToListAsync();
            return blogPosts;
        }
    }
}
