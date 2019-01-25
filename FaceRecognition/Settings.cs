using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FaceRecognition
{
    public static class Settings
    {
        public static Color[] ImageSquareColors
        {
            get
            {
                return new[] { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Cyan,
                               Color.DarkBlue, Color.Olive, Color.PaleVioletRed, Color.Pink};
            }
        }
    }
}
