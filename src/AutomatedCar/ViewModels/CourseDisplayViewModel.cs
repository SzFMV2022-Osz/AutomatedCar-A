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

        }

        public void KeyDown()
        {
            // World.Instance.ControlledCar.Y += 5;
            Manager.BrakePedal();
        }

        public void KeyLeft()
        {
            // World.Instance.ControlledCar.X -= 5;
            Manager.TurnLeft();
        }

        public void KeyRight()
        {
            // World.Instance.ControlledCar.X += 5;
            Manager.TurnRight();
        }

        public void PageUp()
        {
            // World.Instance.ControlledCar.Rotation += 5;
            Manager.ShiftUp();
        }

        public void PageDown()
        {
            // World.Instance.ControlledCar.Rotation -= 5;
            Manager.ShiftDown();
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

        public void OnKeyUp(Key key)
        {
            if (key == Key.Up || key == Key.Down)
            {
                
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