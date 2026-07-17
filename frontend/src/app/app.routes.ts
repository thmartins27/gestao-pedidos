import { Routes } from '@angular/router';
import { Component } from '@angular/core';
import { Pedidos } from './features/pedidos/pedidos';

@Component({
  selector: 'app-placeholder',
  template: '<p>Em contrução {{nome}}</p>',
})
class Placeholder {
  nome = '';
}

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: Placeholder, data: { nome: 'Dashboard' } },
  { path: 'pedidos', component: Pedidos, data: { nome: 'Pedidos' } },
  { path: 'novo-pedido', component: Placeholder, data: { nome: 'Novo Pedido' } },
];
