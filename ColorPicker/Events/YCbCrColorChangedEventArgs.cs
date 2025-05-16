using ColorPicker.ColorSpace;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class YCbCrColorChangedEventArgs : EventArgs {
        public YCbCr YCbCr { private set; get; }

        public bool UserOperation { private set; get; }

        public YCbCrColorChangedEventArgs(YCbCr ycbcr, bool user_operation) {
            this.YCbCr = ycbcr;
            this.UserOperation = user_operation;
        }
    }

    public delegate void YCbCrColorChangedHandler(object sender, YCbCrColorChangedEventArgs cce);
}
