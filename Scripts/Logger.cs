using System;
using System.IO;
using UnityEngine;
using Object = System.Object;


namespace UM_Logger
{


	public static class Logger
	{

		public static bool IsActivated = true;
		public static bool IsFileSave  = false;

		public static void Log<T>(this T type, string message)
		{
			if (!IsActivated) return;
			message = "[" + DateTime.Now.ToString() + "]" + typeof(T).Name + " - " + message;
			Debug.Log(message);
			if (IsFileSave)
			{
				using (var writer = File.AppendText($"{Application.persistentDataPath}/EventLog.txt"))
				{
					writer.WriteLine(message);
				}
			}
		}

		public static void Print(string message)
		{
			Debug.Log(message);
		}

	}


}