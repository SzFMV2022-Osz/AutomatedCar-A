namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReadOnlyAEBPacket
    {
        bool CollisionPredicted { get; }
        bool EmergencyBreakActivated { get; }
    }
}
