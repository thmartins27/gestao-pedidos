import { Routes } from '@angular/router';
import { Component } from '@angular/core';

@Component({
  selector: 'app-placeholder',
  template: '<p>Em construção</p>',
})
class Placeholder {}

export const routes: Routes = [
  { path: '', redirectTo: 'pedidos', pathMatch: 'full' },
  { path: 'dashboard', component: Placeholder },
  {
    path: 'pedidos',
    loadComponent: () => import('./features/pedidos/pedidos').then((m) => m.Pedidos),
  },
  {
    path: 'novo-pedido',
    loadComponent: () => import('./features/novo-pedido/novo-pedido').then((m) => m.NovoPedido),
  },
];
