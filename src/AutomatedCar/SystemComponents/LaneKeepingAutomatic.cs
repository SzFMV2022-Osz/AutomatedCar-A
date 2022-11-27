namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia.Media;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LaneKeepingAutomatic : SystemComponent
    {
        private IList<WorldObject> roadLanes;
        private AutomatedCar car;
        private LaneKeepingPacket laneKeepingPacket;

        private int x = 2;    // I use this number to 'calibrate' at which point the steer wheel needs to be used.
        private int currentRPM;

        public LaneKeepingAutomatic(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.laneKeepingPacket = new LaneKeepingPacket();
            virtualFunctionBus.LeaneKeepingPacket = this.laneKeepingPacket;
            this.currentRPM = virtualFunctionBus.PowertrainPacket.Rpm;
        }

        public override void Process()
        {
            var car = World.Instance.ControlledCar;
            this.roadLanes = this.virtualFunctionBus.CameraPacket.RelevantWorldObjs;

            // This packet will be implemented in other way ...
            if (this.laneKeepingPacket.LaneKeepingStatus == LaneKeepingStatus.Activated)
            {
                this.Corrigate();
            }
        }

        /// <summary>
        /// Corrigate the car positon from the side of the lane toward to the middle.
        /// </summary>
        public void Corrigate()
        {
            WorldObject closestObject = ;   // Needs to be implemented to get somehow the closest object values.
                                            // Needs to be calculated from the roadLanes and the car current position to know where we need to turn.

            if (this.virtualFunctionBus.PowertrainPacket.CurrentGear == "D")
            {
                this.RotateCar(closestObject);
            }
        }

        private void RotateCar(WorldObject closestObject)
        {
            this.virtualFunctionBus.InputPacket.PedalState = Pedals.Gas;

            // The steering intervention depends on the car orientation and the closest object orientation.
            if (this.car.Rotation > closestObject.Rotation + this.x)
            {
                this.virtualFunctionBus.InputPacket.SteeringState = SteeringState.Left;
            }
            else if (this.car.Rotation < closestObject.Rotation - this.x)
            {
                this.virtualFunctionBus.InputPacket.SteeringState = SteeringState.Right;
            }

            throw new NotImplementedException();
        }
    }
}
