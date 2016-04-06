using GoogleAnalytics;
using GoogleAnalytics.Core;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Serialization;
using tiviViet.Models;
using tiviViet.Resources;

namespace tiviViet
{
	public class App : Application
	{
		private static string _fileName;

		public static int naptien;

		public static bool sttRemove;

		public static ObservableCollection<Chanel> ListFavorite;

		private bool _phoneApplicationInitialized;

		private bool _contentLoaded;

		public static PhoneApplicationFrame RootFrame
		{
			get;
			private set;
		}

		public static CookieContainer SessionCookieContainer
		{
			get;
			set;
		}

		static App()
		{
			App._fileName = "favorite.txt";
			App.ListFavorite = new ObservableCollection<Chanel>();
		}

		public App()
		{
			base.add_UnhandledException(new EventHandler<ApplicationUnhandledExceptionEventArgs>(this, App.Application_UnhandledException));
			this.InitializeComponent();
			this.InitializePhoneApplication();
			this.InitializeLanguage();
			this.LoadFavorite();
			if (IsolatedStorageFile.GetUserStoreForApplication().FileExists("rm.txt"))
			{
				App.sttRemove = true;
			}
			if (Debugger.get_IsAttached())
			{
				Application.get_Current().get_Host().get_Settings().set_EnableFrameRateCounter(true);
				PhoneApplicationService.get_Current().set_UserIdleDetectionMode(1);
			}
		}

		public static void AddFavorite(Chanel s)
		{
			foreach (Chanel listFavorite in App.ListFavorite)
			{
				if (listFavorite.ChanelName != s.ChanelName)
				{
					continue;
				}
				App.ListFavorite.Remove(listFavorite);
				App.ListFavorite.Insert(0, s);
				App.SaveFavorite();
				return;
			}
			App.ListFavorite.Insert(0, s);
			App.SaveFavorite();
		}

		private void Application_Activated(object sender, ActivatedEventArgs e)
		{
		}

		private void Application_Closing(object sender, ClosingEventArgs e)
		{
		}

		private void Application_Deactivated(object sender, DeactivatedEventArgs e)
		{
		}

		private void Application_Launching(object sender, LaunchingEventArgs e)
		{
		}

		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (Debugger.get_IsAttached())
			{
				EasyTracker.GetTracker().SendException(e.get_ExceptionObject().get_Message(), false);
				Debugger.Break();
			}
		}

		private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
		{
			App.RootFrame.remove_Navigated(new NavigatedEventHandler(this, App.ClearBackStackAfterReset));
			if (e.get_NavigationMode() != null && e.get_NavigationMode() != 3)
			{
				return;
			}
			while (App.RootFrame.RemoveBackEntry() != null)
			{
			}
		}

		private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
		{
			if (base.get_RootVisual() != App.RootFrame)
			{
				base.set_RootVisual(App.RootFrame);
			}
			App.RootFrame.remove_Navigated(new NavigatedEventHandler(this, App.CompleteInitializePhoneApplication));
		}

		private void CheckForResetNavigation(object sender, NavigationEventArgs e)
		{
			if (e.get_NavigationMode() == 4)
			{
				App.RootFrame.add_Navigated(new NavigatedEventHandler(this, App.ClearBackStackAfterReset));
			}
		}

		public static bool DeleteFavorite(string id)
		{
			foreach (Chanel listFavorite in App.ListFavorite)
			{
				if (listFavorite.ChanelName != id)
				{
					continue;
				}
				App.ListFavorite.Remove(listFavorite);
				App.SaveFavorite();
				return true;
			}
			App.SaveFavorite();
			return true;
		}

