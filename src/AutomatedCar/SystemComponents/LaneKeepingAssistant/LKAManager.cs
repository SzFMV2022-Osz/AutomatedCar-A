namespace AutomatedCar.SystemComponents.LaneKeepingAssistant
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.LaneKeepingAssistant.Validation;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static AutomatedCar.SystemComponents.LaneKeepingAssistant.Validation.ILKAValidation;

    public class LKAManager : SystemComponent
    {
        private ILKAPacket lkapacket;
        private ILKAValidation lkavalidation;

        public bool LKAIsOn = false;
        public bool canLKARun = false;

        public List<Coordinate> roadSides = new List<Coordinate>();
        public int carX;
        public int carY;

        public LKAManager(VirtualFunctionBus virtualFunctionBus)
            :base(virtualFunctionBus)
        {
            this.lkapacket = new LKAPacket();
            virtualFunctionBus.LKAPacket = this.lkapacket;
            this.lkapacket.LaneKeepingStatus = false;
            this.lkapacket.LaneKeepingAvailable = false;
            this.lkavalidation = new LKAValidation();
        }

        public override void Process()
        {
            LkaInputs input = LkaInputs.Empty;

            this.virtualFunctionBus.InputPacket.LKAInputs.TryDequeue(out input);

            if (input == LkaInputs.Empty)
            {
                if (!this.LKAIsOn)
                {
                    this.lkapacket.LaneKeepingStatus = false;
                    this.canLKARun = this.lkavalidation.CanBeTurnedOn(this.roadSides, this.carX, this.carY);
                    if (this.canLKARun)
                    {
                        this.lkapacket.LaneKeepingAvailable = true;
                    }
                }
                else //this.LKAIsOn = true
                {
                    this.lkapacket.LaneKeepingStatus = true;
                    this.canLKARun = !this.lkavalidation.MustBeTurnedOff(this.roadSides, this.carX, this.carY);
                    if (this.canLKARun)
                    {
                        this.lkapacket.LaneKeepingAvailable = false;
                    }
                }
            }

            if (input == LkaInputs.TurnOnOrOff)
            {
                if (!this.LKAIsOn)
                {
                    this.canLKARun = this.lkavalidation.CanBeTurnedOn(this.roadSides, this.carX, this.carY);
                    if (this.canLKARun)
                    {
                        this.lkapacket.LaneKeepingStatus = true;
                        this.lkapacket.LaneKeepingAvailable = true;
                    }
                    else
                    {
                        this.lkapacket.LaneKeepingStatus = false;
                        this.lkapacket.LaneKeepingAvailable = false;
                    }
                }
                else
                {
                    this.lkapacket.LaneKeepingStatus = false;
                    this.lkapacket.LaneKeepingAvailable = false;
                }
            }
        }
    }
}
