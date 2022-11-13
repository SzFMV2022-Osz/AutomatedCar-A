namespace AutomatedCar.SystemComponents.CruiseControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CruiseControl : SystemComponent
    {
        public CruiseControl(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
