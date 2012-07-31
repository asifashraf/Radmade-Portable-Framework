using System.Runtime.Serialization;

public class SystemInformation 
    {
        [DataMember]
        public KeyValue[] DiskDrive { get; set; }
        [DataMember]
        public KeyValue[] ComputerSystem { get; set; }
        [DataMember]
        public KeyValue[] OperatingSystem { get; set; }
        [DataMember]
        public KeyValue[] Processor { get; set; }
        [DataMember]
        public KeyValue[] SystemDevices { get; set; }
        [DataMember]
        public KeyValue[] StartupPrograms { get; set; }
 
}
    public class KeyValue {
        public string Key { get; set; }
        public string Value { get; set; }
        public KeyValue(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }