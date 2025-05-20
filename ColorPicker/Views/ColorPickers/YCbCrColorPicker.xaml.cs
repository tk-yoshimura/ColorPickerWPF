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

        #region EventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<YCbCrColorChangedEventArgs> YCbCrColorChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

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

        #region ColorPicker events
        private void ColorPicker_Loaded(object sender, RoutedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(Size));
        }

        private void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(Size));
        }
        #endregion
    }
}
