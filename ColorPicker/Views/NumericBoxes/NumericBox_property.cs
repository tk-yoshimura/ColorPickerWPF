using System.Windows;
using System.Windows.Media;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class NumericBox {
        #region TextBoxBorderBrush
        protected static readonly DependencyProperty TextBoxBorderBrushProperty =
            DependencyProperty.Register(
                nameof(TextBoxBorderBrush),
                typeof(Brush),
                typeof(NumericBox),
                new PropertyMetadata(new SolidColorBrush(Colors.Gray))
            );

        public Brush TextBoxBorderBrush {
            get => (Brush)GetValue(TextBoxBorderBrushProperty);
            set {
                SetValue(TextBoxBorderBrushProperty, value);
            }
        }
        #endregion

        #region SelectionBrush
        protected static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(
                nameof(SelectionBrush),
                typeof(Brush),
                typeof(NumericBox),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 120, 215)))
            );

        public Brush SelectionBrush {
            get => (Brush)GetValue(SelectionBrushProperty);
            set {
                SetValue(SelectionBrushProperty, value);
            }
        }
        #endregion

        #region ButtonBorderBrush
        protected static readonly DependencyProperty ButtonBorderBrushProperty =
            DependencyProperty.Register(
                nameof(ButtonBorderBrush),
                typeof(Brush),
                typeof(NumericBox),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray))
            );

        public Brush ButtonBorderBrush {
            get => (Brush)GetValue(ButtonBorderBrushProperty);
            set {
                SetValue(ButtonBorderBrushProperty, value);
            }
        }
        #endregion

        #region ChevronFillBrush
        protected static readonly DependencyProperty ChevronFillBrushProperty =
            DependencyProperty.Register(
                nameof(ChevronFillBrush),
                typeof(Brush),
                typeof(NumericBox),
                new PropertyMetadata(new SolidColorBrush(Colors.Gray))
            );

        public Brush ChevronFillBrush {
            get => (Brush)GetValue(ChevronFillBrushProperty);
            set {
                SetValue(ChevronFillBrushProperty, value);
            }
        }
        #endregion

        #region ButtonMouseOverBrush
        protected static readonly DependencyProperty ButtonMouseOverBrushProperty =
            DependencyProperty.Register(
                nameof(ButtonMouseOverBrush),
                typeof(Brush),
                typeof(NumericBox),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray))
            );

        public Brush ButtonMouseOverBrush {
            get => (Brush)GetValue(ButtonMouseOverBrushProperty);
            set {
                SetValue(ButtonMouseOverBrushProperty, value);
            }
        }
        #endregion

        #region ButtonMouseLeaveBrush
        protected static readonly DependencyProperty ButtonMouseLeaveBrushProperty =
            DependencyProperty.Register(
                nameof(ButtonMouseLeaveBrush),
                typeof(Brush),
                typeof(NumericBox),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent))
            );

        public Brush ButtonMouseLeaveBrush {
            get => (Brush)GetValue(ButtonMouseLeaveBrushProperty);
            set {
                SetValue(ButtonMouseLeaveBrushProperty, value);
            }
        }
        #endregion

        #region ChevronMargin
        protected static readonly DependencyProperty ChevronMarginProperty =
            DependencyProperty.Register(
                nameof(ChevronMargin),
                typeof(Thickness),
                typeof(NumericBox),
                new PropertyMetadata(new Thickness(4, 2, 4, 2))
            );

        public Thickness ChevronMargin {
            get => (Thickness)GetValue(ChevronMarginProperty);
            set {
                SetValue(ChevronMarginProperty, value);
            }
        }
        #endregion
    }
}
