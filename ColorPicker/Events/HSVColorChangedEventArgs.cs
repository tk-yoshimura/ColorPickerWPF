using ColorPicker.ColorSpace;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class HSVColorChangedEventArgs : EventArgs {
        public HSV Color { private set; get; }

        public bool UserOperation { private set; get; }

        public HSVColorChangedEventArgs(HSV color, bool user_operation) {
            this.Color = color;
            this.UserOperation = user_operation;
        }
    }
}
