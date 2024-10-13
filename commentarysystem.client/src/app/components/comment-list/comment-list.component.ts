import { Component, Input, OnInit } from '@angular/core';
import { CommentService } from '../../services/comment.service';
import { Comment } from '../../models/comment.model';
import { apiSettings } from '../../../environments/environment.development';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.css'],
})
export class CommentListComponent implements OnInit {
  @Input() postId!: number;
  comments: Comment[] = [];

  // Pagination settings
  currentPage: number = 1;
  commentsPerPage: number = apiSettings.itemsPerPage;

  // Sort settings
  sortBy: string = 'date'; // Default sort field
  sortOrder: string = 'desc'; // Default sort order
  isNextButtonDisabled: boolean = false; // Track if the Next button should be disabled

  constructor(private commentService: CommentService) {}

  ngOnInit(): void {
    this.loadComments(this.currentPage); // Load comments for the first page on initialization
  }

  // Load comments for the current page with filtering and sorting
  loadComments(page: number): void {
    if (page < 1) return; // Ensure the page number is always positive

    this.commentService.getComments(this.commentsPerPage, page, this.sortBy, this.sortOrder)
      .subscribe((comments) => {
        this.comments = comments

        // Check if the length of comments is less than commentsPerPage
        this.isNextButtonDisabled = comments.length < this.commentsPerPage;
        this.currentPage = page; // Update the current page after loading
      });
  }

  // Handle sort application
  applySort(): void {
    this.loadComments(this.currentPage); // Reload comments with the selected sort options
  }

  // Handle page change when the user clicks left or right arrow
  changePage(page: number): void {
    if (page >= 1) {
      this.loadComments(page); // Make an API call to load comments for the new page
    }
  }
}
