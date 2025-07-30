import { Component } from '@angular/core';
import { GraphqlService } from '../services/graphql';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-test-graphql',
  standalone: true,
  imports: [JsonPipe],
  templateUrl: './test-graphql.html',
  styleUrls: ['./test-graphql.css']
})
export class TestGraphqlComponent {
  response: any;
  error: string = '';
  isLoading = false;

  constructor(private graphqlService: GraphqlService) {}

  testConnection() {
    this.isLoading = true;
    this.response = null;
    this.error = '';

    this.graphqlService.testQuery().subscribe({
      next: (result) => {
        this.response = result;
        this.isLoading = false;
        console.log('GraphQL response:', result);
      },
      error: (err) => {
        this.error = err.message;
        this.isLoading = false;
        console.error('GraphQL error:', err);
      }
    });
  }
}
