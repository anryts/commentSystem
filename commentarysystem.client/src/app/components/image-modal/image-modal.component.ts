import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-image-modal',
  templateUrl: './image-modal.component.html',
  styleUrls: ['./image-modal.component.css']
})
export class ImageModalComponent {
  @Input() isOpen: boolean = false; // Determines if the modal is open
  @Input() imageContent: string | null = null; // Base64 content of the image to display
  @Output() close = new EventEmitter<void>(); // Event emitter for close action

  closeModal(): void {
    this.isOpen = false; // Close the modal
    this.imageContent = null; // Clear the image content
    this.close.emit(); // Emit close event to parent component
  }
}
