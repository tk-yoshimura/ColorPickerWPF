﻿using ColorPicker;
using ColorPicker.ColorSpace;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ColorPickerGUITest {
    public class MainViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private double selected_value;
        public double SelectedValue {
            get => selected_value;
            set {
                selected_value = value;

                OnPropertyChanged(nameof(SelectedValue));
            }
        }

        private RGB selected_rgb;
        public RGB SelectedRGB {
            get => selected_rgb;
            set {
                Debug.WriteLine($"{nameof(MainViewModel)} - {nameof(SelectedRGB)}");
                Debug.WriteLine($"{selected_rgb} -> {value}");

                selected_rgb = value;
                selected_hsv = value;
                selected_ycbcr = value;

                OnPropertyChanged(nameof(SelectedRGB));
                OnPropertyChanged(nameof(SelectedHSV));
                OnPropertyChanged(nameof(SelectedYCbCr));
            }
        }

        private HSV selected_hsv;
        public HSV SelectedHSV {
            get => selected_hsv;
            set {
                Debug.WriteLine($"{nameof(MainViewModel)} - {nameof(SelectedHSV)}");
                Debug.WriteLine($"{selected_hsv} -> {value}");

                selected_rgb = value;
                selected_hsv = value;
                selected_ycbcr = (RGB)value;

                OnPropertyChanged(nameof(SelectedHSV));
                OnPropertyChanged(nameof(SelectedRGB));
                OnPropertyChanged(nameof(SelectedYCbCr));
            }
        }

        private YCbCr selected_ycbcr;
        public YCbCr SelectedYCbCr {
            get => selected_ycbcr;
            set {
                Debug.WriteLine($"{nameof(MainViewModel)} - {nameof(SelectedYCbCr)}");
                Debug.WriteLine($"{selected_ycbcr} -> {value}");

                selected_rgb = value;
                selected_hsv = (RGB)value;
                selected_ycbcr = value;

                OnPropertyChanged(nameof(SelectedYCbCr));
                OnPropertyChanged(nameof(SelectedRGB));
                OnPropertyChanged(nameof(SelectedHSV));
            }
        }

        private double selected_alpha = 1;
        public double SelectedAlpha {
            get => selected_alpha;
            set {
                Debug.WriteLine($"{nameof(MainViewModel)} - {nameof(SelectedAlpha)}");

                selected_alpha = value;

                OnPropertyChanged(nameof(SelectedAlpha));
            }
        }

        private double selected_grayscale;
        public double SelectedGrayscale {
            get => selected_grayscale;
            set {
                selected_grayscale = value;

                OnPropertyChanged(nameof(SelectedGrayscale));
            }
        }

        private NumericBoxResolutionMode rgb_resolution_mode = NumericBoxResolutionMode.Byte;
        public NumericBoxResolutionMode RGBResolutionMode {
            get => rgb_resolution_mode;
            set {
                rgb_resolution_mode = value;

                OnPropertyChanged(nameof(RGBResolutionMode));
            }
        }

        private NumericBoxResolutionMode hsv_resolution_mode = NumericBoxResolutionMode.Percent;
        public NumericBoxResolutionMode HSVResolutionMode {
            get => hsv_resolution_mode;
            set {
                hsv_resolution_mode = value;

                OnPropertyChanged(nameof(HSVResolutionMode));
            }
        }

        private HueConversionMode hue_conversion_mode = HueConversionMode.OstwaldPerceptual;
        public HueConversionMode HueConversionMode {
            get => hue_conversion_mode;
            set {
                hue_conversion_mode = value;

                OnPropertyChanged(nameof(HueConversionMode));
            }
        }
    }
}
