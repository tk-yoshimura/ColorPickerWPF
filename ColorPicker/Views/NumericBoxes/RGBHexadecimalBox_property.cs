using System.Windows;
using System.Windows.Media;

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for RGBHexadecimalBox.xaml
    /// </summary>
    public partial class RGBHexadecimalBox {
        #region BorderBrush
        protected static new readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(
                nameof(BorderBrush),
                typeof(Brush),
                typeof(RGBHexadecimalBox),
                new PropertyMetadata(new SolidColorBrush(Colors.Gray))
            );

        public new Brush BorderBrush {
            get => (Brush)GetValue(BorderBrushProperty);
            set {
                SetValue(BorderBrushProperty, value);
            }
        }
        #endregion

        #region SelectionBrush
        protected static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(
                nameof(SelectionBrush),
                typeof(Brush),
                typeof(RGBHexadecimalBox),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 120, 215)))
            );

        public Brush SelectionBrush {
            get => (Brush)GetValue(SelectionBrushProperty);
            set {
                SetValue(SelectionBrushProperty, value);
            }
        }
        #endregion

        #region InvalidHexBrush
        protected static readonly DependencyProperty InvalidHexBrushProperty =
            DependencyProperty.Register(
                nameof(InvalidHexBrush),
                typeof(Brush),
                typeof(RGBHexadecimalBox),
                new PropertyMetadata(new SolidColorBrush(Colors.Gray))
            );

        public Brush InvalidHexBrush {
            get => (Brush)GetValue(InvalidHexBrushProperty);
            set {
                SetValue(InvalidHexBrushProperty, value);
            }
        }
        #endregion
    }
}
