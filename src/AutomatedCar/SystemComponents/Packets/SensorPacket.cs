namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using ReactiveUI;

    internal class SensorPacket : ReactiveObject, ISensorPacket
    {
        private List<WorldObject> relevantWorldObjs;

        public List<WorldObject> RelevantWorldObjs
        {
            get => this.RelevantWorldObjs;
            set => this.RaiseAndSetIfChanged(ref this.relevantWorldObjs, value);
        }
    }
}
