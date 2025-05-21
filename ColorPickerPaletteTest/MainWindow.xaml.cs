using System.Windows;
using System.Windows.Media;

namespace ColorPickerPaletteTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Palette_PaletteClicked(object sender, ColorPicker.PaletteClickedEventArgs e) {
            Color color = e.Color;

            StatusBar.Text = $"PaletteClicked: R={color.R} G={color.G} B={color.B}";
        }
    }
}