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
        private readonly DispatcherTimer timer_buttons, timer_display;

        public NumericBox() {
            InitializeComponent();

            DataObject.AddPastingHandler(textBox, OnPaste);

            timer_buttons = new DispatcherTimer() {
                Interval = TimeSpan.FromSeconds(0.04)
            };
            timer_buttons.Tick += TimerButtons_Tick;
            Unloaded += (s, e) => StopTimerButtons();

            timer_display = new DispatcherTimer() {
                Interval = TimeSpan.FromSeconds(0.01)
            };
            timer_display.Tick += TimerDisplay_Tick;
            Unloaded += (s, e) => StopTimerDisplay();
        }

        public event EventHandler<EventArgs> ValueChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region Value
        protected static readonly DependencyProperty ValueProperty =
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
                ctrl.SetValue((int)e.NewValue, internal_only: true);
            }
        }

        private int current_value = 0;
        public int Value {
            get => (int)GetValue(ValueProperty);
            set {
                SetValue(value);
            }
        }

        protected int SetValue(int value, bool internal_only = false) {
            int new_value = int.Clamp(value, MinValue, MaxValue);

            if (current_value != new_value || value != new_value) {
                bool is_minimum = IsMinimum, is_maximum = IsMaximum;

                current_value = new_value;

                if (textBox.IsFocused) {
                    UpdateValue(new_value);
                }
                else {
                    StopTimerDisplay();
                    StartupTimerDisplay();
                }

                if (!internal_only) {
                    SetValue(ValueProperty, new_value);
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }

                if (is_minimum != IsMinimum) {
                    OnPropertyChanged(nameof(IsMinimum));
                }
                if (is_maximum != IsMaximum) {
                    OnPropertyChanged(nameof(IsMaximum));
                }
            }

            return value;
        }
        #endregion

        #region MinValue
        int min_value = 0;
        public int MinValue {
            get => min_value;
            set {
                if (value < 0) {
                    throw new ArgumentException("Must be non-negative.", nameof(MinValue));
                }

                min_value = value;

                OnPropertyChanged(nameof(MinValue));
                OnPropertyChanged(nameof(IsMinimum));
            }
        }
        #endregion

        #region MaxValue
        int max_value = 10000;
        public int MaxValue {
            get => max_value;
            set {
                if (value < 0) {
                    throw new ArgumentException("Must be non-negative.", nameof(MaxValue));
                }

                max_value = value;

                OnPropertyChanged(nameof(MaxValue));
                OnPropertyChanged(nameof(IsMaximum));
            }
        }
        #endregion

        private void UpdateValue(int val) {
            int index = textBox.CaretIndex;

            textBox.TextChanged -= TextBox_TextChanged;
            textBox.Text = $"{val}";
            textBox.CaretIndex = index;
            textBox.TextChanged += TextBox_TextChanged;
        }

        public bool IsMinimum => current_value <= MinValue;
        public bool IsMaximum => current_value >= MaxValue;

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
            ButtonsFocusIn();

            ChangeValue(1);

            StartupTimerButtons();
        }

        private void GridDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            ButtonsFocusIn();

            ChangeValue(-1);

            StartupTimerButtons();
        }

        private void ButtonsFocusIn() {
            if (!textBox.IsFocused) {
                textBox.Focus();
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            StopTimerButtons();
        }

        private void NumericBox_MouseWheel(object sender, MouseWheelEventArgs e) {
            ChangeValue(int.Sign(e.Delta));
        }

        protected void ChangeValue(int diff) {
            Value = current_value + diff;
        }

        private int mouse_press_duration = 0;
        protected void StartupTimerButtons() {
            if (timer_buttons.IsEnabled) {
                return;
            }

            timer_buttons.Start();
        }

        protected void StopTimerButtons() {
            if (!timer_buttons.IsEnabled) {
                return;
            }

            timer_buttons.Stop();
            mouse_press_duration = 0;
        }

        private void TimerButtons_Tick(object sender, EventArgs e) {
            const int startup_times = 5;

            if ((!GridUp.IsMouseOver && !GridDown.IsMouseOver) || Mouse.LeftButton != MouseButtonState.Pressed) {
                StopTimerButtons();
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
            UpdateValue(current_value);
            StopTimerDisplay();
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
