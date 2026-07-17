import { Component, input } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-estado-carregando',
  imports: [MatProgressSpinnerModule],
  templateUrl: './estado-carregando.html',
  styleUrl: './estado-carregando.scss',
})
export class EstadoCarregando {
  mensagem = input('Carregando…');
}
