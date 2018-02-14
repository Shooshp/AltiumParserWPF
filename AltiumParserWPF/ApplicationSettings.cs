using System.Deployment.Application;
using System.IO;
using System.Reflection;

namespace AltiumParserWPF
{
    public static class ApplicationSettings
    {
        public static string Name
        {
            get
            {
                var name = Path.GetFileName(Assembly.GetEntryAssembly().GetName().Name);

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    name += @" " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
                }

                return name;
            }
        }                                    
    }
}
