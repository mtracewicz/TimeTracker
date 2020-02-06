using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI
{
    class ControlsFactory
    {

        public static DockPanel CreateDockpanel(String labelContent,RoutedEventHandler onClick)
        {
            DockPanel dockPanel = new DockPanel
            {
                LastChildFill = true,
                Margin = new Thickness(2.5)
            };
            Button button = new Button
            {
                Content = "X",
                MinWidth = 25,
                Background = Utils.GetBrushFromHex("#fff03e47")
            };
            button.Click += onClick;
            Label label = new Label
            {
                Content = labelContent,
                Background = Utils.GetBrushFromHex("#ff555b6e")
            };
            dockPanel.Children.Add(button);
            dockPanel.Children.Add(label);
            return dockPanel;
        }

    }
}
