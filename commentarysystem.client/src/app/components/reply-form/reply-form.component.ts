import {Component, Input, OnInit} from '@angular/core';
import {CommentService} from '../../services/comment.service';
import {Comment} from '../../models/comment.model';
import {Reply} from '../../models/comment.model';

@Component({
  selector: 'app-reply-form',
  templateUrl: './reply-form.component.html',
  styleUrls: ['./reply-form.component.css']
})
export class ReplyFormComponent implements OnInit {
  @Input() selectedText: string = ''; // Original comment text
  @Input() parentCommentId: number | undefined; // ID of the parent comment
  allowedHtmlTagsRegex = /^(<a\b[^>]*>.*?<\/a>|<code\b[^>]*>.*?<\/code>|<i\b[^>]*>.*?<\/i>|<strong\b[^>]*>.*?<\/strong>|[^<>]*)*$/;
  username: string = '';
  email: string = '';
  replyText: string = '';  // Holds the final reply text
  captchaCode: string = '';
  captchaInput: string = '';
  imageError: string = '';
  textFileError: string = '';
  uploadedImages: File[] = []; // Array to hold uploaded image URLs
  uploadedTextFiles: File[] = []; // Array to hold uploaded image URLs
  // Array to store the start/end indices and selected text
  selectedRanges: { startIndex: number; endIndex: number; selectedText: string }[] = [];
  selectedTexts: { selectedText: string }[] = [];

  constructor(private commentService: CommentService) {
  }

  ngOnInit(): void {
    this.generateCaptcha(); // Generate CAPTCHA when the component initializes
    this.setupTextSelectionListener(); // Setup the listener for text selection
  }

// Insert HTML tags at the cursor position
  insertTag(tag: string): void {
    const textArea = document.getElementById('replyText') as HTMLTextAreaElement;
    const start = textArea.selectionStart;
    const end = textArea.selectionEnd;

    const selectedText = textArea.value.substring(start, end);
    const before = textArea.value.substring(0, start);
    const after = textArea.value.substring(end, textArea.value.length);

    this.replyText = `${before}<${tag}>${selectedText}</${tag}>${after}`;
  }

  // Insert a link with a prompt for the URL
  insertLink(): void {
    const url = prompt("Enter the URL", "https://");
    if (url) {
      this.insertTag(`a href="${url}"`);
    }
  }

  // Handle image upload
  onImageChange(event: any): void {
    const files: File[] = Array.from(event.target.files); // Specify that this is an array of File objects
    this.uploadedImages = []; // Clear previous images
    this.imageError = ''; // Clear any previous errors

    const imagePromises: Promise<File>[] = files.map((file: File): Promise<File> => {
      return new Promise<File>((resolve, reject) => {
        const reader = new FileReader();
        const img = new Image();

        reader.onload = (e: ProgressEvent<FileReader>) => {
          img.src = e.target?.result as string; // Cast result to string
          img.onload = async () => {
            // Check dimensions
            if (img.width > 320 || img.height > 240) {
              try {
                const resizedFile = await this.resizeImage(img, 320, 240, file.name); // Resize image
                this.uploadedImages.push(resizedFile); // Add resized file to gallery
                resolve(resizedFile); // Resolve promise with resized file
              } catch (error) {
                reject(error); // Reject promise if there’s an error
              }
            } else {
              this.uploadedImages.push(file); // Add original file to gallery
              resolve(file); // Resolve promise with original file
            }
          };
        };

        reader.readAsDataURL(file);
      });
    });

    // Wait for all images to be processed
    Promise.all(imagePromises)
      .then(() => {
        alert('Images uploaded successfully:');
        console.log('Uploaded Images:', this.uploadedImages);
      })
      .catch((error) => {
        console.error('Error processing images:', error);
        this.imageError = 'Error processing images. Please check the console for details.';
      });
  }

