﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Start9.Api;
using Start9.Api.AppBar;
using Start9.Api.Plex;
using Start9.Api.Tools;
using Start9.Api.DiskItems;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using static Start9.Api.SystemScaling;
using AppInfo = Start9.Api.Appx.AppInfo;
using System.Collections.ObjectModel;
using Start9.Api.Controls;

namespace FrontEndTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DecoratableWindow
    {
        public ObservableCollection<DiskItem> DiskItems
        {
            get => (ObservableCollection<DiskItem>)GetValue(DiskItemsProperty);
            set => SetValue(DiskItemsProperty, value);
        }

        public static readonly DependencyProperty DiskItemsProperty =
            DependencyProperty.Register("DiskItems", typeof(ObservableCollection<DiskItem>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<DiskItem>()));

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        /*protected override void OnSourceInitialized(EventArgs e)
        {
            helper = new WindowInteropHelper(this).EnsureHandle();
            base.Background = new SolidColorBrush(Colors.Transparent);
            Background = new SolidColorBrush(Colors.Transparent);
            var hs = HwndSource.FromHwnd(helper);
            hs.CompositionTarget.BackgroundColor = System.Windows.Media.Colors.Transparent;
            base.OnSourceInitialized(e);
        }*/

        private void MainWindow_Loaded(Object sender, RoutedEventArgs e)
        {
            DiskItem picturesItem = new DiskItem(Environment.ExpandEnvironmentVariables(@"%userprofile%\Pictures"));
            TestTreeView.ItemsSource = picturesItem.SubItems;
        }

        private void zMainWindow_Loaded(Object sender, RoutedEventArgs e)
        {
            DiskItem picturesItem = new DiskItem(Environment.ExpandEnvironmentVariables(@"%userprofile%\Pictures"));
            TestTreeView.ItemsSource = picturesItem.SubItems;
            foreach (DiskItem d in picturesItem.SubItems)
            {
                if (d.ItemType == DiskItem.DiskItemType.File)
                {
                    Debug.WriteLine("FILE TYPE: " + picturesItem.FriendlyItemType);
                    break;
                }
            }

            DiskItem desktopItem = new DiskItem(Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop"));
            Debug.WriteLine("DESKTOP: " + desktopItem.FriendlyItemType);
            //FileIconOverrides.ItemsSource = IconPref.FileIconOverrides;
            DiskItem item = new DiskItem("Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe");
            item.ItemAppInfo.NotificationReceived += (sneder, args) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    /*TestTileStackPanel.Children.Clear();
                    foreach (ImageBrush i in args.NewNotification.Images)
                    {
                        Canvas c = new Canvas()
                        {
                            Width = 100,
                            Height = 100,
                            Background = i
                        };
                        TestTileStackPanel.Children.Add(c);
                    }
                    */
                    foreach (var s in args.NewNotification.Text)
                    {
                        /*TextBlock t = new TextBlock()
                        {
                            Text = s
                        };
                        TestTileStackPanel.Children.Add(t);*/
                        Debug.WriteLine(s);
                    }
                }));
            };
            DiskItems.Add(item);
            /*AppBarWindow appBar = new AppBarWindow()
            {
                DockedWidthOrHeight = 100,
                AllowsTransparency = true,
                Opacity = 0.75,
            };

            Button sideButton = new Button()
            {
                Content = "CHANGE SIDE"
            };

            Button monitorButton = new Button()
            {
                Content = "CHANGE MONITOR"
            };
            monitorButton.Click += (sneder, args) =>
            {
                if (appBar.Monitor == MonitorInfo.AllMonitors[0] && (MonitorInfo.AllMonitors.Count > 1))
                {
                    appBar.Monitor = MonitorInfo.AllMonitors[1];
                }
                else if (MonitorInfo.AllMonitors.Count > 1)
                {
                    appBar.Monitor = MonitorInfo.AllMonitors[0];
                }
            };

            Button closeButton = new Button()
            {
                Content = "CLOSE"
            };
            closeButton.Click += (sneder, args) =>
            {
                appBar.Close();
            };

            appBar.Content = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Children = { sideButton, closeButton }
            };
            sideButton.Click += (sneder, args) =>
            {
                var stack = (appBar.Content as StackPanel);

                if (appBar.DockMode == AppBarDockMode.Bottom)
                {
                    appBar.DockMode = AppBarDockMode.Left;
                    stack.Orientation = Orientation.Vertical;
                }
                else if (appBar.DockMode == AppBarDockMode.Left)
                {
                    appBar.DockMode = AppBarDockMode.Top;
                    stack.Orientation = Orientation.Horizontal;
                }
                else if (appBar.DockMode == AppBarDockMode.Top)
                {
                    appBar.DockMode = AppBarDockMode.Right;
                    stack.Orientation = Orientation.Vertical;
                }
                else
                {
                    appBar.DockMode = AppBarDockMode.Bottom;
                    stack.Orientation = Orientation.Horizontal;
                }
            };
            if (MonitorInfo.AllMonitors.Count > 1)
            {
                (appBar.Content as StackPanel).Children.Add(monitorButton);
            }

            appBar.Show();

            new AppBarWindow()
            {
                Background = new SolidColorBrush(Colors.Blue),
                DockedWidthOrHeight = 125,
                AllowsTransparency = true,
                Opacity = 0.75,
                DockMode = AppBarDockMode.Top
            }.Show();

            new AppBarWindow()
            {
                Background = new SolidColorBrush(Colors.Green),
                DockedWidthOrHeight = 75,
                AllowsTransparency = true,
                Opacity = 0.75,
                DockMode = AppBarDockMode.Right
            }.Show();*/
        }


        /*public MainWindow(bool f)
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            AppxTools.TileInfo Info = new AppxTools.TileInfo("Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe");
            Info.NotificationReceived += (object sneder, AppxTools.NotificationInfoEventArgs args) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    TileTestStackPanel.Children.Clear();
                    foreach (ImageBrush i in args.NewNotification.Images)
                    {
                        Canvas c = new Canvas()
                        {
                            Width = 100,
                            Height = 100,
                            Background = i
                        };
                        TileTestStackPanel.Children.Add(c);
                    }

                    foreach (string s in args.NewNotification.Text)
                    {
                        TextBlock t = new TextBlock()
                        {
                            Text = s
                        };
                        TileTestStackPanel.Children.Add(t);
                    }
                }));
            };
        }

        private void MainWindow_oldLoaded(object sender, RoutedEventArgs e)
        {
            //Resources["TestImageBrush"] = new ImageBrush(new BitmapImage(new Uri(Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\TestImage.png"), UriKind.RelativeOrAbsolute)));

            foreach (DiskItem d in DiskItem.AllApps)
            {
                TreeViewItem t = GetTreeViewItemFromDiskItem(d);
                if (t.Tag.GetType().Name.Contains("DiskFolder"))
                {
                    t.Expanded += (object sneder, RoutedEventArgs args) =>
                    {
                        foreach (DiskItem i in (t.Tag as DiskFolder).SubItems)
                        {
                            t.Items.Add(GetTreeViewItemFromDiskItem(i));
                        }
                    };

                    t.MouseDoubleClick += (object sneder, MouseButtonEventArgs args) =>
                    {
                        Process.Start((t.Tag as DiskFolder).Path);
                    };
                }
                else
                {
                    t.Expanded += (object sneder, RoutedEventArgs args) =>
                    {
                        Process.Start((t.Tag as DiskItem).Path);
                    };
                }
                AllAppsTree.Items.Add(d);
            }
        }*/

        private TreeViewItem GetTreeViewItemFromDiskItem(DiskItem d)
        {
            var p = Path.GetFileNameWithoutExtension(d.ItemPath);
            return new TreeViewItem()
            {
                Tag = d,
                Header = p,
                Style = (Style)Resources[typeof(TreeViewItem)]
            };
        }

        private void ToggleButton_Click(Object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(this, "Yes, I'm working on these for some reason.\n\n...blame Fleccy :P", "This is a Plex MessageBox");
        }

        private void FavButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://start9.menu");
        }
    }/*

    public class ReplacementIconNameToReplacedCanvasConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string source = value.ToString();
            return new Canvas()
            {
                Width = 48,
                Height = 48,
                Background = new ImageBrush(new BitmapImage(new Uri(Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\IconOverrides\" + source), UriKind.RelativeOrAbsolute)))
            };
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }*/

    class ReplacementIconNameToOriginalCanvasConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType,
            Object parameter, CultureInfo culture)
        {
            var source = value.ToString();
            WinApi.ShFileInfo shInfo = new WinApi.ShFileInfo();
            WinApi.SHGetFileInfo(Environment.ExpandEnvironmentVariables(source), 0, ref shInfo, (UInt32)Marshal.SizeOf(shInfo), 0x000000000 | 0x100);
            System.Drawing.Icon entryIcon = System.Drawing.Icon.FromHandle(shInfo.hIcon);
            ImageSource entryIconImageSource = Imaging.CreateBitmapSourceFromHIcon(
            entryIcon.Handle,
            Int32Rect.Empty,
            BitmapSizeOptions.FromWidthAndHeight(System.Convert.ToInt32(RealPixelsToWpfUnits(48)), System.Convert.ToInt32(RealPixelsToWpfUnits(48)))
            );
            return new Canvas()
            {
                Width = 48,
                Height = 48,
                Background = new ImageBrush(entryIconImageSource)  //MiscTools.GetIconFromFilePath(Environment.ExpandEnvironmentVariables(source)))
            };
        }

        public Object ConvertBack(Object value, Type targetType,
            Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}