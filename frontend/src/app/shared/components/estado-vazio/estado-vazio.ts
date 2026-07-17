import { Component, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-estado-vazio',
  imports: [MatIconModule, MatButtonModule, RouterLink],
  templateUrl: './estado-vazio.html',
  styleUrl: './estado-vazio.scss',
})
export class EstadoVazio {
  icone = input('inbox');
  titulo = input.required<string>();
  subtitulo = input('');
  acaoLabel = input<string>();
  acaoRota = input<string>();
}
