using System.Windows;
using System.Windows.Media;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class NumericBox {
        #region TextBoxBorderBrush
        public static readonly DependencyProperty TextBoxBorderBrushProperty =
            DependencyProperty.Register(
                nameof(TextBoxBorderBrush),
                typeof(Brush),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Colors.Gray),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public Brush TextBoxBorderBrush {
            get => (Brush)GetValue(TextBoxBorderBrushProperty);
            set {
                SetValue(TextBoxBorderBrushProperty, value);
            }
        }
        #endregion

        #region SelectionBrush
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(
                nameof(SelectionBrush),
                typeof(Brush),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Color.FromRgb(0, 120, 215)),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public Brush SelectionBrush {
            get => (Brush)GetValue(SelectionBrushProperty);
            set {
                SetValue(SelectionBrushProperty, value);
            }
        }
        #endregion

        #region ButtonBorderBrush
        public static readonly DependencyProperty ButtonBorderBrushProperty =
            DependencyProperty.Register(
                nameof(ButtonBorderBrush),
                typeof(Brush),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Colors.LightGray),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public Brush ButtonBorderBrush {
            get => (Brush)GetValue(ButtonBorderBrushProperty);
            set {
                SetValue(ButtonBorderBrushProperty, value);
            }
        }
        #endregion

        #region TriangleFillBrush
        public static readonly DependencyProperty TriangleFillBrushProperty =
            DependencyProperty.Register(
                nameof(TriangleFillBrush),
                typeof(Brush),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Colors.Gray),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public Brush TriangleFillBrush {
            get => (Brush)GetValue(TriangleFillBrushProperty);
            set {
                SetValue(TriangleFillBrushProperty, value);
            }
        }
        #endregion

        #region ButtonMouseOverBrush
        public static readonly DependencyProperty ButtonMouseOverBrushProperty =
            DependencyProperty.Register(
                nameof(ButtonMouseOverBrush),
                typeof(Brush),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Colors.LightGray),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public Brush ButtonMouseOverBrush {
            get => (Brush)GetValue(ButtonMouseOverBrushProperty);
            set {
                SetValue(ButtonMouseOverBrushProperty, value);
            }
        }
        #endregion

        #region ButtonMouseLeaveBrush
        public static readonly DependencyProperty ButtonMouseLeaveBrushProperty =
            DependencyProperty.Register(
                nameof(ButtonMouseLeaveBrush),
                typeof(Brush),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Colors.Transparent),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public Brush ButtonMouseLeaveBrush {
            get => (Brush)GetValue(ButtonMouseLeaveBrushProperty);
            set {
                SetValue(ButtonMouseLeaveBrushProperty, value);
            }
        }
        #endregion

        #region TriangleMargin
        public static readonly DependencyProperty TriangleMarginProperty =
            DependencyProperty.Register(
                nameof(TriangleMargin),
                typeof(Thickness),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    new Thickness(4, 2, 4, 2),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );

        public Thickness TriangleMargin {
            get => (Thickness)GetValue(TriangleMarginProperty);
            set {
                SetValue(TriangleMarginProperty, value);
            }
        }
        #endregion
    }
}
