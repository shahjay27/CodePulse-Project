import { Category } from '../../category/models/category.model';

export interface BlogPost {
  id: string;
  title: string;
  description: string;
  content: string;
  featuredImageUrl: string;
  urlHandle: string;
  author: string;
  publishedDate: Date;
  isVisible: boolean;
  categories: Category[];
}
