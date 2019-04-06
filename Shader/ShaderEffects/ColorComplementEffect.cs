using System;
using System.Windows;

using System.Windows.Media;

using System.Windows.Media.Effects;

namespace MyEffects

{

    public class ColorComplementEffect : ShaderEffect

    {

        public ColorComplementEffect()

        {

            PixelShader = _shader;

            UpdateShaderValue(InputProperty);

        }


        public Brush Input

        {

            get { return (Brush)GetValue(InputProperty); }

            set { SetValue(InputProperty, value); }

        }


        public static readonly DependencyProperty InputProperty =

            ShaderEffect.RegisterPixelShaderSamplerProperty(

                    "Input",

                    typeof(ColorComplementEffect),

                    0);

        private static PixelShader _shader =

            new PixelShader() { UriSource = new Uri(@"pack://application:,,,/Shader;component/ColorComplementEffect.ps") };

    }

}
