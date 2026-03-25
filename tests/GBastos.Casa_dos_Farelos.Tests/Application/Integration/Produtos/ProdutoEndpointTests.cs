using FluentAssertions;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Request;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Integration.Produtos;

public class ProdutoEndpointTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly Guid _mockCategoriaId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public ProdutoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    // ---------------- CREATE ----------------
    [Fact]
    public async Task Deve_criar_produto_com_sucesso()
    {
        var produto = await CriarProdutoAsync("Ração", "Ração para cães", 15m);

        produto.Should().NotBeNull();
        produto.Nome.Should().Be("Ração");
        produto.Descricao.Should().Be("Ração para cães");
        produto.Preco.Should().Be(15m);
    }

    // ---------------- READ ----------------
    [Fact]
    public async Task Deve_listar_produtos()
    {
        await CriarProdutoAsync("Ração A", "Ração A", 10m);
        await CriarProdutoAsync("Ração B", "Ração B", 20m);

        var response = await _client.GetAsync("/api/produtos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var produtos = await response.Content.ReadFromJsonAsync<List<ProdutoResponse>>();
        produtos.Should().NotBeNull();
        produtos!.Count.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Deve_obter_produto_por_id()
    {
        var produtoCriado = await CriarProdutoAsync("Ração X", "Ração X", 20m);

        var getResponse = await _client.GetAsync($"/api/produtos/{produtoCriado.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var produto = await getResponse.Content.ReadFromJsonAsync<ProdutoResponse>();
        produto!.Id.Should().Be(produtoCriado.Id);
    }

    // ---------------- UPDATE ----------------
    [Fact]
    public async Task Deve_atualizar_produto()
    {
        var produtoCriado = await CriarProdutoAsync("Ração", "Ração simples", 10m);

        var updateRequest = CriarRequest("Ração Premium", "Ração super premium", 20m, _mockCategoriaId, 5);
        var putResponse = await _client.PutAsJsonAsync($"/api/produtos/{produtoCriado.Id}", updateRequest);
        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var produtoAtualizado = await _client.GetFromJsonAsync<ProdutoResponse>($"/api/produtos/{produtoCriado.Id}");
        produtoAtualizado!.Nome.Should().Be("Ração Premium");
        produtoAtualizado.Descricao.Should().Be("Ração super premium");
        produtoAtualizado.Preco.Should().Be(20m);
    }

    // ---------------- DELETE ----------------
    [Fact]
    public async Task Deve_remover_produto()
    {
        var produtoCriado = await CriarProdutoAsync("Ração", "Ração simples", 10m);

        var deleteResponse = await _client.DeleteAsync($"/api/produtos/{produtoCriado.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/produtos/{produtoCriado.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ---------------- VALIDATION ----------------
    [Theory]
    [InlineData("", "Ração para cães", 10, "Nome do produto obrigatório.")]
    [InlineData("Ração", "", 10, "A descrição do Produto é obrigatória.")]
    [InlineData("Ração", "Ração para cães", 0, "Preço inválido.")]
    public async Task Nao_deve_criar_produto_com_dados_invalidos(
        string nome,
        string descricao,
        decimal preco,
        string expectedError)
    {
        var request = CriarRequest(nome, descricao, preco, _mockCategoriaId, 5);
        var response = await _client.PostAsJsonAsync("/api/produtos", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        errors.Should().NotBeNull();
        errors!.Errors.Should().Contain(expectedError);
    }

    // ---------------- HELPERS ----------------
    private CreateProdutoRequest CriarRequest(
        string nome,
        string descricao,
        decimal preco,
        Guid categoriaId,
        int quantEstoque)
        => new(nome, descricao, preco, categoriaId, quantEstoque);

    private async Task<ProdutoResponse> CriarProdutoAsync(
        string nome = "Ração",
        string descricao = "Ração para cães",
        decimal preco = 10m,
        int quantEstoque = 5)
    {
        var request = CriarRequest(nome, descricao, preco, _mockCategoriaId, quantEstoque);
        var response = await _client.PostAsJsonAsync("/api/produtos", request);
        response.EnsureSuccessStatusCode();

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        var produtoResponse = await _client.GetFromJsonAsync<ProdutoResponse>($"/api/produtos/{id}");
        return produtoResponse!;
    }

    private record ValidationErrorResponse(string[] Errors);

    // ---------------- RESPONSE TYPE ----------------
    public record ProdutoResponse(
        Guid Id,
        string Nome,
        string Descricao,
        decimal Preco,
        Guid CategoriaId,
        int QuantEstoque
    );
}