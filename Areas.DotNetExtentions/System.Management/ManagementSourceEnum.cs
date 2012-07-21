namespace RadApi.Core
{
    public enum ManagementSourceEnum
    {
        [StringValue("Operating System")]
        Win32_OperatingSystem,

        [StringValue("Computer System")]
        Win32_ComputerSystem,

        [StringValue("Processor")]
        Win32_Processor,

        [StringValue("Disk Drive")]
        Win32_DiskDrive,

        [StringValue("Startup Command")]
Win32_StartupCommand,
        
        [StringValue("System Devices")]
Win32_SystemDevices
    }
}

//[StringValue("Program Group")]
//        Win32_LogicalProgramGroup,
