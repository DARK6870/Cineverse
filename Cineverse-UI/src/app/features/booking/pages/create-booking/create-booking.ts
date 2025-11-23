import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { Screening } from '../../../screening/api/screening.graphql.types';
import { firstValueFrom } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ToastService } from '../../../../core/services/toast.service';
import { JwtClaimsService } from '../../../../core/services/jwt-claims.service';
import { UserStatus } from '../../../../shared/models/jwt-payload.model';

@Component({
  selector: 'app-create-booking',
  imports: [],
  standalone: true,
  templateUrl: 'create-booking.html',
  styleUrl: 'create-booking.css'
})
export class CreateBooking implements OnInit {
  private screeningGraphQlService = inject(ScreeningGraphqlService);
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);
  private toastService = inject(ToastService);
  private jwtClaimsService = inject(JwtClaimsService);
  private router = inject(Router);


  screening = signal<Screening | null>(null);

  async ngOnInit() {
    if ((await this.jwtClaimsService.decodeTokenAsync()).userStatus != UserStatus.Normal)
    {
      this.router.navigate(['/profile']).then(() => {
        this.toastService.warning('Please confirm your email');
      });
    }
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(async paramMap => {
        const screeningId = paramMap.get("screeningId");
        if (!screeningId) return;

        const screening = await firstValueFrom(
          this.screeningGraphQlService.getScreeningById(screeningId)
        );

        if (screening === null){
          this.toastService.error('Movie was not found');
          return;
        }

        this.screening.set(screening);
      })
  }
}
