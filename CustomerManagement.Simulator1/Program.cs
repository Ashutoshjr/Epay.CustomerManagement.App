namespace CustomerManagement.Simulator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("start api execuation process");

            var simulator = new Simulator();
            await simulator.RunSimulation(6);

            Console.WriteLine("multiple api execuation completed");
            Console.ReadLine();
        }
    }
}