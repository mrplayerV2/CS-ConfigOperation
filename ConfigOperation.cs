using System;
using System.Configuration;
using System.Xml;

internal class ConfigOperation
{
	public static string GetConnectionString(string ConnName)
	{
		return ConfigurationManager.ConnectionStrings[ConnName].ConnectionString;
	}

	public static string GetAppSettings(string SettingName)
	{
		try
		{
			return ConfigurationManager.AppSettings[SettingName].ToString();
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public static string GetAppSettings(string configFile, string SettingName)
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(configFile);
			return xmlDocument.DocumentElement.SelectSingleNode("//add[@key='" + SettingName + "']")?.Attributes["value"].Value;
		}
		catch
		{
			return null;
		}
	}

	public static bool SetAppSettings(string AppKey, string AppValue)
	{
		return SetAppSettings(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, AppKey, AppValue);
	}

	public static bool SetAppSettings(string configFile, string AppKey, string AppValue)
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(configFile);
			XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//add[@key='" + AppKey + "']");
			if (xmlNode == null)
			{
				return false;
			}
			xmlNode.Attributes["value"].Value = AppValue;
			xmlDocument.Save(configFile);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
