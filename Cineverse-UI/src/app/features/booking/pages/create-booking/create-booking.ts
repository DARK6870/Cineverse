import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { LoadingService } from '../../../../core/services/loading.service';
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
  templateUrl: 'create-booking.html',
  styleUrl: 'create-booking.css'
})
export class CreateBooking implements OnInit {
  screening = signal<Screening | null>(null);

  constructor(
    private screeningGraphQlService: ScreeningGraphqlService,
    private loadingService: LoadingService,
    private route: ActivatedRoute,
    private destroyRef: DestroyRef,
    private toastService: ToastService,
    private jwtClaimsService: JwtClaimsService,
    private router: Router
  ) {
  }

  async ngOnInit() {
    if ((await this.jwtClaimsService.decodeTokenAsync()).userStatus != UserStatus.Normal)
    {
      this.router.navigate(['/account']).then(() => {
        this.toastService.warning('Please confirm your email');
      });
    }
    this.loadingService.show();

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
        this.loadingService.hide();
      })
  }
}
