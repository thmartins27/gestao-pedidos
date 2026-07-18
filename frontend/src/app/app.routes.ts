import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard').then((m) => m.Dashboard),
  },
  {
    path: 'pedidos',
    loadComponent: () => import('./features/pedidos/pedidos').then((m) => m.Pedidos),
  },
  {
    path: 'novo-pedido',
    loadComponent: () => import('./features/novo-pedido/novo-pedido').then((m) => m.NovoPedido),
  },
  {
    path: 'produtos',
    loadComponent: () => import('./features/produtos/produtos').then((m) => m.Produtos),
  },
  {
    path: 'novo-produto',
    loadComponent: () =>
      import('./features/novo-produto/novo-produto').then((m) => m.NovoProduto),
  },
];
