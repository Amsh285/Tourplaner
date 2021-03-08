using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Tourplaner.Infrastructure
{
    public class ViewModelBinder
    {
        public void Bind(IDictionary<PropertyChangedBase, FrameworkElement> viewModelMap)
        {
            if (viewModelMap is null)
                throw new ArgumentNullException(nameof(viewModelMap));

            foreach (KeyValuePair<PropertyChangedBase, FrameworkElement> viewModelWithView in viewModelMap)
            {
                PropertyChangedBase viewModel = viewModelWithView.Key;
                FrameworkElement view = viewModelWithView.Value;

                Bind(viewModel, view);
            }
        }

        public void Bind(PropertyChangedBase viewModel, FrameworkElement view)
        {
            Debug.WriteLine($"Binding {viewModel} with {view}");

            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            if (view is null)
                throw new ArgumentNullException(nameof(view));

            if (view is ContentControl contentControl && contentControl.Content is FrameworkElement content)
            {
                Bind(viewModel, content);
            }
            else
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(view); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(view, i);

                    if (child is FrameworkElement control)
                    {
                        Bind(viewModel, control);

                        if ((control is TextBox || control is TextBlock) && !string.IsNullOrWhiteSpace(control.Name))
                        {
                            Binding binding = new Binding(control.Name);
                            binding.Source = viewModel;

                            if (control is TextBox)
                                control.SetBinding(TextBox.TextProperty, binding);
                            else if (control is TextBlock)
                                control.SetBinding(TextBlock.TextProperty, binding);
                        }
                    }
                }
            }
        }
    }
}
