import {Component, EventEmitter, Output} from '@angular/core';
import {CommentService} from '../../services/comment.service';
import {Comment} from '../../models/comment.model';

@Component({
  selector: 'app-comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css']
})
export class CommentFormComponent {
  username: string = '';  // User Name
  email: string = '';     // Email
  homepage: string = '';  // Home Page (optional)
  captcha: string = '';   // CAPTCHA
  text: string = '';      // Text message
  captchaCode: string = '';
  captchaInput: string = '';

  // List of allowed HTML tags (for sanitization)
  allowedTags: string[] = ['<b>', '<i>', '<u>', '<strong>', '<em>'];

  ngOnInit(): void {
    this.generateCaptcha();
  }

  @Output() commentPosted: EventEmitter<void> = new EventEmitter<void>(); // Emit event when comment is posted

  constructor(private commentService: CommentService) {
  }

  generateCaptcha() {
    const canvas = document.getElementById('captchaCanvas') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d');
    const chars = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const captchaLength = 6;

    this.captchaCode = '';
    for (let i = 0; i < captchaLength; i++) {
      const randomChar = chars.charAt(Math.floor(Math.random() * chars.length));
      this.captchaCode += randomChar;
    }

    if (ctx) {
      // Style the canvas
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      ctx.font = '30px Arial';
      ctx.fillStyle = '#000';
      ctx.fillText(this.captchaCode, 10, 30);
    }
  }

  public onSubmit(form: any) {
    // Add your logic to send the comment to the API
    // const newComment: Comment = {
    //   userName: this.username,
    //   userEmail: this.email,
    //   text: this.text,
    //   selectedTags: [],
    // };

    // this.commentService.addComment(newComment).subscribe((response) => {
    //   console.log('Comment submitted successfully!');
    // });
    // this.resetForm();
  }

  // Sanitize text input by removing unwanted HTML tags
  sanitizeInput(event: any) {
    const input = event.target.value;
    const sanitizedInput = this.stripUnallowedTags(input);
    this.text = sanitizedInput;
  }

  // Utility method to strip disallowed HTML tags
  stripUnallowedTags(input: string): string {
    const allowedPattern = new RegExp(this.allowedTags.join('|'), 'gi');
    return input.replace(/<\/?[^>]+(>|$)/g, (match) => (allowedPattern.test(match) ? match : ''));
  }


  // Public method to reset the form fields
  public resetForm() {
    this.username = '';
    this.email = '';
    this.homepage = '';
    this.captcha = '';
    this.text = '';
    this.captchaInput = '';
    this.generateCaptcha(); // Reset CAPTCHA after submission
  }

  protected readonly name = name;
}
