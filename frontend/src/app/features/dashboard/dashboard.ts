import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { DashboardResumo } from '../../models/dashboard.model';
import { DashboardService } from '../../core/services/dashboard.service';
import { StatusChip } from '../../shared/components/status-chip/status-chip';
import { EstadoCarregando } from '../../shared/components/estado-carregando/estado-carregando';
import { EstadoErro } from '../../shared/components/estado-erro/estado-erro';

@Component({
  selector: 'app-dashboard',
  imports: [CurrencyPipe, StatusChip, EstadoCarregando, EstadoErro],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  resumo = signal<DashboardResumo | null>(null);
  carregando = signal(false);
  erro = signal(false);

  private dashboardService = inject(DashboardService);

  ngOnInit(): void {
    this.carregarResumo();
  }

  carregarResumo(): void {
    this.carregando.set(true);
    this.erro.set(false);

    this.dashboardService.obterResumo().subscribe({
      next: (resumo) => {
        this.resumo.set(resumo);
        this.carregando.set(false);
      },
      error: () => {
        this.erro.set(true);
        this.carregando.set(false);
      },
    });
  }
}
