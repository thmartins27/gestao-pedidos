import { Component, input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-estado-erro',
  imports: [MatIconModule, MatButtonModule],
  templateUrl: './estado-erro.html',
  styleUrl: './estado-erro.scss',
})
export class EstadoErro {
  titulo = input('Não foi possível carregar');
  subtitulo = input('Verifique sua conexão e tente novamente.');
  tentarNovamente = output<void>();
}
