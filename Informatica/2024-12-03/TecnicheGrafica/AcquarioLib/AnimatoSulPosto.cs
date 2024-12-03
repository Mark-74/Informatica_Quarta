using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AcquarioLib
{
    public class AnimatoSulPosto : Inanimato
    {
        private int AnimationIndex = 0;
        private List<Image> Frames;
        public AnimatoSulPosto(Canvas canvas, List<Image> frames)
            : base(canvas, frames[0])
        {
            Frames = frames;
        }

        private void NextFrame()
        {
            Image = Frames[AnimationIndex++];
        }
    }
}
