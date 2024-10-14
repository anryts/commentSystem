import {Component, Input} from '@angular/core';
import {Comment} from '../../models/comment.model';
import {CommentService} from '../../services/comment.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent {
  @Input() comment: Comment | undefined;
  isReplyFormVisible: boolean = false;
  parentCommentId: number | undefined;
  selectedText: string = '';
  isModalOpen: boolean = false; // Control modal visibility
  selectedImageContent: string | null = null; // Base64 content of the selected image
  constructor(private commentService: CommentService, private sanitizer: DomSanitizer) {
  }

  toggleReplyForm() {
    this.isReplyFormVisible = !this.isReplyFormVisible;
  }

  openModal(imageContent: string): void {
    this.selectedImageContent = imageContent; // Set the selected image content
    this.isModalOpen = true; // Open the modal
  }

  closeModal(): void {
    this.isModalOpen = false; // Close the modal
    this.selectedImageContent = null; // Clear the image content
  }

  // Method to sanitize HTML
  get sanitizedHtml(): any {
    return this.sanitizer.bypassSecurityTrustHtml(this.comment!.text);
  }
}
