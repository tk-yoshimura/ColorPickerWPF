using System.Windows.Media;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class PaletteClickedEventArgs : EventArgs {
        public Color Color { private set; get; }

        public PaletteClickedEventArgs(Color color) {
            this.Color = color;
        }
    }
}
