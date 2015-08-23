using System;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
	public static class WindowsServices
	{
		private static ServiceController[] _services = null;
		private static ServiceController[] Services
		{
			get
			{
				if (WindowsServices._services == null)
				{
					WindowsServices._services = ServiceController.GetServices();
				}
				return WindowsServices._services;
			}
		}
		private static ServiceController[] _devices = null;
		private static ServiceController[] Devices
		{
			get
			{
				if (WindowsServices._devices == null)
				{
					WindowsServices._devices = ServiceController.GetDevices();
				}
				return WindowsServices._devices;
			}
		}
		public static ServiceController GetServiceControl(string serviceName)
		{
			return WindowsServices.GetServiceControl(new string[] { serviceName });
		}
		public static ServiceController GetServiceControl(string serviceName, bool dontUseCache)
		{
			return WindowsServices.GetServiceControl(new string[] { serviceName }, dontUseCache);
		}
		public static ServiceController GetServiceControl(string[] possibleServiceNames)
		{
			return WindowsServices.GetServiceControl(possibleServiceNames, false);
		}
		public static ServiceController GetServiceControl(string[] possibleServiceNames, bool dontUseCache)
		{
			if (dontUseCache)
			{
				WindowsServices._services = null;
				WindowsServices._devices = null;
			}
			foreach (ServiceController sc in WindowsServices.Services)
			{
				foreach (string sn in possibleServiceNames)
				{
					if (string.Equals(sn, sc.ServiceName, StringComparison.InvariantCultureIgnoreCase))
					{
						return sc;
					}
				}
			}
			foreach (ServiceController sc in WindowsServices.Devices)
			{
				foreach (string sn in possibleServiceNames)
				{
					if (string.Equals(sn, sc.ServiceName, StringComparison.InvariantCultureIgnoreCase))
					{
						return sc;
					}
				}
			}
			return null;
		}
	}
}
