using ColorPicker.ColorSpace;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class RGBColorChangedEventArgs : EventArgs {
        public RGB RGB { private set; get; }

        public bool UserOperation { private set; get; }

        public RGBColorChangedEventArgs(RGB RGB, bool user_operation) {
            this.RGB = RGB;
            this.UserOperation = user_operation;
        }
    }

    public delegate void RGBColorChangedHandler(object sender, RGBColorChangedEventArgs cce);
}
