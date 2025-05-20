using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for GraphicalSlider.xaml
    /// </summary>
    public partial class GraphicalSlider : UserControl, INotifyPropertyChanged {

        public GraphicalSlider() {
            InitializeComponent();
        }

        #region EventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<SliderValueChangedEventArgs> SliderValueChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Value
        protected static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(GraphicalSlider),
                new FrameworkPropertyMetadata(
                    0d,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged
                )
            );

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is GraphicalSlider ctrl) {
                ctrl.SetValue((double)e.NewValue, user_operation: false, internal_only: true);
            }
        }

        public double Value {
            get => (double)GetValue(ValueProperty);
            set {
                SetValue(value, user_operation: false);
            }
        }

        double current_value = 0d;
        protected virtual void SetValue(double value, bool user_operation, bool internal_only = false) {
            value = double.Clamp(value, 0, 1);

            if (current_value != value) {
                current_value = value;

                RenderThumb(value);

                if (!internal_only) {
                    SetValue(ValueProperty, value);

                    SliderValueChanged?.Invoke(this, new SliderValueChangedEventArgs(value, user_operation));
                }
            }
        }
        #endregion

        #region Slider events
        private void Slider_Loaded(object sender, RoutedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(TrackPixelWidth));
            OnPropertyChanged(nameof(TrackPixelHeight));
        }

        private void Slider_SizeChanged(object sender, SizeChangedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(TrackPixelWidth));
            OnPropertyChanged(nameof(TrackPixelHeight));
        }
        #endregion
    }
}
