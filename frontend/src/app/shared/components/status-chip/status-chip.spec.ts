import { TestBed } from '@angular/core/testing';
import { StatusChip } from './status-chip';

describe('StatusChip', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [StatusChip] });
  });

  it('deve exibir o texto do status', () => {
    const fixture = TestBed.createComponent(StatusChip);
    fixture.componentRef.setInput('status', 'Pago');
    fixture.detectChanges();

    const chip = fixture.nativeElement.querySelector('.status-chip') as HTMLElement;
    expect(chip.textContent).toContain('Pago');
  });

  it('deve aplicar a classe de cor conforme o status', () => {
    const fixture = TestBed.createComponent(StatusChip);
    fixture.componentRef.setInput('status', 'Cancelado');
    fixture.detectChanges();

    const chip = fixture.nativeElement.querySelector('.status-chip') as HTMLElement;
    expect(chip.classList).toContain('status-cancelado');
  });
});
