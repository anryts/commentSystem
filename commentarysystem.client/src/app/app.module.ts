import {HttpClientModule} from '@angular/common/http';
import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {CommentListComponent} from './components/comment-list/comment-list.component';
import {CommentComponent} from './components/comment/comment.component';
import {FormsModule} from '@angular/forms';
import {CommentFormComponent} from './components/comment-form/comment-form.component';
import { ReplyFormComponent } from './components/reply-form/reply-form.component';
import { ImageModalComponent } from './components/image-modal/image-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    CommentListComponent,
    CommentComponent,
    CommentFormComponent,
    ReplyFormComponent,
    ImageModalComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
