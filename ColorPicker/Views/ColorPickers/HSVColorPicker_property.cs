// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class HSVColorPicker {

        public int Size => checked((int)double.Min(ActualWidth, ActualHeight));

        public int PixelSize => checked((int)Utils.ColorPickerUtil.GetPixelMinSize(this));

        protected bool IsValidSize => PixelSize >= 50;
    }
}
