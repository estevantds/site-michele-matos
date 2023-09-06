using MiMatos.Context;
using MiMatos.Models;
using MiMatos.Repositories.Interfaces;

namespace MiMatos.Repositories
{
    public class PropriedadeRepository : IPropriedadeRepository
    {
        private readonly AppDbContext _context;

        public PropriedadeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Propriedade> Propriedades => _context.Propriedades;

        public IEnumerable<Propriedade> PropriedadesDisponiveis => _context.Propriedades.Where(p => p.ParaVenda || p.ParaLocacao);
        public IEnumerable<Propriedade> PropriedadesDisponiveisRecemAdicionadas()
        {
            var propriedades = _context.Propriedades.Where(p => p.ParaVenda || p.ParaLocacao).OrderByDescending(p => p.AtualizadoEm).Take(10).ToList();

            foreach (var propriedade in propriedades)
            {
                var imagens = _context.Imagens.Where(i => i.PropriedadeId == propriedade.PropriedadeId).ToList();

                if (imagens.Count() > 0)
                {
                    var destaques = imagens.Where(i => i.Destaque);
                    if (destaques.Count() > 0)
                    {
                        foreach (var imagem in destaques)
                        {
                            if (imagem.Destaque)
                            {
                                propriedade.CaminhoImagem = imagem.Caminho;
                            }
                        }
                    }
                    else
                    {
                        propriedade.CaminhoImagem = imagens.FirstOrDefault().Caminho;
                    }

                    foreach (var imagem in imagens)
                    {
                        if (imagem.Caminho != propriedade.CaminhoImagem)
                        {
                            if(propriedade.CaminhosImagens.Count() < 6)
                            {
                                propriedade.CaminhosImagens.Add(imagem.Caminho);
                            }
                        }
                    }
                }
                else
                {
                    propriedade.CaminhoImagem = "/images/preparando-imovel.png";
                }
            }

            return propriedades;
        }

        public Propriedade GetPropriedadeByCodigo(string codigo)
        {
            return _context.Propriedades.FirstOrDefault(p => p.Codigo == codigo);
        }
    }
}
