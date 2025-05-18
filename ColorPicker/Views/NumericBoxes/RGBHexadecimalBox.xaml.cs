using ColorPicker.ColorSpace;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for RGBHexadecimalBox.xaml
    /// </summary>
    public partial class RGBHexadecimalBox : UserControl, INotifyPropertyChanged {
        public RGBHexadecimalBox() {
            InitializeComponent();

            DataObject.AddPastingHandler(textBox, OnPaste);
        }

        public event EventHandler<EventArgs> ValueChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static readonly DependencyProperty SelectedColorProperty =
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
                ctrl.SelectedColor = (RGB)e.NewValue;
            }
        }

        public RGB SelectedColor {
            get => (RGB)GetValue(SelectedColorProperty);
            set {
                SetValue(SelectedColorProperty, value);

                UpdateText();

                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected void UpdateText() {
            Color color = (Color)SelectedColor;

            int index = textBox.CaretIndex;

            textBox.TextChanged -= TextBox_TextChanged;
            textBox.Text = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            textBox.CaretIndex = index;
            textBox.TextChanged += TextBox_TextChanged;
        }

        private static Regex NonHexadecimalRegex { get; } = new Regex("[^0-9A-Fa-f]+");
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            string text = ((TextBox)sender).Text + e.Text;
            e.Handled = NonHexadecimalRegex.IsMatch(text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            string text = ((TextBox)sender).Text;

            if (TryParseColor(text, out RGB color)) {
                SelectedColor = color;
            }
            else {
                int index = textBox.CaretIndex;
                textBox.Text = textBox.Text.ToUpper();
                textBox.CaretIndex = index;
            }

            OnPropertyChanged(nameof(IsColorREF));
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
            string text = ((TextBox)sender).Text;

            if (TryParseColor(text, out RGB color)) {
                SelectedColor = color;
            }
            else {
                UpdateText();
            }

            OnPropertyChanged(nameof(IsColorREF));
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

        protected static bool TryParseColor(string text, out RGB color) {
            text = text.Trim();

            if (NonHexadecimalRegex.IsMatch(text) || text.Length != 6) {
                color = new();
                return false;
            }

            int hex = Convert.ToInt32(text, 16);

            int r = (hex >> 16) & 0xFF, g = (hex >> 8) & 0xFF, b = hex & 0xFF;

            color = new RGB(r / 255d, g / 255d, b / 255d);

            return true;
        }

        public bool IsColorREF => textBox is not null && textBox.Text.Length == 6;
    }
}
