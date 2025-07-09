export interface UpdateBlogPost {
  title: string;
  description: string;
  content: string;
  featuredImageUrl: string;
  urlHandle: string;
  author: string;
  publishedDate: Date;
  isVisible: boolean;
  categories: string[];
}
