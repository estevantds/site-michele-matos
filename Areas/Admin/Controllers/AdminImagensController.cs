using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;
using MiMatos.ViewModels;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminImagensController : Controller
    {
        private string _serverPath;
        private readonly AppDbContext _context;

        public AdminImagensController(IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _serverPath = webHostEnvironment.WebRootPath;
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var images = _context.Imagens.Where(p => p.PropriedadeId == id);

            return View(images);
        }

        public IActionResult Upload(int? id)
        {
            ImagemViewModel model = new ImagemViewModel();
            if(id != null)
            {
                model.PropriedadeId = id;
            }
            else
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> images, ImagemViewModel model)
        {
            var caminhoImagem = _serverPath + "/imagens/arquivosPropriedades/";
            List<Imagem> listaImagens = new List<Imagem>();

            if (!Directory.Exists(caminhoImagem))
            {
                Directory.CreateDirectory(caminhoImagem);
            }

            if (model.PropriedadeId == null)
            {
                return NotFound();
            }

            foreach (var image in images)
            {
                var imageName = Guid.NewGuid().ToString() + "_" + image.FileName;

                using (var stream = System.IO.File.Create(caminhoImagem + imageName))
                {
                    await image.CopyToAsync(stream);
                }

                listaImagens.Add(new Imagem
                {
                    Caminho = "/imagens/arquivosPropriedades/" + imageName,
                    CriadoEm = DateTime.Now,
                    PropriedadeId = model.PropriedadeId ?? 0
                });
            }
            _context.Imagens.AddRange(listaImagens);

            _context.SaveChanges();
            return RedirectToAction("Index", "AdminPropriedades");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Imagens
                .FirstOrDefaultAsync(m => m.ImagemId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var imagem = await _context.Imagens.FindAsync(id);
            int propriedadeId = 0;
            if (imagem != null)
            {
                propriedadeId = imagem.PropriedadeId;
                _context.Imagens.Remove(imagem);
            } 

            await _context.SaveChangesAsync();

            if (System.IO.File.Exists(_serverPath + imagem.Caminho))
            {
                System.IO.File.Delete(_serverPath + imagem.Caminho);
            }

            return RedirectToRoute(new { action = "Index", controller = "AdminImagens", id = propriedadeId });
        }

        public async Task<IActionResult> Destaque(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Imagens.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        [HttpPost, ActionName("Destaque")]
        public async Task<IActionResult> Destaque(int id)
        {
            var imagem = await _context.Imagens.FindAsync(id);
            if(imagem == null)
            {
                return NotFound();
            }

            if (!imagem.Destaque)
            {
                var destaqueAntigo = new Imagem();
                destaqueAntigo = _context.Imagens.FirstOrDefault(i => i.PropriedadeId == imagem.PropriedadeId && i.Destaque);

                imagem.Destaque = true;
                _context.Update(imagem);

                if (destaqueAntigo != null)
                {
                    destaqueAntigo.Destaque = false;
                    _context.Update(destaqueAntigo);
                }

                _context.SaveChanges();
            }

            return RedirectToRoute(new { controller = "AdminImagens", action = "Index", id = imagem.PropriedadeId });
        }
    }
}
