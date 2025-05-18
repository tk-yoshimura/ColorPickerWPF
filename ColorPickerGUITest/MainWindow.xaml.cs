using ColorPicker;
using System.Diagnostics;
using System.Windows;

namespace ColorPickerGUITest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void HSVColorPicker_HSVColorChanged(object sender, HSVColorChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.HSV}";
            }
        }

        private void YCbCrColorPicker_YCbCrColorChanged(object sender, YCbCrColorChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.YCbCr}";
            }
        }

        private void GraphicalSlider_SliderValueChanged(object sender, SliderValueChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.Value}";
            }
        }

        private void RedSlider_RGBColorChanged(object sender, RGBColorChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.RGB}";
            }
        }

        private void GreenSlider_RGBColorChanged(object sender, RGBColorChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.RGB}";
            }
        }

        private void BlueSlider_RGBColorChanged(object sender, RGBColorChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.RGB}";
            }
        }

        private void GrayscaleSlider_SliderValueChanged(object sender, SliderValueChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"grayscale={e.Value:0.000}";
            }
        }

        private void HueSlider_HSVColorChanged(object sender, HSVColorChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.HSV}";
            }
        }

        private void SaturationSlider_HSVColorChanged(object sender, HSVColorChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.HSV}";
            }
        }

        private void BrightnessSlider_HSVColorChanged(object sender, HSVColorChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"{e.HSV}";
            }
        }

        private void AlphaSlider_SliderValueChanged(object sender, SliderValueChangedEventArgs e) {
            if (e.UserOperation) {
                Debug.WriteLine("\n-------------");

                StatusBar.Text = $"alpha={e.Value:0.000}";
            }
        }
    }
}