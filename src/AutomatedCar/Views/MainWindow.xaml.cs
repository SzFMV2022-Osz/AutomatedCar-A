namespace AutomatedCar.Views
{
    using System;
    using System.Reactive.Linq;
    using AutomatedCar.ViewModels;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Markup.Xaml;
    using Avalonia.ReactiveUI;
    using ReactiveUI;

    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            this.InitializeComponent();
            var popUpWindow = new PopUpWindow();
            popUpWindow.Closing += (s, e) =>
            {
                ((Window)s).Hide();
                e.Cancel = true;
                Keyboard.Keys.Clear();
            };
            this.WhenActivated(x => x(ViewModel.WhenAnyValue(x => x.PopUp.ControlledCar.Car.VirtualFunctionBus.CollisionPacket.Collided).Where(x => x == true).Subscribe(x =>
            {
                popUpWindow.DataContext = ViewModel.PopUp;
                popUpWindow.ShowDialog(this);
            })));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keyboard.Keys.Add(e.Key);
            base.OnKeyDown(e);

            MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;

            if (Keyboard.IsKeyDown(Key.Up))
            {
                viewModel.CourseDisplay.KeyUp();
            }

            if (Keyboard.IsKeyDown(Key.Down))
            {
                viewModel.CourseDisplay.KeyDown();
            }

            if (Keyboard.IsKeyDown(Key.Left))
            {
                viewModel.CourseDisplay.KeyLeft();
            }

            if (Keyboard.IsKeyDown(Key.Right))
            {
                viewModel.CourseDisplay.KeyRight();
            }

            if (Keyboard.IsKeyDown(Key.PageUp))
            {
                viewModel.CourseDisplay.PageUp();
            }

            if (Keyboard.IsKeyDown(Key.PageDown))
            {
                viewModel.CourseDisplay.PageDown();
            }

            if (Keyboard.IsKeyDown(Key.D1))
            {
                viewModel.CourseDisplay.ToggleDebug();
            }

            if (Keyboard.IsKeyDown(Key.D2))
            {
                viewModel.CourseDisplay.ToggleCamera();
            }

            if (Keyboard.IsKeyDown(Key.D3))
            {
                viewModel.CourseDisplay.ToggleRadar();
            }

            if (Keyboard.IsKeyDown(Key.D4))
            {
                viewModel.CourseDisplay.ToggleUltrasonic();
            }

            if (Keyboard.IsKeyDown(Key.D5))
            {
                viewModel.CourseDisplay.ToggleRotation();
            }

            if (Keyboard.IsKeyDown(Key.F1))
            {
                new HelpWindow().Show();
                Keyboard.Keys.Remove(Key.F1);
            }

            if (Keyboard.IsKeyDown(Key.F5))
            {
                viewModel.NextControlledCar();
                Keyboard.Keys.Remove(Key.F5);
            }

            if (Keyboard.IsKeyDown(Key.F6))
            {
                viewModel.PrevControlledCar();
                Keyboard.Keys.Remove(Key.F5);
            }

            var scrollViewer = this.Get<CourseDisplayView>("courseDisplay").Get<ScrollViewer>("scrollViewer");
            viewModel.CourseDisplay.FocusCar(scrollViewer);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;

            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                viewModel.CourseDisplay.OnKeyUp("Empty");
            }

            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                viewModel.CourseDisplay.OnKeyUp("Center");
            }

            Keyboard.Keys.Remove(e.Key);
            base.OnKeyUp(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}