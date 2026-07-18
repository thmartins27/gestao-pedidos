import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

interface ItemMenu {
  rota: string;
  icone: string;
  label: string;
}

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  menuAberto = signal(false);

  itensMenu: ItemMenu[] = [
    { rota: '/dashboard', icone: 'dashboard', label: 'Dashboard' },
    { rota: '/pedidos', icone: 'receipt_long', label: 'Pedidos' },
    { rota: '/novo-pedido', icone: 'add_circle', label: 'Novo Pedido' },
    { rota: '/produtos', icone: 'inventory_2', label: 'Produtos' },
    { rota: '/novo-produto', icone: 'add_box', label: 'Novo Produto' },
  ];

  toggleMenu(): void {
    this.menuAberto.update((aberto) => !aberto);
  }

  fecharMenu(): void {
    this.menuAberto.set(false);
  }
}
