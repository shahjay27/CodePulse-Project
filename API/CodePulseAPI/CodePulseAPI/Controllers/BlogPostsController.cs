using CodePulseAPI.Models.Domain;
using CodePulseAPI.Models.DTO;
using CodePulseAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepo;
        private readonly ICategoryRepository categoryRepo;

        public BlogPostsController(IBlogPostRepository blogPostRepo, ICategoryRepository categoryRepo)
        {
            this.blogPostRepo = blogPostRepo;
            this.categoryRepo = categoryRepo;
        }

        [HttpPost]
        [Authorize(Roles="Writer")]
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
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await this.categoryRepo.FindByIdAsync(categoryGuid);
                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

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
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Categories = blogPost.Categories
                   .Select(x => new CategoryDto
                   {
                       Id = x.Id,
                       Name = x.Name,
                       UrlHandle = x.UrlHandle
                   }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost()
        {
            var blogPosts = await this.blogPostRepo.GetAllAsync();

            //to dto
            var response = new List<BlogPostDto>();

            foreach (var blogpost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogpost.Id,
                    Title = blogpost.Title,
                    Author = blogpost.Author,
                    Content = blogpost.Content,
                    Description = blogpost.Description,
                    IsVisible = blogpost.IsVisible,
                    UrlHandle = blogpost.UrlHandle,
                    PublishedDate = blogpost.PublishedDate,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    Categories = blogpost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostByID([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepo.GetByIdAsync(id);

            if (blogPost is null)
                return NotFound();

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
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Categories = blogPost.Categories
                   .Select(x => new CategoryDto
                   {
                       Id = x.Id,
                       Name = x.Name,
                       UrlHandle = x.UrlHandle
                   }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{url}")]
        public async Task<IActionResult> GetBlogPostByUrl([FromRoute] string url)
        {
            var blogPost = await blogPostRepo.GetByUrlHandleAsync(url);

            if (blogPost is null)
                return NotFound();

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
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Categories = blogPost.Categories
                   .Select(x => new CategoryDto
                   {
                       Id = x.Id,
                       Name = x.Name,
                       UrlHandle = x.UrlHandle
                   }).ToList()
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id,[FromBody] UpdateBlogPostRequestDto request)
        {
            // From DTO to Domain model
            var blogPost = new BlogPost
            {
                Id = id,
                Title = request.Title,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                PublishedDate = request.PublishedDate,
                Description = request.Description,
                IsVisible = request.IsVisible,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach(var categoryGuid in request.Categories)
            {
                var existingCat = await categoryRepo.FindByIdAsync(categoryGuid);

                if(existingCat != null)
                    blogPost.Categories.Add(existingCat);
            }

            // call repo to update
            var updateBlogPost = await blogPostRepo.UpdateBlogPostAsync(blogPost);

            if(updateBlogPost == null)
                return NotFound();

            // Convert to DTO
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
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Categories = blogPost.Categories
                   .Select(x => new CategoryDto
                   {
                       Id = x.Id,
                       Name = x.Name,
                       UrlHandle = x.UrlHandle
                   }).ToList()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var deletedBlogPost = await blogPostRepo.DeleteAsync(id);

            if(deletedBlogPost == null) return NotFound();

            //domain to DTO
            var response = new BlogPostDto
            {
                Id = deletedBlogPost.Id,
                Title = deletedBlogPost.Title,
                Author = deletedBlogPost.Author,
                Content = deletedBlogPost.Content,
                Description = deletedBlogPost.Description,
                IsVisible = deletedBlogPost.IsVisible,
                UrlHandle = deletedBlogPost.UrlHandle,
                PublishedDate = deletedBlogPost.PublishedDate,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl
            };

            return Ok(response);
        }
    }
}
