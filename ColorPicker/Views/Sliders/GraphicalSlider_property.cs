using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class GraphicalSlider {


        private double track_margin_width = 4d;
        public double TrackMarginWidth {
            get => track_margin_width;
            set {
                if (double.IsNegative(value)) {
                    throw new ArgumentException("Must be non-negative.", nameof(TrackMarginWidth));
                }

                track_margin_width = value;
                RenderAllImages();

                OnPropertyChanged(nameof(TrackMarginWidth));
                OnPropertyChanged(nameof(TrackMargin));
            }
        }

        private Size thumb_size = new(8d, 8d);
        public Size ThumbSize {
            get => thumb_size;
            set {
                thumb_size = value;
                RenderAllImages();

                OnPropertyChanged(nameof(ThumbSize));
            }
        }

        public Thickness TrackMargin => new(TrackMarginWidth, 0d, TrackMarginWidth, TrackMarginWidth);

        protected int PixelWidth => checked((int)(Utils.ColorPickerUtil.GetPixelSize(this).pixelWidth));
        protected int PixelHeight => checked((int)(Utils.ColorPickerUtil.GetPixelSize(this).pixelHeight));

        protected int TrackPixelWidth =>
            checked((int)(Utils.ColorPickerUtil.GetPixelSize(this).pixelWidth -
            TrackMarginWidth * Utils.ColorPickerUtil.GetVisualScalingFactor(this).scaleX * 2));
        protected int TrackPixelHeight =>
            checked((int)(Utils.ColorPickerUtil.GetPixelSize(this).pixelHeight -
            TrackMarginWidth * Utils.ColorPickerUtil.GetVisualScalingFactor(this).scaleY));

        protected bool IsValidSize => TrackPixelWidth >= 50 && TrackPixelHeight >= 2;
    }
}
