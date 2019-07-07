using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VIdeoCloop
{
    public partial class FormUSB : Form
    {
        public FormUSB()
        {
            InitializeComponent();
        }

        public DriveInfo Drive { get; set; }

        private void FormUSB_Load(object sender, EventArgs e)
        {

            ManagementEventWatcher watcherConnect = new ManagementEventWatcher();
            WqlEventQuery queryconnect = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
            // watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            watcherConnect.EventArrived += Watcher_EventArrivedConnect;
            watcherConnect.Query = queryconnect;
            watcherConnect.Start();
            watcherConnect.WaitForNextEvent();
            ManagementEventWatcher watcherdisConnect = new ManagementEventWatcher();
            WqlEventQuery querydisconnect = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 3");
            // watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            watcherdisConnect.EventArrived += Watcher_EventArrivedDisconnect;
            watcherdisConnect.Query = querydisconnect;
            watcherdisConnect.Start();
            watcherdisConnect.WaitForNextEvent();

            /*      while (true)
                  {
                      foreach (ManagementObject device in new ManagementObjectSearcher(@"SELECT * FROM Win32_DiskDrive WHERE InterfaceType LIKE 'USB%'").Get())
                      {
                          Console.WriteLine((string)device.GetPropertyValue("DeviceID"));
                          Console.WriteLine((string)device.GetPropertyValue("PNPDeviceID"));

                          foreach (ManagementObject partition in new ManagementObjectSearcher(
                              "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + device.Properties["DeviceID"].Value
                              + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                          {
                              foreach (ManagementObject disk in new ManagementObjectSearcher(
                                          "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='"
                                              + partition["DeviceID"]
                                              + "'} WHERE AssocClass = Win32_LogicalDiskToPartition").Get())
                              {
                                  Console.WriteLine ("Drive letter " + disk["Name"] + "\nPNPDeviceID:" + device.Properties["PNPDeviceID"].Value + "\nSerialNumber:" + device.Properties["SerialNumber"].Value + "\nModel:" + device.Properties["Model"].Value + "\nBytesPerSector:" + device.Properties["BytesPerSector"].Value + "\nFirmwareRevision:" + device.Properties["FirmwareRevision"].Value + "\nSize:" + device.Properties["Size"].Value + "\nPNPDeviceId:" + device.Properties["PNPDeviceId"].Value);
                              }
                              Thread.Sleep(500);
                          }
                      }
                  }
                  this.Text = Drive.Name + "(" + Drive.VolumeLabel + ")" + FormatBytes(Drive.AvailableFreeSpace) + "/" + FormatBytes(Drive.TotalSize);
                  //   XCopy(@"C:\Users\Server\Downloads\GT-MP-Client-Setup.exe", Drive.Name+ @"GT-MP-Client-Setup.exe");
               //*   File.SetAttributes(Drive.Name + @"GT-MP-Client-Setup.exe", FileAttributes.ReadOnly);
                  File.Create(Drive.Name + "VideoCloop");
                  File.SetAttributes(Drive.Name + "VideoCloop", FileAttributes.Hidden);
                  Drive.VolumeLabel = "mioo";
                  */
        }

        private void Watcher_EventArrivedConnect(object sender, EventArrivedEventArgs e)
        {
            MessageBox.Show("connect");
        }
        private void Watcher_EventArrivedDisconnect(object sender, EventArrivedEventArgs e)
        {
            MessageBox.Show("Discoonect");
        }
        public List<USBDeviceInfo> GetUSBDevices()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub");
            ManagementObjectCollection collection = searcher.Get();

            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();
            foreach (var device in collection)
            {
                USBDeviceInfo deviceInfo = new USBDeviceInfo();
                deviceInfo.Availability = (String)device.GetPropertyValue("Availability");
                deviceInfo.Caption = (String)device.GetPropertyValue("Caption");
                deviceInfo.ClassCode = (String)device.GetPropertyValue("ClassCode");
                deviceInfo.ConfigManagerErrorCode = (UInt32)device.GetPropertyValue("ConfigManagerErrorCode");
                deviceInfo.ConfigManagerUserConfig = (Boolean)device.GetPropertyValue("ConfigManagerUserConfig");
                deviceInfo.CreationClassName = (String)device.GetPropertyValue("CreationClassName");
                deviceInfo.CurrentAlternateSettings = (String)device.GetPropertyValue("CurrentAlternateSettings");
                deviceInfo.CurrentConfigValue = (String)device.GetPropertyValue("CurrentConfigValue");
                deviceInfo.Description = (String)device.GetPropertyValue("Description");
                deviceInfo.DeviceID = (String)device.GetPropertyValue("DeviceID");
                deviceInfo.ErrorCleared = (String)device.GetPropertyValue("ErrorCleared");
                deviceInfo.ErrorDescription = (String)device.GetPropertyValue("ErrorDescription");
                deviceInfo.GangSwitched = (String)device.GetPropertyValue("GangSwitched");
                deviceInfo.InstallDate = (String)device.GetPropertyValue("InstallDate");
                deviceInfo.LastErrorCode = (String)device.GetPropertyValue("LastErrorCode");
                deviceInfo.Name = (String)device.GetPropertyValue("Name");
                deviceInfo.NumberOfConfigs = (String)device.GetPropertyValue("NumberOfConfigs");
                deviceInfo.NumberOfPorts = (String)device.GetPropertyValue("NumberOfPorts");
                deviceInfo.PNPDeviceID = (String)device.GetPropertyValue("PNPDeviceID");
                deviceInfo.PowerManagementCapabilities = (String)device.GetPropertyValue("PowerManagementCapabilities");
                deviceInfo.PowerManagementSupported = (String)device.GetPropertyValue("PowerManagementSupported");
                deviceInfo.ProtocolCode = (String)device.GetPropertyValue("ProtocolCode");
                deviceInfo.Status = (String)device.GetPropertyValue("Status");
                deviceInfo.StatusInfo = (String)device.GetPropertyValue("StatusInfo");
                deviceInfo.SubclassCode = (String)device.GetPropertyValue("SubclassCode");
                deviceInfo.SystemCreationClassName = (String)device.GetPropertyValue("SystemCreationClassName");
                deviceInfo.SystemName = (String)device.GetPropertyValue("SystemName");
                deviceInfo.USBVersion = (String)device.GetPropertyValue("USBVersion");
                devices.Add(deviceInfo);
            }

            collection.Dispose();
            searcher.Dispose();
            return devices;
        }

        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName,
    CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref Int32 pbCancel,
    CopyFileFlags dwCopyFlags);

        delegate CopyProgressResult CopyProgressRoutine(
        long TotalFileSize,
        long TotalBytesTransferred,
        long StreamSize,
        long StreamBytesTransferred,
        uint dwStreamNumber,
        CopyProgressCallbackReason dwCallbackReason,
        IntPtr hSourceFile,
        IntPtr hDestinationFile,
        IntPtr lpData);

        int pbCancel;

        enum CopyProgressResult : uint
        {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }

        enum CopyProgressCallbackReason : uint
        {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        [Flags]
        enum CopyFileFlags : uint
        {
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
        }

        private void XCopy(string oldFile, string newFile)
        {
            CopyFileEx(oldFile, newFile, new CopyProgressRoutine(this.CopyProgressHandler), IntPtr.Zero, ref pbCancel, CopyFileFlags.COPY_FILE_RESTARTABLE);
        }

        private CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize, long StreamByteTrans, uint dwStreamNumber, CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            return CopyProgressResult.PROGRESS_CONTINUE;
        }
    }
    public class USBDeviceInfo
    {
        public String Availability { get; set; }
        public String Caption { get; set; }
        public String ClassCode { get; set; }
        public UInt32 ConfigManagerErrorCode { get; set; }
        public Boolean ConfigManagerUserConfig { get; set; }
        public String CreationClassName { get; set; }
        public String CurrentAlternateSettings { get; set; }
        public String CurrentConfigValue { get; set; }
        public String Description { get; set; }
        public String DeviceID { get; set; }
        public String ErrorCleared { get; set; }
        public String ErrorDescription { get; set; }
        public String GangSwitched { get; set; }
        public String InstallDate { get; set; }
        public String LastErrorCode { get; set; }
        public String Name { get; set; }
        public String NumberOfConfigs { get; set; }
        public String NumberOfPorts { get; set; }
        public String PNPDeviceID { get; set; }
        public String PowerManagementCapabilities { get; set; }
        public String PowerManagementSupported { get; set; }
        public String ProtocolCode { get; set; }
        public String Status { get; set; }
        public String StatusInfo { get; set; }
        public String SubclassCode { get; set; }
        public String SystemCreationClassName { get; set; }
        public String SystemName { get; set; }
        public String USBVersion { get; set; }
    }

   
}
