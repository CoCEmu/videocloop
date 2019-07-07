using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace VIdeoCloop
{
    class UsbDetector
    {
        public List<DriveInfo> ExistRemoveableDeviceList;

        public UsbDetector()
        {
            ExistRemoveableDeviceList = new List<DriveInfo>();
            DriveInfo[] mydrives = DriveInfo.GetDrives();
            foreach (DriveInfo mydrive in mydrives)
            {
                if (mydrive.DriveType == DriveType.Removable)
                {
                    ExistRemoveableDeviceList.Add(mydrive);
                }
            }
        }
    }
}