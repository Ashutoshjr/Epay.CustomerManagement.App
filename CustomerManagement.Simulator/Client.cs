using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Simulator
{
    public class Client
    {
        public static async Task Main(string[] args)
        {
            var simulator = new Simulator();
            await simulator.RunSimulation(10); 
        }
    }
}
