using System.Collections.ObjectModel;
using AutomatedCar.Models;
using System.Linq;

using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.InputManager.InputHandler;
    using Avalonia.Controls;
    using JetBrains.Annotations;
    using Models;
    using System;
    using Visualization;
    using Avalonia.Input;

    public class CourseDisplayViewModel : ViewModelBase
    {
        public ObservableCollection<WorldObjectViewModel> WorldObjects { get; } = new ObservableCollection<WorldObjectViewModel>();

        private Avalonia.Vector offset;

        public CourseDisplayViewModel(World world)
        {
            this.WorldObjects = new ObservableCollection<WorldObjectViewModel>(world.WorldObjects.Select(wo => new WorldObjectViewModel(wo)));
            this.Width = world.Width;
            this.Height = world.Height;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public Avalonia.Vector Offset
        {
            get => this.offset;
            set => this.RaiseAndSetIfChanged(ref this.offset, value);
        }

        private DebugStatus debugStatus = new DebugStatus();

        public DebugStatus DebugStatus
        {
            get => this.debugStatus;
            set => this.RaiseAndSetIfChanged(ref this.debugStatus, value);
        }

        public void KeyUp()
        {
            // World.Instance.ControlledCar.Y -= 5;
            ControlMessenger.Instance.FirePedalEvent(Pedals.Gas);
        }

        public void KeyDown()
        {
            // World.Instance.ControlledCar.Y += 5;
            ControlMessenger.Instance.FirePedalEvent(Pedals.Brake);
        }

        public void KeyLeft()
        {
            // World.Instance.ControlledCar.X -= 5;
            ControlMessenger.Instance.FireSteeringEvent(SteeringState.Left);
        }

        public void KeyRight()
        {
            // World.Instance.ControlledCar.X += 5;
            ControlMessenger.Instance.FireSteeringEvent(SteeringState.Right);
        }

        public void PageUp()
        {
            // World.Instance.ControlledCar.Rotation += 5;
            ControlMessenger.Instance.FireGearboxEvent(Gears.ShiftUp);
        }

        public void PageDown()
        {
            // World.Instance.ControlledCar.Rotation -= 5;
            ControlMessenger.Instance.FireGearboxEvent(Gears.ShiftDown);
        }

        public void ToggleDebug()
        {
            this.debugStatus.Enabled = !this.debugStatus.Enabled;
        }

        public void ToggleCamera()
        {
            this.DebugStatus.Camera = !this.DebugStatus.Camera;
        }

        public void ToggleRadar()
        {
             // World.Instance.DebugStatus.Radar = !World.Instance.DebugStatus.Radar;
        }

        public void ToggleUltrasonic()
        {
            // World.Instance.DebugStatus.Ultrasonic = !World.Instance.DebugStatus.Ultrasonic;
        }

        public void ToggleRotation()
        {
            // World.Instance.DebugStatus.Rotate = !World.Instance.DebugStatus.Rotate;
        }
        public void ToggleLKA()
        {

        }

        public void OnKeyUp(Key key)
        {
            if (key == Key.Up || key == Key.Down)
            {
                ControlMessenger.Instance.FirePedalEvent(Pedals.Empty);
            }

            if (key == Key.Left || key == Key.Right)
            {
                ControlMessenger.Instance.FireSteeringEvent(SteeringState.Center);
            }

            if (key == Key.PageUp || key == Key.PageDown)
            {
                ControlMessenger.Instance.FireGearboxEvent(Gears.Steady);
            }
        }

        public void FocusCar(ScrollViewer scrollViewer)
        {
            var offsetX = World.Instance.ControlledCar.X - (scrollViewer.Viewport.Width / 2);
            var offsetY = World.Instance.ControlledCar.Y - (scrollViewer.Viewport.Height / 2);
            this.Offset = new Avalonia.Vector(offsetX, offsetY);
        }
    }
}