		public static ObservableCollection<Chanel> DeserializeXmlToList(string listAsXml)
		{
			ObservableCollection<Chanel> observableCollection;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Chanel>));
			ObservableCollection<Chanel> observableCollection1 = new ObservableCollection<Chanel>();
			if (string.IsNullOrEmpty(listAsXml))
			{
				return observableCollection1;
			}
			StringReader stringReader = new StringReader(listAsXml);
			try
			{
				observableCollection = (ObservableCollection<Chanel>)xmlSerializer.Deserialize(stringReader);
			}
			finally
			{
				if (stringReader != null)
				{
					stringReader.Dispose();
				}
			}
			return observableCollection;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/tiviViet;component/App.xaml", 2));
		}

		private void InitializeLanguage()
		{
			try
			{
				App.RootFrame.set_Language(XmlLanguage.GetLanguage(AppResources.ResourceLanguage));
				FlowDirection flowDirection = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
				App.RootFrame.set_FlowDirection(flowDirection);
			}
			catch
			{
				if (Debugger.get_IsAttached())
				{
					Debugger.Break();
				}
				throw;
			}
		}

		private void InitializePhoneApplication()
		{
			if (this._phoneApplicationInitialized)
			{
				return;
			}
			App.RootFrame = new PhoneApplicationFrame();
			App.RootFrame.add_Navigated(new NavigatedEventHandler(this, App.CompleteInitializePhoneApplication));
			App.RootFrame.add_NavigationFailed(new NavigationFailedEventHandler(this, App.RootFrame_NavigationFailed));
			App.RootFrame.add_Navigated(new NavigatedEventHandler(this, App.CheckForResetNavigation));
			App.RootFrame.add_Navigated(new NavigatedEventHandler(this, App.RootFrame_Navigated));
			this._phoneApplicationInitialized = true;
		}

		private void LoadFavorite()
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			try
			{
				if (userStoreForApplication.FileExists(App._fileName))
				{
					IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(App._fileName, 3, userStoreForApplication);
					try
					{
						StreamReader streamReader = new StreamReader(isolatedStorageFileStream);
						try
						{
							App.ListFavorite = App.DeserializeXmlToList(streamReader.ReadToEnd());
							streamReader.Dispose();
							streamReader.Close();
						}
						finally
						{
							if (streamReader != null)
							{
								streamReader.Dispose();
							}
						}
						isolatedStorageFileStream.Dispose();
						isolatedStorageFileStream.Close();
					}
					finally
					{
						if (isolatedStorageFileStream != null)
						{
							isolatedStorageFileStream.Dispose();
						}
					}
				}
				userStoreForApplication.Dispose();
			}
			finally
			{
				if (userStoreForApplication != null)
				{
					userStoreForApplication.Dispose();
				}
			}
		}

		private void RootFrame_Navigated(object sender, NavigationEventArgs e)
		{
			if (e.get_Content() != null)
			{
				EasyTracker.GetTracker().SendView(e.get_Content().ToString());
			}
		}

		private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			if (Debugger.get_IsAttached())
			{
				EasyTracker.GetTracker().SendException(e.get_Exception().get_Message(), false);
				Debugger.Break();
			}
		}

		public static void SaveFavorite()
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			try
			{
				if (userStoreForApplication.FileExists(App._fileName))
				{
					userStoreForApplication.DeleteFile(App._fileName);
				}
				IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(App._fileName, 2, userStoreForApplication);
				try
				{
					StreamWriter streamWriter = new StreamWriter(isolatedStorageFileStream);
					try
					{
						streamWriter.WriteLine(App.SerializeListToXml(App.ListFavorite));
						streamWriter.Dispose();
						streamWriter.Close();
					}
					finally
					{
						if (streamWriter != null)
						{
							streamWriter.Dispose();
						}
					}
					isolatedStorageFileStream.Dispose();
					isolatedStorageFileStream.Close();
				}
				finally
				{
					if (isolatedStorageFileStream != null)
					{
						isolatedStorageFileStream.Dispose();
					}
				}
				userStoreForApplication.Dispose();
			}
			finally
			{
				if (userStoreForApplication != null)
				{
					userStoreForApplication.Dispose();
				}
			}
		}

		public static string SerializeListToXml(ObservableCollection<Chanel> list)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Chanel>));
			StringBuilder stringBuilder = new StringBuilder();
			XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
			xmlWriterSetting.set_Indent(true);
			xmlWriterSetting.set_OmitXmlDeclaration(true);
			XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSetting);
			try
			{
				xmlSerializer.Serialize(xmlWriter, list);
			}
			finally
			{
				if (xmlWriter != null)
				{
					xmlWriter.Dispose();
				}
			}
			return stringBuilder.ToString();
		}
	}
}