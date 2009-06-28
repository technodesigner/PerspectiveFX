using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Perspective.PixelShader.Filters
{
   public class ColorContrast:FilterBase
    {
        EffectProperty Facteur;

        public ColorContrast()
        {
            Facteur = new EffectProperty(this);
            Facteur.Name = "Facteur";
            Facteur.WrapperType = typeof(double);
            Facteur.DefaultValue = "0.5";
            Properties.Add(Facteur);
        }

        public override void MainHlsl(System.CodeDom.Compiler.IndentedTextWriter output)
        {
            output.WriteLine("color.rgb = 1.0 - exp(-(color.rgb * {0}));",this.Facteur.FullName);
        }
    }
}
