using ColorPicker.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for Palette.xaml
    /// </summary>
    public partial class Palette : UserControl {

        public int Columns { get; }
        public int Rows { get; }

        private int select_index = -1;
        public int SelectIndex {
            get => select_index;
            private set {
                if (value != select_index) {
                    select_index = value;
                    RenderCursor(select_index, ActualWidth, ActualHeight);
                }
            }
        }

        private readonly IList<Color> colors;

        protected Palette(int cols, int rows, IList<Color> colors) {
            if (cols < 0 || rows < 0 || checked(cols * rows) != colors.Count) {
                throw new ArgumentException("Mismatch palette colors", $"{cols}, {rows}");
            }

            InitializeComponent();

            Columns = cols;
            Rows = rows;
            this.colors = colors;

            Loaded += (s, e) => {
                GeneratePalette(cols, rows, colors);
            };
        }

        #region EventHandler
        public event EventHandler<PaletteClickedEventArgs> PaletteClicked;
        #endregion

        #region GeneratePalette
        protected void GeneratePalette(int cols, int rows, IList<Color> colors) {
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

                    Rectangle rect = new() {
                        Fill = new SolidColorBrush(color),
                        UseLayoutRounding = true,
                        SnapsToDevicePixels = true
                    };

                    Grid.SetColumn(rect, x);
                    Grid.SetRow(rect, y);

                    GridPalette.Children.Add(rect);
                }
            }
        }
        #endregion

        #region DockPanel events
        private void DockPanel_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            Point pt = e.GetPosition(GridPalette);

            int x = int.Clamp((int)double.Floor(pt.X * Columns / ActualWidth), 0, Columns - 1);
            int y = int.Clamp((int)double.Floor(pt.Y * Rows / ActualHeight), 0, Rows - 1);

            SelectIndex = x + y * Columns;
        }

        private void DockPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            SelectIndex = -1;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (SelectIndex >= 0 && SelectIndex < colors.Count) {
                PaletteClicked?.Invoke(this, new PaletteClickedEventArgs(colors[SelectIndex]));
            }
        }
        #endregion

        #region Render
        private void RenderCursor(int select_index, double width, double height) {
            try {
                checked {
                    _ = (int)width * (int)height * 4;
                }
            }
            catch (OverflowException) {
                return;
            }

            if (select_index < 0) {
                ImageCursor.Source = null;
            }

            DrawingVisual visual = new();

            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            RenderTargetBitmap bitmap = new((int)width, (int)height, dpi_x, dpi_y, PixelFormats.Pbgra32);

            Pen pen_white = new(new SolidColorBrush(Colors.White), 1);
            Pen pen_black = new(new SolidColorBrush(Colors.Black), 1);

            int x = select_index % Columns, y = select_index / Columns;

            Rect rect1 = new(x * width / Columns, y * height / Rows, width / Columns, height / Rows);
            Rect rect2 = new(rect1.X + 1, rect1.Y + 1, rect1.Width - 2, rect1.Height - 2);

            using (DrawingContext context = visual.RenderOpen()) {
                context.PushTransform(ColorPickerUtil.GetVisualScalingTransform(this));

                context.DrawRectangle(null, pen_black, rect1);
                context.DrawRectangle(null, pen_white, rect2);
            }

            bitmap.Render(visual);
            bitmap.Freeze();

            ImageCursor.Source = bitmap;
        }
        #endregion
    }
}
