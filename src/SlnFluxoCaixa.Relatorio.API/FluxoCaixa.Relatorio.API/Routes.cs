namespace FluxoCaixa.Relatorio.API
{
    public static class Routes
    {
        public static void Map(WebApplication app)
        {
            app.MapGroup("/public/relatorios").MapGroupPublic().WithTags("Relatorios");
            
        }
    }
}
