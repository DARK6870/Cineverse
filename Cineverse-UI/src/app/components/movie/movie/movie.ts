import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-movie',
  imports: [],
  templateUrl: 'movie.html',
  styleUrl: 'movie.css'
})
export class Movie implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {

  }
    ngOnInit(): void {
      this.route.paramMap.subscribe(params => {
        const movieId = params.get('movieId');
        console.log(movieId);
      })
    }

}
