namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using Avalonia.Collections;
    using ReactiveUI;

    public class SensorPacket : ReactiveObject, ISensorPacket
    {
        private List<WorldObject> relevantWorldObjs;

        public List<WorldObject> RelevantWorldObjs
        {
            get => this.relevantWorldObjs;
            set => this.RaiseAndSetIfChanged(ref this.relevantWorldObjs, value);
        }
    }
}
