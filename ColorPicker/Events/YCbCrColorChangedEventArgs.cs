using ColorPicker.ColorSpace;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class YCbCrColorChangedEventArgs : EventArgs {
        public YCbCr Color { private set; get; }

        public bool UserOperation { private set; get; }

        public YCbCrColorChangedEventArgs(YCbCr color, bool user_operation) {
            this.Color = color;
            this.UserOperation = user_operation;
        }
    }

    public delegate void YCbCrColorChangedHandler(object sender, YCbCrColorChangedEventArgs cce);
}
