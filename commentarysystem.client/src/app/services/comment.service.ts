import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import {Observable} from 'rxjs';
import {Comment, Reply} from '../models/comment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  private apiUrl = `${environment.apiBaseUrl}/comments`;

  constructor(private http: HttpClient) { }

  addComment(comment: Comment, images: File[], textFiles: File[]): Observable<Comment> {
    const formData = new FormData();
    // Append comment data to the form data
    formData.append('userName', comment.userName!);
    formData.append('userEmail', comment.userEmail!);
    formData.append('text', comment.text);
    formData.append('parentCommentId', comment.parentCommentId?.toString() || '');

    // Append each image file
    images.forEach(image => {
      formData.append('files', image);
    });

    // Append each text file
    textFiles.forEach(textFile => {
      formData.append('files', textFile);
    });

    return this.http.post<Comment>(this.apiUrl, formData);
  }

  addReply(reply: Reply): Observable<Reply> {
    return this.http.post<Reply>(this.apiUrl + '/reply', reply);
  }

  getCommentById(postId: number): Observable<Comment> {
    return this.http.get<Comment>(`${this.apiUrl}/${postId}`);
  }

  getComments( numberOfComments: number = 25,
               pageNumber: number,
               filterBy?: string,       // Optional parameter for filtering by a field
               sortBy?: string
  ): Observable<Comment[]> {// Optional parameter for sorting) : Observable<Comment[]> {
    let url = `${this.apiUrl}?page=${pageNumber}&pageSize=${numberOfComments}`;

    // Append filtering parameters if provided
    if (filterBy) {
      url += `&filterBy=${filterBy}`;
    }

    // Append sorting parameters if provided
    if (sortBy) {
      url += `&sortBy=${sortBy}`;
    }

    return this.http.get<Comment[]>(url);

    //return this.http.get<Comment[]>(`${this.apiUrl}?page=${pageNumber}&pageSize=${numberOfComments}`);
  }
}
