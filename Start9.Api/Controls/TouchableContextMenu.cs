﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Start9.Api.Controls
{
    public class TouchableContextMenu : ContextMenu
    {
        public Boolean OpenedWithTouch
        {
            get => (Boolean)GetValue(OpenedWithTouchProperty);
            set => SetValue(OpenedWithTouchProperty, value);
        }

        public static readonly DependencyProperty OpenedWithTouchProperty = DependencyProperty.Register("OpenedWithTouch",
            typeof(Boolean), typeof(TouchableContextMenu),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public TouchableContextMenu()
        {
            Loaded += TouchableContextMenu_Loaded;
        }

        private void TouchableContextMenu_Loaded(Object sender, RoutedEventArgs e)
        {
            var source = ContextMenuService.GetPlacementTarget(this);
            var placeTarget = PlacementTarget as UIElement;
            if (source != null)
            {
                (source as UIElement).TouchDown += Source_TouchDown;
                (source as UIElement).MouseDown += Source_MouseDown;
            }
            else if (placeTarget != null)
            {
                placeTarget.TouchDown += Source_TouchDown;
                placeTarget.MouseDown += Source_MouseDown;
            }
        }

        private void Source_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            OpenedWithTouch = false;
        }

        private void Source_TouchDown(Object sender, TouchEventArgs e)
        {
            //TouchStarted = DateTime.Now;
            Timer touchTimer = new Timer(1);
            touchTimer.Elapsed += delegate
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (IsOpen)
                    {
                        OpenedWithTouch = true;
                    }
                    else if (!((e.OriginalSource as UIElement).AreAnyTouchesOver))
                    {
                        touchTimer.Stop();
                    }
                }));
            };
            touchTimer.Start();
        }
    }
}
