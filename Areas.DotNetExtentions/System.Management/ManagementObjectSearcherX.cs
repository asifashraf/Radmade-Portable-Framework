using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace RadApi.Core
{
    public class ManagementObjectSearcherRL : ManagementObjectSearcher
    {
        //get source properties
        public List<PropertyData> GetPropertiesBySource(ManagementSourceEnum queryObject, 
            params string[] filterProperties_emptyForAll)
        {
            int i = 0; 
            var hd = new List<PropertyData>();
            this.Query = new ObjectQuery("SELECT * FROM " + queryObject.Name());
            foreach (ManagementObject wmi_HD in this.Get())
            {
                i++;
                PropertyDataCollection searcherProperties = wmi_HD.Properties;
                foreach (PropertyData sp in searcherProperties)
                {
                    if (filterProperties_emptyForAll.CountedZero())
                        hd.Add(sp);
                    else
                    {
                        var propertyMatched = from item in filterProperties_emptyForAll
                                              where item.ToLower() == sp.Name.ToLower()
                                              select item;
                        if (propertyMatched.CountedPositive())
                        {
                            hd.Add(sp);
                        }
                    }
                }
            }

            return hd;
        }
        
        //get all information at once
        public SystemInformation GetSystemInformation()
        {
            SystemInformation info = new SystemInformation();

            //computer system
            info.ComputerSystem = GetSysInfoDataAsKeyValues(ManagementSourceEnum.Win32_ComputerSystem,
               ComputerSystemPropertiesEnum.Caption.GetNames<ComputerSystemPropertiesEnum>());

            //hard drive
            info.DiskDrive = GetSysInfoDataAsKeyValues(ManagementSourceEnum.Win32_DiskDrive,
                HddPropertiesEnum.Name.GetNames<HddPropertiesEnum>());

            //operating system
            info.OperatingSystem = GetSysInfoDataAsKeyValues(ManagementSourceEnum.Win32_OperatingSystem,
               OperatingSystemPropertiesEnum.BootDevice.GetNames<OperatingSystemPropertiesEnum>());

            //processor
            info.Processor = GetSysInfoDataAsKeyValues(ManagementSourceEnum.Win32_Processor,
               ProcessorPropertiesEnum.Caption.GetNames<ProcessorPropertiesEnum>()
                );

            //system devices
            info.SystemDevices = GetSysInfoDataAsKeyValues(ManagementSourceEnum.Win32_SystemDevices);

            //startup programs
            info.StartupPrograms = GetSysInfoDataAsKeyValues(ManagementSourceEnum.Win32_StartupCommand);

            return info;
        }

        private KeyValue[] GetSysInfoDataAsKeyValues(ManagementSourceEnum infoSource,
           params string[] goodProperties)
        {
            var propertyData = this.GetPropertiesBySource(infoSource, goodProperties);

            List<KeyValue> parsed = new List<KeyValue>();
            foreach (var hddP in propertyData)
            {
                parsed.Add(new KeyValue(hddP.Name, hddP.Value.Text()));
            }
            return parsed.ToArray<KeyValue>();
        }
    }
}
