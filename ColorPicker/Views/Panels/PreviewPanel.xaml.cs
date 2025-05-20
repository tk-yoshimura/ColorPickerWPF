using ColorPicker.ColorSpace;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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

        protected int PanelWidth => checked((int)Utils.ColorPickerUtil.GetPixelSize(this).pixelWidth);
        protected int PanelHeight => checked((int)Utils.ColorPickerUtil.GetPixelSize(this).pixelHeight);

        private int block_size = 5;
        public int BlockSize {
            get => block_size;
            set {
                block_size = value;
                RenderPanel(Color, Alpha);

                OnPropertyChanged(nameof(BlockSize));
            }
        }

        private void Panel_Loaded(object sender, RoutedEventArgs e) {
            RenderPanel(Color, Alpha);
        }

        private void Panel_SizeChanged(object sender, SizeChangedEventArgs e) {
            RenderPanel(Color, Alpha);
        }

        protected bool IsValidSize => PanelWidth >= 1 && PanelHeight >= 1;

        protected void RenderPanel(RGB color, double alpha) {
            if (!IsValidSize) {
                return;
            }

            byte[] buf = new byte[checked(PanelWidth * PanelHeight * 4)];

            RenderPanel(PanelWidth, PanelHeight, buf, color, alpha);

            PixelFormat pixel_format = PixelFormats.Pbgra32;
            int stride = checked(PanelWidth * pixel_format.BitsPerPixel + 7) / 8;
            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            BitmapSource bitmap = BitmapSource.Create(PanelWidth, PanelHeight, dpi_x, dpi_y, pixel_format, null, buf, stride);

            bitmap.Freeze();

            ImagePanel.Source = bitmap;

            OnPropertyChanged(nameof(ImagePanel));

            Debug.WriteLine($"{nameof(PreviewPanel)} - {nameof(RenderPanel)}");
        }

        protected virtual void RenderPanel(int width, int height, byte[] buf, RGB color, double alpha) {
            const double light_color = 0.75, dark_color = 0.25;

            if (height < 1) {
                return;
            }

            int block_size = BlockSize;
            double scale = 1d / (width - 1);

            (double r, double g, double b) = color.Normalize;
            alpha = double.Clamp(alpha, 0, 1);
            double alpha_c = 1 - alpha;

            byte[] buf0 = new byte[4], buf1 = new byte[4], buf2 = new byte[4];

            double r1 = r * alpha + dark_color * alpha_c;
            double g1 = g * alpha + dark_color * alpha_c;
            double b1 = b * alpha + dark_color * alpha_c;

            double r2 = r * alpha + light_color * alpha_c;
            double g2 = g * alpha + light_color * alpha_c;
            double b2 = b * alpha + light_color * alpha_c;

            buf0[0] = (byte)(b * 255 + 0.5);
            buf0[1] = (byte)(g * 255 + 0.5);
            buf0[2] = (byte)(r * 255 + 0.5);
            buf0[3] = 255;

            buf1[0] = (byte)(b1 * 255 + 0.5);
            buf1[1] = (byte)(g1 * 255 + 0.5);
            buf1[2] = (byte)(r1 * 255 + 0.5);
            buf1[3] = 255;

            buf2[0] = (byte)(b2 * 255 + 0.5);
            buf2[1] = (byte)(g2 * 255 + 0.5);
            buf2[2] = (byte)(r2 * 255 + 0.5);
            buf2[3] = 255;

            unsafe {
                fixed (byte* c = buf, c0 = buf0, c1 = buf1, c2 = buf2) {
                    for (int x, y = 0, i = 0; y < height; y++) {
                        for (x = 0; x < width; x++, i += 4) {
                            if (x <= y) {
                                byte* d = (((x / block_size) & 1) ^ ((y / block_size) & 1)) == 0 ? c1 : c2;

                                if (x < y) {
                                    c[i] = d[0];
                                    c[i + 1] = d[1];
                                    c[i + 2] = d[2];
                                    c[i + 3] = d[3];
                                }
                                else {
                                    c[i] = (byte)(d[0] / 2 + c0[0] / 2);
                                    c[i + 1] = (byte)(d[1] / 2 + c0[1] / 2);
                                    c[i + 2] = (byte)(d[2] / 2 + c0[2] / 2);
                                    c[i + 3] = 255;
                                }
                            }
                            else {
                                c[i] = c0[0];
                                c[i + 1] = c0[1];
                                c[i + 2] = c0[2];
                                c[i + 3] = c0[3];
                            }

                            Debug.Assert(i + 3 < buf.Length);
                        }
                    }
                }
            }
        }
    }
}
