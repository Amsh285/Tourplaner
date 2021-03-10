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
        public event RoutedEventHandler ViewLoaded;

        public ShellViewDataTemplateSelector()
        {
            this.viewModelBinder = new ViewModelBinder();

            ViewLoaded += ShellViewDataTemplateSelector_ViewLoaded;
        }

        private void ShellViewDataTemplateSelector_ViewLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement view && view.DataContext is PropertyChangedBase viewModel)
            {
                viewModelBinder.Bind(viewModel, view);

                if (viewModel is IHasView hasView)
                    hasView.Loaded(sender, e);
            }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item is TabItemViewModel tabItemViewModel)
            {
                Assert.NotNull(tabItemViewModel.ViewModel, nameof(tabItemViewModel.ViewModel));
                
                Type viewModelType = tabItemViewModel.ViewModel
                    .GetType();

                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                Type associatedViewType = executingAssembly.GetTypes()
                    .FirstOrDefault(t => t.IsSubclassOf(typeof(UserControl)) && CompareNamingConventions(viewModelType, t));

                DataTemplate template = new DataTemplate(viewModelType);
                FrameworkElementFactory factory = new FrameworkElementFactory(associatedViewType);
                factory.SetValue(FrameworkElement.DataContextProperty, tabItemViewModel.ViewModel);
                factory.AddHandler(FrameworkElement.LoadedEvent, ViewLoaded);

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

        private readonly ViewModelBinder viewModelBinder;
    }
}
