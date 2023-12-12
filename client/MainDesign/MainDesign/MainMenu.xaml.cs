﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MainDesign
{
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            LoadImages();
        }

        private void LoadImages()
        {
            try
            {
                var button1 = FindName("Button1") as Button;
                var button2 = FindName("Button2") as Button;

                if (button1 != null && button2 != null)
                {
                    // Load the first image and set it as the background for Button1
                    var bitmapImage1 = new BitmapImage(new Uri("pack://application:,,,/MainDesign;component/Images/Image1.png"));
                    var imageBrush1 = new System.Windows.Media.ImageBrush(bitmapImage1);
                    button1.Background = imageBrush1;

                    // Load the second image and set it as the background for Button2
                    var bitmapImage2 = new BitmapImage(new Uri("pack://application:,,,/MainDesign;component/Images/Image2.png"));
                    var imageBrush2 = new System.Windows.Media.ImageBrush(bitmapImage2);
                    button2.Background = imageBrush2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
            }
        }
    }
}

