using System.ServiceProcess;

namespace TimerService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new TimeTrackingService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
