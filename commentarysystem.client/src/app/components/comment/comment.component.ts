import {Component, Input} from '@angular/core';
import {Comment} from '../../models/comment.model';
import {CommentService} from '../../services/comment.service';  // Import the Comment model

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
  constructor(private commentService: CommentService) {
  }

  toggleReplyForm() {
    this.isReplyFormVisible = !this.isReplyFormVisible;
  }

  // Capture selected text from the comment text
  captureSelectedText(event: any) {
    const selection = window.getSelection();
    this.selectedText = selection ? selection.toString() : '';
  }

  openModal(imageContent: string): void {
    this.selectedImageContent = imageContent; // Set the selected image content
    this.isModalOpen = true; // Open the modal
  }

  closeModal(): void {
    this.isModalOpen = false; // Close the modal
    this.selectedImageContent = null; // Clear the image content
  }

  // Highlight text in comment and insert it into the reply form
  highlightText() {
    const selectedText = window.getSelection()?.toString();
    if (selectedText) {
      this.selectedText = `> ${selectedText}\n\n` + this.selectedText; // Add selected text as a quote to the reply form
      this.isReplyFormVisible = true; // Automatically show reply form when text is selected
    }
  }
}
