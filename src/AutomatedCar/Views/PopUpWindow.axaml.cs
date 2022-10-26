namespace AutomatedCar.Views
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    public partial class PopUpWindow : Window
    {
        public PopUpWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
