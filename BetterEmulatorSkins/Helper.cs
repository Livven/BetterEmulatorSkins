using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace BetterEmulatorSkins
{
    public static class Helper
    {
        public static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
        public static T FindElementInVisualTree<T>(DependencyObject parent) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if (count == 0)
            {
                return null;
            }
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var childChild = FindElementInVisualTree<T>(child);
                    if (childChild != null)
                    {
                        return childChild;
                    }
                }
            }
            return null;
        }
    }
}
