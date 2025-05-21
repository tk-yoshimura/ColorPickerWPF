// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class SliderValueChangedEventArgs : EventArgs {
        public double Value { private set; get; }

        public bool UserOperation { private set; get; }

        public SliderValueChangedEventArgs(double value, bool user_operation) {
            this.Value = value;
            this.UserOperation = user_operation;
        }
    }
}
