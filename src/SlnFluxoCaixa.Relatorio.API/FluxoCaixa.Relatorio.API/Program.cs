using FluxoCaixa.Relatorio.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
}

public class SaldoDia
{
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