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

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<SliderValueChangedEventArgs> SliderValueChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public static readonly DependencyProperty SelectedColorProperty =
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
                ctrl.Value = (double)e.NewValue;
            }
        }

        public double Value {
            get => (double)GetValue(SelectedColorProperty);
            set {
                SetValue(value, user_operation: false);
            }
        }

        protected virtual void SetValue(double value, bool user_operation) {
            SetValue(SelectedColorProperty, double.Clamp(value, 0, 1));

            RenderPointer();

            SliderValueChanged?.Invoke(this, new SliderValueChangedEventArgs(Value, user_operation));

            OnPropertyChanged(nameof(Value));
        }

        private int track_margin_width = 4;
        public int TrackMarginWidth {
            get => track_margin_width;
            set {
                if (value < 0) {
                    throw new ArgumentException("Must be non-negative.", nameof(TrackMarginWidth));
                }

                track_margin_width = value;
                RenderAllImages();

                OnPropertyChanged(nameof(TrackMarginWidth));
                OnPropertyChanged(nameof(TrackMargin));
            }
        }

        private Size thumb_size = new(8, 8);
        public Size ThumbSize {
            get => thumb_size;
            set {
                thumb_size = value;
                RenderAllImages();

                OnPropertyChanged(nameof(ThumbSize));
            }
        }

        public Thickness TrackMargin => new(TrackMarginWidth, 0, TrackMarginWidth, TrackMarginWidth);

        protected int TrackWidth => checked((int)ActualWidth) - TrackMarginWidth * 2;
        protected int TrackHeight => checked((int)ActualHeight) - TrackMarginWidth;

        private void Slider_Loaded(object sender, RoutedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(TrackWidth));
            OnPropertyChanged(nameof(TrackHeight));
        }

        private void Slider_SizeChanged(object sender, SizeChangedEventArgs e) {
            RenderAllImages();

            OnPropertyChanged(nameof(TrackWidth));
            OnPropertyChanged(nameof(TrackHeight));
        }

        public virtual void RenderAllImages() {
            RenderTrack();
            RenderPointer();
        }

        protected bool IsValidSize => TrackWidth >= 50 && TrackHeight >= 2;
    }
}
