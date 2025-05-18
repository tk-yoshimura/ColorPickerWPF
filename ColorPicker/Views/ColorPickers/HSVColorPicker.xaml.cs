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

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<HSVColorChangedEventArgs> HSVColorChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region SelectedColor
        public static readonly DependencyProperty SelectedColorProperty =
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
                ctrl.SelectedColor = (HSV)e.NewValue;
            }
        }

        public HSV SelectedColor {
            get => (HSV)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value, user_operation: false);
            }
        }

        HSV prev_color = new();
        protected void SetSelectedColor(HSV color, bool user_operation) {
            if (prev_color.H != color.H) {
                prev_color = color;
                
                SetValue(SelectedColorProperty, color);
                RenderTriangle();
            }
            else {
                prev_color = color;

                SetValue(SelectedColorProperty, color);
            }

            RenderPointer();

            HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(SelectedColor, user_operation));
        }
        #endregion

        public int Size => checked((int)double.Min(ActualWidth, ActualHeight));

        public int PixelSize => checked((int)Utils.ColorPickerUtil.GetPixelMinSize(this));

        private void ColorPicker_Loaded(object sender, RoutedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(Size));
        }

        private void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(Size));
        }

        public void RenderAllImages() {
            RenderRing();
            RenderTriangle();
            RenderPointer();
        }

        protected bool IsValidSize => PixelSize >= 50;
    }
}
