import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css'],
})
export class EditBlogpostComponent implements OnInit, OnDestroy {
  id: string | null = null;
  model?: BlogPost;
  categories$?: Observable<Category[]>;
  selectedCategories?: string[];
  isImageSelectorVisible: boolean = false;

  routeSubs?: Subscription;
  blogPostServiceSubs?: Subscription;
  updateBlogPostSubs?: Subscription;
  deleteBlogPostSub?: Subscription;
  imageSelectSub?:Subscription;

  constructor(
    private route: ActivatedRoute,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService,
    private router: Router,
    private imageService: ImageService
  ) {}

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();

    this.route.paramMap.subscribe({
      next: (res) => {
        this.id = res.get('id');

        //Get blog post from API
        if (this.id) {
          this.blogPostServiceSubs = this.blogPostService
            .getBlogPostById(this.id)
            .subscribe({
              next: (res) => {
                this.model = res;
                this.selectedCategories = res.categories.map((x) => x.id);
              },
            });
        }

        this.imageSelectSub = this.imageService.onSelectImage().subscribe({
          next: (res) => {
            if(this.model){
              this.model.featuredImageUrl = res.url;
              this.isImageSelectorVisible = false;
            }
          }
        })

      },
    });
  }

  ngOnDestroy(): void {
    this.routeSubs?.unsubscribe();
    this.blogPostServiceSubs?.unsubscribe();
    this.updateBlogPostSubs?.unsubscribe();
    this.deleteBlogPostSub?.unsubscribe();
    this.imageSelectSub?.unsubscribe();
  }

  onDelete(): void {
    if (this.id) {
      this.deleteBlogPostSub = this.blogPostService
        .deleteBlogPost(this.id)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/blogposts');
          },
        });
    }
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  onFormSubmit(): void {
    // Convet into Req object
    if (this.model && this.id) {
      var updateBlogPost: UpdateBlogPost = {
        author: this.model.author,
        content: this.model.content,
        description: this.model.description,
        featuredImageUrl: this.model.featuredImageUrl,
        isVisible: this.model.isVisible,
        publishedDate: this.model.publishedDate,
        title: this.model.title,
        urlHandle: this.model.urlHandle,
        categories: this.selectedCategories ?? [],
      };

      this.updateBlogPostSubs = this.blogPostService
        .updateBlogPost(this.id, updateBlogPost)
        .subscribe({
          next: (res) => {
            this.router.navigateByUrl('/admin/blogposts');
          },
        });
    }
  }
}
