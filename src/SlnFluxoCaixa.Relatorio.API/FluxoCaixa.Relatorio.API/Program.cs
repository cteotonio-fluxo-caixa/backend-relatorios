using FluxoCaixa.Relatorio.API;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Serviço de Relatórios", Version = "v1" });
    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<RelatorioDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Routes.Map(app);

app.Run();



public class RelatorioDbContext : DbContext
{
    public DbSet<SaldoDia> SaldoDia { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    public RelatorioDbContext(DbContextOptions options) : base(options) { }

    public async Task<List<Transacao>>  GetTransacoesPorDataAsync(DateTime Data)
    {
        string sql = @"SELECT T.TransacaoId, T.Valor, T.DataTransacao, T.Descricao, CT.Nome AS NomeCategoria, MP.Nome AS NomeMetodoPagamento 
                       FROM Transacoes T
                       INNER JOIN CategoriasTransacoes CT ON T.CategoriaId = CT.CategoriaId
                       INNER JOIN MetodosPagamento MP ON T.MetodoPagamentoId = MP.MetodoPagamentoId 
                       WHERE CONVERT(Date, DataTransacao) = @DataTransacao "
        ;

        return await Transacoes.FromSqlRaw(sql, new SqlParameter("@DataTransacao", Data.Date)).ToListAsync();
    }
}

public class SaldoDia
{
    [JsonIgnore]
    [Key]
    public Guid SaldoDiaId { get; set; }
    public DateTime DataSaldo { get; set; }
    public decimal Saldo { get; set; }
}

public class RelatorioSaldoDia
{
    public SaldoDia SaldoDiaAnterior {  get; set; }

    public List<Transacao> Transacoes { get; set; }

    public SaldoDia SaldoDiaAtual { get; set; }
}

/// <summary>
/// Transações
/// </summary>
public class Transacao
{
    [JsonIgnore]
    [Key]
    public Guid TransacaoId { get; set; }
    /// <summary>
    /// Valor da transação
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Data da Transação
    /// </summary>
    public DateTime DataTransacao { get; set; }

    /// <summary>
    /// Descrição
    /// </summary>
    public string Descricao { get; set; }

    /// <summary>
    /// Categoria
    /// </summary>
    public string NomeCategoria { get; set; }

    /// <summary>
    /// Método de Pagamento
    /// </summary>
    public string NomeMetodoPagamento { get; set; }

}