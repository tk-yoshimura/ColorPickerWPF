using ColorPicker.ColorSpace;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for YCbCrColorPicker.xaml
    /// </summary>
    public partial class YCbCrColorPicker : UserControl, INotifyPropertyChanged {
        public YCbCrColorPicker() {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<YCbCrColorChangedEventArgs> YCbCrColorChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(YCbCr),
                typeof(YCbCrColorPicker),
                new FrameworkPropertyMetadata(
                    new YCbCr(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is YCbCrColorPicker ctrl) {
                ctrl.SetSelectedColor((YCbCr)e.NewValue, user_operation: false, internal_only: true);
            }
        }

        public YCbCr SelectedColor {
            get => (YCbCr)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value, user_operation: false);
            }
        }

        YCbCr current_color = new();
        protected void SetSelectedColor(YCbCr color, bool user_operation, bool internal_only = false) {
            if (current_color.Y != color.Y) {
                current_color = color;

                RenderPointer(color);
                RenderCbCr(color);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    YCbCrColorChanged?.Invoke(this, new YCbCrColorChangedEventArgs(color, user_operation));
                }
            }
            else if (current_color.Cb != color.Cb || current_color.Cr != color.Cr) {
                current_color = color;

                RenderPointer(color);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    YCbCrColorChanged?.Invoke(this, new YCbCrColorChangedEventArgs(color, user_operation));
                }
            }
        }
        #endregion

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

        protected int PickerPixelSize => checked((int)(Utils.ColorPickerUtil.GetPixelMinSize(this) - MarginWidth * Utils.ColorPickerUtil.GetVisualScalingFactor(this).scaleX * 2));

        private void ColorPicker_Loaded(object sender, RoutedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(Size));
        }

        private void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(Size));
        }

        public void RenderAllImages() {
            RenderCbCr(SelectedColor);
            RenderPointer(SelectedColor);
        }

        protected bool IsValidSize => PickerPixelSize >= 50;
    }
}
