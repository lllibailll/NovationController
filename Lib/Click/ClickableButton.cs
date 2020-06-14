using System;
using System.Drawing;

namespace Lib.Click
{
    public class ClickableButton
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
        public Action LoadCallback { get; set; }
        public Action ClickCallback { get; set; }
    }
}