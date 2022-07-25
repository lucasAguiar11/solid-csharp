using System.Collections.Generic;
using Alura.LeilaoOnline.WebApp.Models;

namespace Alura.LeilaoOnline.WebApp.Dados
{
    public interface ILeilaoDao
    {
        public IEnumerable<Categoria> BuscarCategorias();
        public IEnumerable<Leilao> BuscarLeiloes();
        public Leilao BuscarPorId(int id);
        public void SalvarLeitao(Leilao leilao);
        public void AtualizarLeitao(Leilao leilao);
        public void DeletarLeitao(Leilao leilao);
        public IEnumerable<Leilao> PesquisarLeilao(string termo);
    }
}