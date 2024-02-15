namespace FluxoCaixa.Relatorio.API
{
    public static class RouteExtension
    {
        public static RouteGroupBuilder MapGroupPublic(this RouteGroupBuilder group)
        {
            group.MapGet("/{datasaldo}", async (DateTime datasaldo, RelatorioDbContext db) =>
            {
                DateTime dataSaldoDiaAnterior = datasaldo.AddDays(-1);
                var saldoDia = db.SaldoDia.Where(w => w.DataSaldo == datasaldo.Date).FirstOrDefault();
                var saldoDiaAnterior = db.SaldoDia.Where(w => w.DataSaldo == dataSaldoDiaAnterior.Date).FirstOrDefault();
                var listaTrasacoes = db.Transacoes.Where(w => w.DataTransacao.Date == datasaldo.Date).ToList();
                if (saldoDia != null)
                {
                    RelatorioSaldoDia rel = new RelatorioSaldoDia();
                    rel.SaldoDiaAtual = saldoDia;
                    rel.SaldoDiaAtual = saldoDiaAnterior;
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