  // Resize image to max dimensions
  resizeImage(img: HTMLImageElement, maxWidth: number, maxHeight: number, originalFileName: string): Promise<File> {
    return new Promise((resolve, reject) => {
      const canvas = document.createElement('canvas');
      const ctx = canvas.getContext('2d');

      if (ctx) {
        const aspectRatio = img.width / img.height;
        if (img.width > img.height) {
          canvas.width = maxWidth;
          canvas.height = maxWidth / aspectRatio;
        } else {
          canvas.height = maxHeight;
          canvas.width = maxHeight * aspectRatio;
        }

        // Draw the resized image on the canvas
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);

        // Convert canvas to Blob
        canvas.toBlob((blob) => {
          if (blob) {
            // Create a File object from the Blob
            const resizedFile = new File([blob], originalFileName, { type: 'image/jpeg' }); // Change MIME type as necessary
            this.uploadedImages.push(resizedFile); // Add to uploaded images array
            this.imageError = ''; // Clear error
            console.log('Resized image file:', resizedFile);
            resolve(resizedFile); // Resolve the promise with the File object
          } else {
            reject('Could not create blob from canvas.');
          }
        }, 'image/jpeg'); // You can change the format here as needed (e.g., 'image/png')
      } else {
        reject('Could not get canvas context.');
      }
    });
  }

  // Remove image from the gallery
  removeImage(image: File): void {
    this.uploadedImages = this.uploadedImages.filter(img => img !== image);
  }


  // Handle text file upload
  onTextFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      if (file.size > 100 * 1024) { // Check size
        this.textFileError = 'Text file should not exceed 100 KB.';
      } else {
        this.textFileError = ''; // Clear error
        const reader = new FileReader();
        reader.onload = (e: any) => {
          const content = e.target.result;
          console.log('Text file uploaded successfully:', content);
          // Process the text file content as needed
        };
        reader.readAsText(file);
      }
    }
    alert('Text file uploaded successfully:');
  }

  // Method to generate CAPTCHA and draw it on canvas
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

  // Handle input change for live preview
  onTextChange(): void {
    // You can also perform validation here if needed
    this.validateReplyText(); // Just to ensure validation is checked
  }

  // Listen for text selection and capture start/end indices
  setupTextSelectionListener() {
    document.addEventListener('mouseup', () => {
      const selection = window.getSelection();
      if (selection) {
        const selectedText = selection.toString();
        const range = selection.getRangeAt(0);  // Get the selected range

        if (selectedText) {
          const startIndex = this.selectedText.indexOf(selectedText);
          const endIndex = startIndex + selectedText.length;

          // Store the start and end index along with the selected text
          this.selectedRanges.push({startIndex, endIndex, selectedText});

          // Add the selected text to the array for quoting
          this.selectedTexts.push({selectedText});

          // Append the selected text to the reply text area as a quote
          this.replyText += `> ${selectedText}\n\n`;
        }
      }
    });
  }

  // Validate that all opened tags are properly closed
  validateTagClosure(): boolean {
    const tagStack: string[] = [];
    const tagPattern = /<\/?([a-zA-Z]+)(\s*[^>]*)?>/g; // Regex to match opening and closing tags
    let match;

    while ((match = tagPattern.exec(this.replyText)) !== null) {
      const tagName = match[1];

      // If it's a closing tag
      if (match[0].startsWith('</')) {
        if (tagStack.length === 0 || tagStack[tagStack.length - 1] !== tagName) {
          // Unmatched closing tag
          return false;
        }
        tagStack.pop(); // Pop the last opening tag
      } else if (['a', 'code', 'i', 'strong'].includes(tagName)) {
        // If it's an opening tag, push it to the stack
        tagStack.push(tagName);
      }
    }

    // If the stack is not empty, there are unclosed tags
    return tagStack.length === 0;
  }


  // This will validate the HTML tags
  validateReplyText(): boolean {
    return this.allowedHtmlTagsRegex.test(this.replyText) && this.validateTagClosure();
  }

  onSubmit(form: any) {
    if (this.validateReplyText() && form.valid) {
      const comment: Comment = {
        parentCommentId: this.parentCommentId,
        text: form.value.replyText,
        userName: form.value.username,
        userEmail: form.value.email,
        selectedRanges: this.selectedRanges
      };

      this.commentService.addComment(comment, this.uploadedImages, this.uploadedTextFiles)
        .subscribe((data) => {
          alert(data);
          this.resetForm();
        });

    } else {
      alert('Please fill in all the fields and enter the correct CAPTCHA code.');
    }
  }

  resetForm() {
    this.username = '';
    this.email = '';
    this.replyText = '';
    this.captchaInput = '';
    this.selectedRanges = [];  // Clear selected ranges
    this.selectedTexts = [];  // Clear selected texts
    this.generateCaptcha(); // Reset the CAPTCHA after form submission
  }
}
