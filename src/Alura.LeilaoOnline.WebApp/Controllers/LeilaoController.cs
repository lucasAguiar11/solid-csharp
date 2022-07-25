using Microsoft.AspNetCore.Mvc;
using Alura.LeilaoOnline.WebApp.Dados;
using Alura.LeilaoOnline.WebApp.Models;
using System;

namespace Alura.LeilaoOnline.WebApp.Controllers
{
    public class LeilaoController : Controller
    {
        ILeilaoDao _dao;

        public LeilaoController(ILeilaoDao dao)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            var leiloes = _dao.BuscarLeiloes();
            return View(leiloes);
        }

        [HttpGet]
        public IActionResult Insert()
        {
            ViewData["Categorias"] = _dao.BuscarCategorias();
            ViewData["Operacao"] = "Inclusão";
            return View("Form");
        }

        [HttpPost]
        public IActionResult Insert(Leilao model)
        {
            if (ModelState.IsValid)
            {
                _dao.SalvarLeitao(model);
                return RedirectToAction("Index");
            }

            ViewData["Categorias"] = _dao.BuscarCategorias();
            ViewData["Operacao"] = "Inclusão";
            return View("Form", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["Categorias"] = _dao.BuscarCategorias();
            ViewData["Operacao"] = "Edição";
            var leilao = _dao.BuscarPorId(id);
            if (leilao == null) return NotFound();
            return View("Form", leilao);
        }

        [HttpPost]
        public IActionResult Edit(Leilao model)
        {
            if (ModelState.IsValid)
            {
                _dao.AtualizarLeitao(model);
                return RedirectToAction("Index");
            }

            ViewData["Categorias"] = _dao.BuscarCategorias();
            ViewData["Operacao"] = "Edição";
            return View("Form", model);
        }

        [HttpPost]
        public IActionResult Inicia(int id)
        {
            var leilao = _dao.BuscarPorId(id);
            if (leilao == null) return NotFound();
            if (leilao.Situacao != SituacaoLeilao.Rascunho) return StatusCode(405);
            leilao.Situacao = SituacaoLeilao.Pregao;
            leilao.Inicio = DateTime.Now;
            _dao.AtualizarLeitao(leilao);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Finaliza(int id)
        {
            var leilao = _dao.BuscarPorId(id);
            if (leilao == null) return NotFound();
            if (leilao.Situacao != SituacaoLeilao.Pregao) return StatusCode(405);
            leilao.Situacao = SituacaoLeilao.Finalizado;
            leilao.Termino = DateTime.Now;
            _dao.AtualizarLeitao(leilao);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var leilao = _dao.BuscarPorId(id);
            if (leilao == null) return NotFound();
            if (leilao.Situacao == SituacaoLeilao.Pregao) return StatusCode(405);
            _dao.DeletarLeitao(leilao);
            return NoContent();
        }

        [HttpGet]
        public IActionResult Pesquisa(string termo)
        {
            ViewData["termo"] = termo;
            var leiloes = _dao.PesquisarLeilao(termo);
            return View("Index", leiloes);
        }
    }
}