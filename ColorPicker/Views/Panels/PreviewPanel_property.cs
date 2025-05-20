// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class PreviewPanel {

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

        protected bool IsValidSize => PanelWidth >= 1 && PanelHeight >= 1;
    }
}
