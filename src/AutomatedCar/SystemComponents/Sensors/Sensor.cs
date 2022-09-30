namespace AutomatedCar.SystemComponents.Sensors
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    internal abstract class Sensor : SystemComponent
    {
        protected ISensorPacket sensorPacket;

        public Sensor(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
        }

        protected abstract List<WorldObject> FilterRelevantWorldObjects(List<WorldObject> worldObjects);

        protected abstract void SaveWorldObjectsToPacket(List<WorldObject> worldObjects);

        private AutomatedCar GetAutomatedCar()
        {
            return World.Instance.ControlledCar;
        }

        private List<WorldObject> GetWorldObjects()
        {
            return World.Instance.WorldObjects;
        }
    }
}
