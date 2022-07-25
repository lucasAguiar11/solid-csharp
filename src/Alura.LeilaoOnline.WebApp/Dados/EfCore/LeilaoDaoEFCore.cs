using System.Collections.Generic;
using System.Linq;
using Alura.LeilaoOnline.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Alura.LeilaoOnline.WebApp.Dados.EfCore
{
    public class LeilaoDaoEFCore : ILeilaoDao
    {
        private AppDbContext _context;

        public LeilaoDaoEFCore()
        {
            _context = new AppDbContext();
        }

        public IEnumerable<Categoria> BuscarCategorias()
        {
            return _context.Categorias.ToList();
        }

        public IEnumerable<Leilao> BuscarLeiloes()
        {
            return _context.Leiloes
                .Include(l => l.Categoria);
        }

        public Leilao BuscarPorId(int id)
        {
            return _context.Leiloes.First(l => l.Id == id);
        }

        public void SalvarLeitao(Leilao leilao)
        {
            _context.Leiloes.Add(leilao);
            _context.SaveChanges();
        }

        public void AtualizarLeitao(Leilao leilao)
        {
            _context.Leiloes.Update(leilao);
            _context.SaveChanges();
        }

        public void DeletarLeitao(Leilao leilao)
        {
            _context.Leiloes.Remove(leilao);
            _context.SaveChanges();
        }

        public IEnumerable<Leilao> PesquisarLeilao(string termo)
        {
            return _context.Leiloes
                .Include(l => l.Categoria)
                .Where(l => string.IsNullOrWhiteSpace(termo) ||
                            l.Titulo.ToUpper().Contains(termo.ToUpper()) ||
                            l.Descricao.ToUpper().Contains(termo.ToUpper()) ||
                            l.Categoria.Descricao.ToUpper().Contains(termo.ToUpper())
                );
        }
    }
}