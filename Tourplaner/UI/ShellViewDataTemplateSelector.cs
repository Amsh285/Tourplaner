using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Tourplaner.Infrastructure;

namespace Tourplaner.UI
{
    public class ShellViewDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item is IScreen screen)
            {
                Type viewModelType = screen.GetType();

                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                Type associatedViewType = executingAssembly.GetTypes()
                    .FirstOrDefault(t => t.IsSubclassOf(typeof(UserControl)) && CompareNamingConventions(viewModelType, t));

                if (associatedViewType == null)
                    throw new ViewNotFoundException($"ViewType: {viewModelType.Name.Replace("ViewModel", "View")} could not be found for ViewModel:{viewModelType}");

                DataTemplate template = new DataTemplate(viewModelType);
                FrameworkElementFactory factory = new FrameworkElementFactory(associatedViewType);
                factory.SetValue(FrameworkElement.DataContextProperty, screen);

                template.VisualTree = factory;

                return template;
            }

            return base.SelectTemplate(item, container);
        }

        private static bool CompareNamingConventions(Type viewModelType, Type viewType)
        {
            string viewModelname = viewModelType
                .Name
                .Replace("ViewModel", string.Empty, StringComparison.Ordinal);

            string viewName = viewType
                .Name
                .Replace("View", string.Empty, StringComparison.Ordinal);

            return viewName.Equals(viewModelname, StringComparison.Ordinal);
        }
    }
}
