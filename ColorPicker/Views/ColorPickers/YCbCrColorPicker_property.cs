// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class YCbCrColorPicker {

        private int margin_width = 4;
        public int MarginWidth {
            get => margin_width;
            set {
                if (value < 0) {
                    throw new ArgumentException("Must be non-negative.", nameof(MarginWidth));
                }

                margin_width = value;
                RenderAllImages();

                OnPropertyChanged(nameof(MarginWidth));
            }
        }

        public int Size => checked((int)double.Min(ActualWidth, ActualHeight));

        public int PixelSize => checked((int)Utils.ColorPickerUtil.GetPixelMinSize(this));

        protected int PickerPixelSize =>
            checked((int)(Utils.ColorPickerUtil.GetPixelMinSize(this) -
            MarginWidth * Utils.ColorPickerUtil.GetVisualScalingFactor(this).scaleX * 2));

        protected bool IsValidSize => PickerPixelSize >= 50;
    }
}
