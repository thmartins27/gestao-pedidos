using GestaoPedidos.Api.DTOs.Pedidos;
using GestaoPedidos.Api.Exceptions;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Models.Enums;
using GestaoPedidos.Api.Repositories;

namespace GestaoPedidos.Api.Services;

public class PedidoService : IPedidoService
{

    private readonly IClienteRepository _clienteRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPedidoRepository _pedidoRepository;


    public PedidoService(
        IClienteRepository clienteRepository,
        IProdutoRepository produtoRepository,
        IPedidoRepository pedidoRepository
    )
    {
        _clienteRepository = clienteRepository;
        _produtoRepository = produtoRepository;
        _pedidoRepository = pedidoRepository;
    }

    public async Task<PagedResult<PedidoResumoDto>> ObterTodosAsync(
        int page, int pageSize, StatusPedido? status, CancellationToken ct = default
    )
    {

        var resultado = await _pedidoRepository.ObterTodosAsync(page, pageSize, status, ct);

        return new PagedResult<PedidoResumoDto>
        {
            Items = resultado.Items.Select(MapToResumoDto).ToList(),
            Page = resultado.Page,
            PageSize = resultado.PageSize,
            TotalItems = resultado.TotalItems
        };
    }
    
    public async Task<PedidoDto> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id, ct)
            ?? throw new NotFoundException($"Pedido com id {id} não encontrado");

        return MapToDto(pedido);
    }

    public async Task<PedidoDto> AtualizarStatusAsync(int id, UpdateStatusPedidoDto dto, CancellationToken ct = default)
    {
        var pedido = await _pedidoRepository.ObterParaAtualizacaoAsync(id, ct)
            ?? throw new NotFoundException($"Pedido com id {id} não encontrado");

        pedido.MudarStatus(dto.NovoStatus);

        if (dto.NovoStatus == StatusPedido.Cancelado)
        {
            foreach (var item in pedido.Itens)
                item.Produto.DevolverEstoque(item.Quantidade);
        }

        await _pedidoRepository.SalvarAlteracoesAsync(ct);

        var atualizado = await _pedidoRepository.ObterPorIdAsync(id, ct);
        return MapToDto(atualizado!);
    }

    public async Task<PedidoDto> CriarAsync(CreatePedidoDto dto, CancellationToken ct = default)
    {
        var cliente = await ObterClienteOuFalharAsync(dto.ClienteId, ct);
        var produtos = await ObterProdutosDoPedidoAsync(dto.Itens, ct);

        var pedido = MontarPedido(cliente.Id, produtos, dto.Itens);

        await _pedidoRepository.AdicionarAsync(pedido, ct);
        await _pedidoRepository.SalvarAlteracoesAsync(ct);

        return await ObterDtoCompletoAsync(pedido.Id, ct);

    }

    private async Task<Cliente> ObterClienteOuFalharAsync(int clienteId, CancellationToken ct = default)
        => await _clienteRepository.ObterPorIdAsync(clienteId, ct)
            ?? throw new NotFoundException($"Cliente com id {clienteId} não encontrado");


    private async Task<List<Produto>> ObterProdutosDoPedidoAsync(
        List<CreatePedidoItemDto> itens, CancellationToken ct
    )
    {
        var produtoIds = itens.Select(i => i.ProdutoId).Distinct().ToList();
        var produtos = await _produtoRepository.ObterPorIdsAsync(produtoIds, ct);

        var idsEncontrados = produtos.Select(p => p.Id).ToHashSet();
        var idsFaltando = produtoIds.Where(id => !idsEncontrados.Contains(id)).ToList();

        if (idsFaltando.Count > 0)
            throw new NotFoundException($"Produto(s) não encontrado(s): {string.Join(", ", idsFaltando)}.");

        return produtos;
    }

    private static Pedido MontarPedido(
        int clienteId, List<Produto> produtos, List<CreatePedidoItemDto> itens
    )
    {
        var pedido = new Pedido(clienteId);

        foreach (var itemDto in itens)
        {
            var produto = produtos.First(p => p.Id == itemDto.ProdutoId);

            produto.BaixarEstoque(itemDto.Quantidade);
            var item = new ItemPedido(produto, pedido.Id, itemDto.Quantidade);
            pedido.AdicionarItem(item);
        }

        return pedido;
    }


    private async Task<PedidoDto> ObterDtoCompletoAsync(int pedidoId, CancellationToken ct = default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId, ct);
        return MapToDto(pedido!);
    }

    private static PedidoResumoDto MapToResumoDto(Pedido p) => new()
    {
        Id = p.Id,
        ClienteId = p.ClienteId,
        NomeCliente = p.Cliente.Nome,
        DataPedido = p.DataPedido,
        Status = p.Status.ToString(),
        ValorTotal = p.ValorTotal
    };

    private static PedidoDto MapToDto(Pedido p) => new()
    {
        Id = p.Id,
        ClienteId = p.ClienteId,
        NomeCliente = p.Cliente.Nome,
        DataPedido = p.DataPedido,
        Status = p.Status.ToString(),
        ValorTotal = p.ValorTotal,
        Itens = p.Itens.Select(i => new ItemPedidoDto
        {
            ProdutoId = i.ProdutoId,
            NomeProduto = i.Produto.Nome,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            Subtotal = i.CalcularSubtotal()
        }).ToList()
    };
}