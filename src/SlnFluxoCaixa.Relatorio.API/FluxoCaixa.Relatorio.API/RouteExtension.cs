using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Xml;

namespace FluxoCaixa.Relatorio.API
{
    public static class RouteExtension
    {
        public static RouteGroupBuilder MapGroupPublic(this RouteGroupBuilder group)
        {
            group.MapGet("/saldodia/{datasaldo}", async (DateTime datasaldo, RelatorioDbContext db) =>
            {
                DateTime dataSaldoDiaAnterior = datasaldo.AddDays(-1);
                var saldoDia = db.SaldoDia.Where(w => w.DataSaldo == datasaldo.Date).FirstOrDefault();
                var saldoDiaAnterior = db.SaldoDia.Where(w => w.DataSaldo == dataSaldoDiaAnterior.Date).FirstOrDefault();
                var listaTrasacoes = await db.GetTransacoesPorDataAsync(datasaldo);
                if (saldoDia != null)
                {
                    RelatorioSaldoDia rel = new RelatorioSaldoDia();
                    rel.SaldoDiaAtual = saldoDia;
                    rel.SaldoDiaAnterior = saldoDiaAnterior;
                    rel.Transacoes = listaTrasacoes;
                    return Results.Ok(rel);
                }
                else
                    return Results.NoContent();
            }).Produces<RelatorioSaldoDia>(200);

            return group;
        }
    }
}
