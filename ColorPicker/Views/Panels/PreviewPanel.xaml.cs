using ColorPicker.ColorSpace;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for PreviewPanel.xaml
    /// </summary>
    public partial class PreviewPanel : UserControl, INotifyPropertyChanged {

        public PreviewPanel() {
            InitializeComponent();
        }

        #region EventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Alpha
        protected static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register(
                nameof(Alpha),
                typeof(double),
                typeof(PreviewPanel),
                new FrameworkPropertyMetadata(
                    0d,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnAlphaChanged
                )
            );

        private static void OnAlphaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is PreviewPanel ctrl) {
                ctrl.SetAlpha((double)e.NewValue, internal_only: true);
            }
        }

        public double Alpha {
            get => (double)GetValue(AlphaProperty);
            set {
                SetAlpha(value);
            }
        }

        private void SetAlpha(double value, bool internal_only = false) {
            RenderPanel(Color, value);

            if (!internal_only) {
                SetValue(AlphaProperty, value);
            }
        }
        #endregion

        #region Color
        protected static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(RGB),
                typeof(PreviewPanel),
                new FrameworkPropertyMetadata(
                    new RGB(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is PreviewPanel ctrl) {
                ctrl.SetColor((RGB)e.NewValue, internal_only: true);
            }
        }

        public RGB Color {
            get => (RGB)GetValue(ColorProperty);
            set {
                SetColor(value);
            }
        }

        private void SetColor(RGB value, bool internal_only = false) {
            RenderPanel(value, Alpha);

            if (!internal_only) {
                SetValue(ColorProperty, value);
            }
        }
        #endregion

        #region Panel events
        private void Panel_Loaded(object sender, RoutedEventArgs e) {
            RenderPanel(Color, Alpha);
        }

        private void Panel_SizeChanged(object sender, SizeChangedEventArgs e) {
            RenderPanel(Color, Alpha);
        }
        #endregion
    }
}
