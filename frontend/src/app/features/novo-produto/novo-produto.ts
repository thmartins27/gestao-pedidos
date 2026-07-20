import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CreateProduto } from '../../models/produto.model';
import { ProdutoService } from '../../core/services/produto.service';

@Component({
  selector: 'app-novo-produto',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
  templateUrl: './novo-produto.html',
  styleUrl: './novo-produto.scss',
})
export class NovoProduto {
  salvando = signal(false);

  private fb = inject(FormBuilder);
  private produtoService = inject(ProdutoService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  form = this.fb.group({
    nome: this.fb.control('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(200)],
    }),
    preco: this.fb.control<number | null>(null, [Validators.required, Validators.min(0.01)]),
    estoqueAtual: this.fb.control<number | null>(null, [Validators.required, Validators.min(1)]),
  });

  criarProduto(): void {
    if (this.form.invalid || this.salvando()) {
      return;
    }

    this.salvando.set(true);

    const { nome, preco, estoqueAtual } = this.form.getRawValue();
    const dto: CreateProduto = {
      nome: nome.trim(),
      preco: preco!,
      estoqueAtual: estoqueAtual!,
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
