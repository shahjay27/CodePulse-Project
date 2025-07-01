using CodePulseAPI.Models.Domain;
using CodePulseAPI.Models.DTO;
using CodePulseAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepo;

        public BlogPostsController(IBlogPostRepository blogPostRepo)
        {
            this.blogPostRepo = blogPostRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            //dto to domain model
            var blogPost = new BlogPost
            {
                Title = request.Title,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                PublishedDate = request.PublishedDate,
                Description = request.Description,
                IsVisible = request.IsVisible,
                UrlHandle = request.UrlHandle
            };

            blogPost = await this.blogPostRepo.CreateAsync(blogPost);

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                Description = blogPost.Description,
                IsVisible = blogPost.IsVisible,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl
            };

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost()
        {
            var blogPost = await this.blogPostRepo.GetAllAsync();

            //to dto
            var response = new List<BlogPostDto>();

            foreach(var blogpost in blogPost)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogpost.Id,
                    Title= blogpost.Title,
                    Author = blogpost.Author,
                    Content = blogpost.Content,
                    Description = blogpost.Description,
                    IsVisible = blogpost.IsVisible,
                    UrlHandle = blogpost.UrlHandle,
                    PublishedDate = blogpost.PublishedDate,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl
                });
            }

            return Ok(response);
        }
    }
}
