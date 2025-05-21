using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for Palette.xaml
    /// </summary>
    public partial class Palette : UserControl {

        protected Palette(int cols, int rows, IList<Color> colors) {
            InitializeComponent();

            Loaded += (s, e) => {
                GeneratePalette(cols, rows, colors);
            };
        }

        #region EventHandler
        public event EventHandler<PaletteClickedEventArgs> PaletteClicked;
        #endregion

        #region GeneratePalette
        protected void GeneratePalette(int cols, int rows, IList<Color> colors) {
            if (cols < 0 || rows < 0 || checked(cols * rows) != colors.Count) {
                throw new ArgumentException("Mismatch palette colors", $"{cols}, {rows}");
            }

            GridPalette.ColumnDefinitions.Clear();

            for (int i = 0; i < cols; i++) {
                ColumnDefinition col = new() {
                    Width = new GridLength(1, GridUnitType.Star)
                };

                GridPalette.ColumnDefinitions.Add(col);
            }

            GridPalette.RowDefinitions.Clear();
            for (int i = 0; i < rows; i++) {
                RowDefinition row = new() {
                    Height = new GridLength(1, GridUnitType.Star)
                };

                GridPalette.RowDefinitions.Add(row);
            }

            for (int y = 0, i = 0; y < rows; y++) {
                for (int x = 0; x < cols; x++, i++) {
                    Color color = colors[i];

                    DockPanel rect = new() {
                        Background = new SolidColorBrush(color),
                        UseLayoutRounding = true,
                        SnapsToDevicePixels = true
                    };

                    rect.MouseLeftButtonDown += (s, e) => PaletteClicked.Invoke(this, new PaletteClickedEventArgs(color));

                    Grid.SetColumn(rect, x);
                    Grid.SetRow(rect, y);

                    GridPalette.Children.Add(rect);
                }
            }
        }
        #endregion
    }
}
