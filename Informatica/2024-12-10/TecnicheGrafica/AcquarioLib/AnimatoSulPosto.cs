using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AcquarioLib
{
    public class AnimatoSulPosto : Inanimato
    {
        private int AnimationIndex = 0;
        private List<Image> Frames;
        public AnimatoSulPosto(Canvas canvas, List<Image> frames, DispatcherTimer dispatcher)
            : base(canvas, frames[0], dispatcher)
        {
            Frames = frames;
        }

        private void NextFrame()
        {
            Image = Frames[AnimationIndex++];
        }
    }
}
