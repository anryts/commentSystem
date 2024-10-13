export interface Comment {
  commentId?: number;
  text: string;
  userName: string | null;
  userEmail: string | null;
  createdAt?: string; // ISO date string
  parentCommentId?: number | null; // Nullable for parent comments
  imageUrls?: string[]; // URLs for uploaded images
  textFileUrls?: string[]; // URLs for uploaded text files
  replies?: Comment[]; // Recursively nested replies
  selectedRanges: { startIndex: number; endIndex: number }[]; // Array of selected text ranges
  files?: { content: string; fileType: string; fileName: string }[]; // Array of file data with types
}

export interface Reply {
  parentCommentId: number;     // ID of the parent comment
  text: string;                // The content of the reply
  userName: string;            // Username of the user who posted the reply
  userEmail: string;           // Email of the user who posted the reply
  selectedRanges: { startIndex: number; endIndex: number }[]; // Array of selected text ranges
}
