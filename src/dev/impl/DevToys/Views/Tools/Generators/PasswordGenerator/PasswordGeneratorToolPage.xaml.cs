#nullable enable

using DevToys.Api.Core.Navigation;
using DevToys.Shared.Core;
using DevToys.ViewModels.Tools.PasswordGenerator;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DevToys.Views.Tools.PasswordGenerator
{
    public sealed partial class PasswordGeneratorToolPage : Page
    {
        public static readonly DependencyProperty ViewModelProperty
            = DependencyProperty.Register(
                nameof(ViewModel),
                typeof(PasswordGeneratorToolViewModel),
                typeof(PasswordGeneratorToolPage),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the page's view model.
        /// </summary>
        public PasswordGeneratorToolViewModel ViewModel
        {
            get => (PasswordGeneratorToolViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public PasswordGeneratorToolPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModel is null)
            {
                var parameters = (NavigationParameter)e.Parameter;

                // Set the view model
                Assumes.NotNull(parameters.ViewModel, nameof(parameters.ViewModel));
                ViewModel = (PasswordGeneratorToolViewModel)parameters.ViewModel!;
                DataContext = ViewModel;

                ViewModel.OutputTextBox = OutputTextBox;
            }

            base.OnNavigatedTo(e);
        }
    }
}
