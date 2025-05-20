using ColorPicker.ColorSpace;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for RGBHexadecimalBox.xaml
    /// </summary>
    public partial class RGBHexadecimalBox : UserControl, INotifyPropertyChanged {
        private readonly DispatcherTimer timer_display;

        public RGBHexadecimalBox() {
            InitializeComponent();

            UpdateColor((SelectedColor, SelectedAlpha));

            DataObject.AddPastingHandler(textBox, OnPaste);

            timer_display = new DispatcherTimer() {
                Interval = TimeSpan.FromSeconds(0.01)
            };
            timer_display.Tick += TimerDisplay_Tick;
            Unloaded += (s, e) => StopTimerDisplay();

            OnPropertyChanged(nameof(IsColorREF));
        }

        public event EventHandler<EventArgs> ValueChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(RGB),
                typeof(RGBHexadecimalBox),
                new FrameworkPropertyMetadata(
                    new RGB(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is RGBHexadecimalBox ctrl) {
                ctrl.SetSelectedColor((RGB)e.NewValue, internal_only: true);
            }
        }

        public RGB SelectedColor {
            get => (RGB)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value);
            }
        }

        private RGB current_color = new();
        private void SetSelectedColor(RGB color, bool internal_only = false) {
            if (current_color != color) {
                current_color = color;

                if (textBox.IsFocused) {
                    UpdateColor((color, SelectedAlpha));
                }
                else {
                    StopTimerDisplay();
                    StartupTimerDisplay();
                }

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region SelectedAlpha
        protected static readonly DependencyProperty SelectedAlphaProperty =
            DependencyProperty.Register(
                nameof(SelectedAlpha),
                typeof(double),
                typeof(RGBHexadecimalBox),
                new FrameworkPropertyMetadata(
                    0d,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnAlphaChanged
                )
            );

        private static void OnAlphaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is RGBHexadecimalBox ctrl) {
                ctrl.SetSelectedAlpha((double)e.NewValue, internal_only: true);
            }
        }

        public double SelectedAlpha {
            get => (double)GetValue(SelectedAlphaProperty);
            set {
                SetSelectedAlpha(value);
            }
        }

        private double current_alpha = new();
        private void SetSelectedAlpha(double value, bool internal_only = false) {
            if (current_alpha != value) {
                current_alpha = value;

                if (textBox.IsFocused) {
                    UpdateColor((SelectedColor, value));
                }
                else {
                    StopTimerDisplay();
                    StartupTimerDisplay();
                }

                if (!internal_only) {
                    SetValue(SelectedAlphaProperty, value);
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region EncodingMode
        private HexadecimalBoxEncodingMode encoding_mode = HexadecimalBoxEncodingMode.RGB;
        public HexadecimalBoxEncodingMode EncodingMode {
            get => encoding_mode;
            set {
                encoding_mode = value;

                OnPropertyChanged(nameof(EncodingMode));
                OnPropertyChanged(nameof(IsColorREF));
            }
        }
        #endregion

        protected void UpdateColor(RGBA rgba) {
            Color color = (Color)rgba;

            string str = string.Empty;

            switch (EncodingMode) {
                case HexadecimalBoxEncodingMode.RGB:
                    str = $"{color.R:X2}{color.G:X2}{color.B:X2}";
                    break;
                case HexadecimalBoxEncodingMode.RGBA:
                    str = $"{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}";
                    break;
                case HexadecimalBoxEncodingMode.ARGB:
                    str = $"{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
                    break;
            }

            UpdateText(str);

            OnPropertyChanged(nameof(IsColorREF));
        }

        private void UpdateText(string str) {
            int index = textBox.CaretIndex;
            textBox.TextChanged -= TextBox_TextChanged;
            textBox.Text = str;
            textBox.TextChanged += TextBox_TextChanged;
            textBox.CaretIndex = index;
        }

        private static Regex NonHexadecimalRegex { get; } = new Regex("[^0-9A-Fa-f]+");
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            string text = ((TextBox)sender).Text + e.Text;
            e.Handled = NonHexadecimalRegex.IsMatch(text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            string text = ((TextBox)sender).Text;

            if (TryParseColor(text, out RGB color, out double alpha)) {
                SelectedColor = color;

                if (EncodingMode != HexadecimalBoxEncodingMode.RGB) {
                    SelectedAlpha = alpha;
                }
            }
            else {
                UpdateText(textBox.Text.ToUpper());
            }

            OnPropertyChanged(nameof(IsColorREF));
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
            if (!IsColorREF) {
                UpdateColor((SelectedColor, SelectedAlpha));
                OnPropertyChanged(nameof(IsColorREF));
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e) {
            textBox.SelectAll();
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (sender is not TextBox textbox) {
                return;
            }

            if (textbox.IsFocused) {
                return;
            }

            textbox.Focus();
            e.Handled = true;
        }

        protected void StartupTimerDisplay() {
            if (timer_display.IsEnabled) {
                return;
            }

            timer_display.Start();
        }

        protected void StopTimerDisplay() {
            if (!timer_display.IsEnabled) {
                return;
            }

            timer_display.Stop();
        }

        private void TimerDisplay_Tick(object sender, EventArgs e) {
            UpdateColor((current_color, current_alpha));
            StopTimerDisplay();
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e) {
            bool is_text = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!is_text) {
                e.CancelCommand();
                return;
            }

            string text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            if (string.IsNullOrWhiteSpace(text) || NonHexadecimalRegex.IsMatch(text)) {
                e.CancelCommand();
                return;
            }
        }

        protected bool TryParseColor(string text, out RGB color, out double alpha) {
            text = text.Trim();

            if (NonHexadecimalRegex.IsMatch(text) || text.Length != ExpectedLength) {
                color = new();
                alpha = 0;
                return false;
            }

            int hex = Convert.ToInt32(text, 16);

            if (EncodingMode == HexadecimalBoxEncodingMode.RGB) {
                int r = (hex >> 16) & 0xFF, g = (hex >> 8) & 0xFF, b = hex & 0xFF;

                color = new RGB(r / 255d, g / 255d, b / 255d);
                alpha = 1;

                return true;
            }
            else {
                int r, g, b, a;

                if (EncodingMode == HexadecimalBoxEncodingMode.RGBA) {
                    r = (hex >> 24) & 0xFF;
                    g = (hex >> 16) & 0xFF;
                    b = (hex >> 8) & 0xFF;
                    a = hex & 0xFF;
                }
                else {
                    a = (hex >> 24) & 0xFF;
                    r = (hex >> 16) & 0xFF;
                    g = (hex >> 8) & 0xFF;
                    b = hex & 0xFF;
                }

                color = new RGB(r / 255d, g / 255d, b / 255d);
                alpha = a / 255d;

                return true;
            }
        }

        public bool IsColorREF =>
            textBox is not null && textBox.Text.Length == ExpectedLength;

        private int ExpectedLength => EncodingMode == HexadecimalBoxEncodingMode.RGB ? 6 : 8;
    }
}
