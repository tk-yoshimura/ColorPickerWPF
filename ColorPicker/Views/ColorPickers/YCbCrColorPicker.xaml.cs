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

        public static readonly DependencyProperty SelectedColorProperty =
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
                ctrl.SelectedColor = (YCbCr)e.NewValue;
            }
        }

        public YCbCr SelectedColor {
            get => (YCbCr)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value, user_operation: false);
            }
        }

        YCbCr prev_color = new();
        protected void SetSelectedColor(YCbCr color, bool user_operation) {
            if (prev_color.Y != color.Y) {
                SetValue(SelectedColorProperty, color);
                RenderCbCr();
            }
            else {
                SetValue(SelectedColorProperty, color);
            }

            prev_color = color;

            RenderPointer();

            YCbCrColorChanged?.Invoke(this, new YCbCrColorChangedEventArgs(SelectedColor, user_operation));

            OnPropertyChanged(nameof(SelectedColor));
        }

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

        protected int PickerSize => Size - MarginWidth * 2;

        private void ColorPicker_Loaded(object sender, RoutedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(Size));
        }

        private void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(Size));
        }

        public void RenderAllImages() {
            RenderCbCr();
            RenderPointer();
        }

        protected bool IsValidSize => PickerSize >= 50;
    }
}
