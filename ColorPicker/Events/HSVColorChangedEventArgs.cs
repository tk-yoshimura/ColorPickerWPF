using ColorPicker.ColorSpace;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class HSVColorChangedEventArgs : EventArgs {
        public HSV HSV { private set; get; }

        public bool UserOperation { private set; get; }

        public HSVColorChangedEventArgs(HSV hsv, bool user_operation) {
            this.HSV = hsv;
            this.UserOperation = user_operation;
        }
    }

    public delegate void HSVColorChangedHandler(object sender, HSVColorChangedEventArgs cce);
}
