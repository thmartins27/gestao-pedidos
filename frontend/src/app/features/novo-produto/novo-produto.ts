import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CreateProduto } from '../../models/produto.model';
import { ProdutoService } from '../../core/services/produto.service';

@Component({
  selector: 'app-novo-produto',
  imports: [FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
  templateUrl: './novo-produto.html',
  styleUrl: './novo-produto.scss',
})
export class NovoProduto {
  nome = signal('');
  preco = signal<number | null>(null);
  estoqueAtual = signal<number | null>(null);
  salvando = signal(false);

  private produtoService = inject(ProdutoService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  formValido(): boolean {
    return this.nome().trim().length > 0 && (this.preco() ?? 0) > 0 && (this.estoqueAtual() ?? 0) > 0;
  }

  criarProduto(): void {
    if (!this.formValido() || this.salvando()) {
      return;
    }

    this.salvando.set(true);

    const dto: CreateProduto = {
      nome: this.nome().trim(),
      preco: this.preco()!,
      estoqueAtual: this.estoqueAtual()!,
    };

    this.produtoService.criar(dto).subscribe({
      next: () => {
        this.snackBar.open('Produto cadastrado com sucesso.', undefined, {
          panelClass: 'snackbar-sucesso',
        });
        this.router.navigate(['/produtos']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message, 'Fechar', {
          panelClass: 'snackbar-erro',
          duration: 8000,
        });
        this.salvando.set(false);
      },
    });
  }
}
