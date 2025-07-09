using ColorPicker.ColorSpace;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for HSVColorPicker.xaml
    /// </summary>
    public partial class HSVColorPicker : UserControl, INotifyPropertyChanged {

        public HSVColorPicker() {
            InitializeComponent();
        }

        #region EventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<HSVColorChangedEventArgs> HSVColorChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(HSV),
                typeof(HSVColorPicker),
                new FrameworkPropertyMetadata(
                    new HSV(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is HSVColorPicker ctrl) {
                ctrl.SetSelectedColor((HSV)e.NewValue, user_operation: false, internal_only: true);
            }
        }

        public HSV SelectedColor {
            get => (HSV)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value, user_operation: false);
            }
        }

        HSV current_color = new();
        protected void SetSelectedColor(HSV color, bool user_operation, bool internal_only = false) {
            if (current_color.H != color.H) {
                current_color = color;

                RenderPointer(color);
                RenderTriangle(color);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(color, user_operation));
                }

            }
            else if (current_color.S != color.S || current_color.V != color.V) {
                current_color = color;

                RenderPointer(color);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(color, user_operation));
                }
            }
        }
        #endregion

        #region HueConversionMode
        protected static readonly DependencyProperty HueConversionModeProperty =
            DependencyProperty.Register(
                nameof(HueConversionMode),
                typeof(HueConversionMode),
                typeof(HSVColorPicker),
                new FrameworkPropertyMetadata(
                    HueConversionMode.OstwaldPerceptual,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnHueConversionModeChanged
                )
            );

        private static void OnHueConversionModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is HSVColorPicker ctrl) {
                ctrl.HueConversionMode = (HueConversionMode)e.NewValue;
            }
        }

        private HueConversionMode hue_conversion_mode = HueConversionMode.OstwaldPerceptual;
        public HueConversionMode HueConversionMode {
            get => hue_conversion_mode;
            set {
                if (hue_conversion_mode != value) {
                    hue_conversion_mode = value;

                    SetValue(HueConversionModeProperty, value);

                    RenderRing();
                    RenderPointer(SelectedColor);
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
