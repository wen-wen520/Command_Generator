// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Reflection;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;


namespace Command_Generator.UI.Utils
{
    /// <summary>
    /// Class providing functionality around switching and restoring theme settings
    /// </summary>
    public static class ThemeHelper
    {
        private const string SelectedAppThemeKey = "SelectedAppTheme";

        /// <summary>
        /// Get or set (with LocalSettings persistence) the RequestedTheme of the root element.
        /// </summary>
        public static ElementTheme RootTheme
        {
            get
            {
                if (Window.Current.Content is FrameworkElement rootElement)
                {
                    return rootElement.RequestedTheme;
                }

                Debug.WriteLine("default theme: " + ElementTheme.Default.ToString());
                return ElementTheme.Default;
            }
            set
            {
                if (Window.Current.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = value;

                    ApplicationData.Current.LocalSettings.Values[SelectedAppThemeKey] = rootElement.RequestedTheme.ToString();

                    if (value == ElementTheme.Dark)
                    {
                        ApplicationView.GetForCurrentView().TitleBar.ButtonForegroundColor = Colors.White;
                        //ApplicationView.GetForCurrentView().TitleBar.ButtonHoverBackgroundColor = Colors.DarkGray;
                        //ApplicationView.GetForCurrentView().TitleBar.ButtonPressedBackgroundColor = Colors.LightGray;
                    }
                    else
                    {
                        ApplicationView.GetForCurrentView().TitleBar.ButtonForegroundColor = Colors.Black;
                        //ApplicationView.GetForCurrentView().TitleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                        //ApplicationView.GetForCurrentView().TitleBar.ButtonPressedBackgroundColor = Colors.DarkGray;

                    }
                }
            }
        }

        public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }

        public static void InitializeAppTheme()
        {
            string savedTheme = ApplicationData.Current.LocalSettings.Values[SelectedAppThemeKey]?.ToString();


            if (!string.IsNullOrEmpty(savedTheme))
            {

                RootTheme = GetEnum<ElementTheme>(savedTheme);

            }

        }


        public struct ThemeChangedCallbackToken
        {
            public WeakReference RootFrame;
            public long Token;
        }

        public static ThemeChangedCallbackToken RegisterAppThemeChangedCallback(DependencyPropertyChangedCallback callback)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            long token = rootFrame.RegisterPropertyChangedCallback(FrameworkElement.RequestedThemeProperty, callback);
            return new ThemeChangedCallbackToken { RootFrame = new WeakReference(rootFrame), Token = token };
        }

        public static void UnregisterAppThemeChangedCallback(ThemeChangedCallbackToken callbackToken)
        {
            if (callbackToken.RootFrame.IsAlive)
            {
                Frame rootFrame = callbackToken.RootFrame.Target as Frame;
                rootFrame.UnregisterPropertyChangedCallback(Frame.RequestedThemeProperty, callbackToken.Token);
            }
        }
    }
}

