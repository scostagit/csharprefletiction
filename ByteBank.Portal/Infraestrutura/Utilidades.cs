using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura
{
    public static class Utilidades
    {
        public static bool EhArquivo(string path)
        {
            //StringSplitOptions.RemoveEmptyEntries: remove partes vaizia no caso de // aspas duplas.
            var partePath = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var extension = partePath.Last();
            return extension.Contains(".");
        }

        public static string ConvertPathNomeAssembly(string path)
        {
            var prefixoAssembly = "ByteBank.Portal";
            var pathComPonstos = path.Replace("/", ".");
            var nomeCompleto = $"{prefixoAssembly}{pathComPonstos}";

            return nomeCompleto;
        }

        public static string ObterTipoDeConteudo(string path)
        {
            if (path.EndsWith(".css"))
                return "text/css; charset=uf8";

            if (path.EndsWith(".js"))
                return "application/js; charset=uf8";

            if (path.EndsWith(".html"))
                return "text/html; charset=uf8";

            throw new NotImplementedException("Tipo de conteudo não previsto!");
        }
    }
}
