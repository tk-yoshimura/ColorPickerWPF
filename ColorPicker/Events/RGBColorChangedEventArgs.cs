using ColorPicker.ColorSpace;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class RGBColorChangedEventArgs : EventArgs {
        public RGB Color { private set; get; }

        public bool UserOperation { private set; get; }

        public RGBColorChangedEventArgs(RGB color, bool user_operation) {
            this.Color = color;
            this.UserOperation = user_operation;
        }
    }

    public delegate void RGBColorChangedHandler(object sender, RGBColorChangedEventArgs cce);
}
