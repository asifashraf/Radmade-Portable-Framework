using System.Linq;
using System.ServiceProcess;


public static class ServiceControllerArray
    {
        public static ServiceController ByName(
            this ServiceController[] array, string name)
        {
            return (from a in array 
                    where a.ServiceName.ToLower() == name.ToLower()
                    || a.DisplayName.ToLower() == name.ToLower()
                    select a).One();
        }
    }

