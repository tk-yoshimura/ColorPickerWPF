using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    /// <summary>
    /// Interaction logic for NumericBox.xaml
    /// </summary>
    public partial class NumericBox : UserControl, INotifyPropertyChanged {
        public NumericBox() {
            InitializeComponent();

            DataObject.AddPastingHandler(textBox, OnPaste);
        }

        public event EventHandler<EventArgs> ValueChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region Value
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(int),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged
                )
            );

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is NumericBox ctrl) {
                ctrl.Value = (int)e.NewValue;
            }
        }

        public int Value {
            get => (int)GetValue(ValueProperty);
            set {
                SetValue(ValueProperty, int.Clamp(value, MinValue, MaxValue));

                UpdateText();

                ValueChanged?.Invoke(this, EventArgs.Empty);

                OnPropertyChanged(nameof(IsMinimum));
                OnPropertyChanged(nameof(IsMaximum));
            }
        }
        #endregion

        #region MinValue
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(
                nameof(MinValue),
                typeof(int),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnMinValueChanged
                )
            );

        private static void OnMinValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is NumericBox ctrl) {
                ctrl.MinValue = (int)e.NewValue;
            }
        }

        public int MinValue {
            get => (int)GetValue(MinValueProperty);
            set {
                SetValue(MinValueProperty, value);

                OnPropertyChanged(nameof(IsMinimum));
            }
        }
        #endregion

        #region MaxValue
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(
                nameof(MaxValue),
                typeof(int),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(
                    10000,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnMaxValueChanged
                )
            );

        private static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is NumericBox ctrl) {
                ctrl.MaxValue = (int)e.NewValue;
            }
        }

        public int MaxValue {
            get => (int)GetValue(MaxValueProperty);
            set {
                SetValue(MaxValueProperty, value);

                OnPropertyChanged(nameof(IsMaximum));
            }
        }
        #endregion

        protected void UpdateText() {
            int index = textBox.CaretIndex;

            textBox.TextChanged -= TextBox_TextChanged;
            textBox.Text = $"{Value}";
            textBox.CaretIndex = index;
            textBox.TextChanged += TextBox_TextChanged;
        }

        public bool IsMinimum => Value <= MinValue;
        public bool IsMaximum => Value >= MaxValue;

        private static Regex NonNumericRegex { get; } = new Regex("[^0-9]+");
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            string text = ((TextBox)sender).Text + e.Text;
            e.Handled = NonNumericRegex.IsMatch(text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (int.TryParse(textBox.Text, out int value)) {
                Value = value;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Up) {
                ChangeValue(1);
            }
            else if (e.Key == Key.Down) {
                ChangeValue(-1);
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            textBox.SelectAll();
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (sender is not TextBox textbox) {
                return;
            }

            if (textbox.IsFocused) {
                return;
            }

            textbox.Focus();
            e.Handled = true;
        }

        private void GridUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            ChangeValue(1);

            StartupTimer();
        }

        private void GridDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            ChangeValue(-1);

            StartupTimer();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            StopTimer();
        }

        private void NumericBox_MouseWheel(object sender, MouseWheelEventArgs e) {
            ChangeValue(int.Sign(e.Delta));
        }

        protected void ChangeValue(int diff) {
            Value += diff;
        }

        private DispatcherTimer timer = null;
        private int mouse_press_duration = 0;
        protected void StartupTimer() {
            if (timer is not null) {
                return;
            }

            lock (this) {
                timer = new DispatcherTimer() {
                    Interval = TimeSpan.FromSeconds(0.04)
                };
                timer.Tick += Timer_Tick;
                timer.Start();

                Unloaded += (s, e) => StopTimer();
            }
        }

        protected void StopTimer() {
            if (timer is null) {
                return;
            }

            lock (this) {
                timer.Stop();
                timer = null;
                mouse_press_duration = 0;
            }
        }

        private void Timer_Tick(object sender, EventArgs e) {
            const int startup_times = 5;

            if ((!GridUp.IsMouseOver && !GridDown.IsMouseOver) || Mouse.LeftButton != MouseButtonState.Pressed) {
                StopTimer();
                return;
            }

            if (mouse_press_duration >= startup_times) {
                int diff = (int)double.Exp10(int.Clamp((mouse_press_duration - startup_times) / 9, 0, 16));

                if (GridUp.IsMouseOver) {
                    ChangeValue(diff);
                }
                else {
                    ChangeValue(-diff);
                }
            }

            mouse_press_duration++;
        }

        private void NumericBox_LostFocus(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(textBox.Text)) {
                textBox.Text = $"{MinValue}";
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e) {
            bool is_text = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!is_text) {
                e.CancelCommand();
                return;
            }

            string text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            if (string.IsNullOrWhiteSpace(text) || NonNumericRegex.IsMatch(text)) {
                e.CancelCommand();
                return;
            }
        }
    }
}
