namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using Avalonia.Collections;
    using ReactiveUI;

    public class SensorPacket : ReactiveObject, ISensorPacket
    {
        private AvaloniaList<WorldObject> relevantWorldObjs;

        public SensorPacket()
        {
            this.relevantWorldObjs = new AvaloniaList<WorldObject>();
        }

        public AvaloniaList<WorldObject> RelevantWorldObjs
        {
            get => this.relevantWorldObjs;
            set
            {
                this.relevantWorldObjs.Clear();
                this.RaiseAndSetIfChanged(ref this.relevantWorldObjs, value, nameof(this.RelevantWorldObjs));
            }
        }
    }
}
