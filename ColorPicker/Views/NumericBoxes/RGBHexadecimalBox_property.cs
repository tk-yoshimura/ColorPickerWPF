using System.Windows;
using System.Windows.Media;

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for RGBHexadecimalBox.xaml
    /// </summary>
    public partial class RGBHexadecimalBox {
        #region BorderBrush
        public static new readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(
                nameof(BorderBrush),
                typeof(Brush),
                typeof(RGBHexadecimalBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Colors.Gray),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public new Brush BorderBrush {
            get => (Brush)GetValue(BorderBrushProperty);
            set {
                SetValue(BorderBrushProperty, value);

                OnPropertyChanged(nameof(BorderBrush));
            }
        }
        #endregion

        #region SelectionBrush
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(
                nameof(SelectionBrush),
                typeof(Brush),
                typeof(RGBHexadecimalBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Color.FromRgb(0, 120, 215)),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public Brush SelectionBrush {
            get => (Brush)GetValue(SelectionBrushProperty);
            set {
                SetValue(SelectionBrushProperty, value);

                OnPropertyChanged(nameof(SelectionBrush));
            }
        }
        #endregion
    }
}
