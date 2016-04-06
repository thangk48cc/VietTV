using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.plus
{
	internal class Session
	{
		public string CurrentService
		{
			get;
			set;
		}

		public string ExpiryDate
		{
			get;
			set;
		}

		public List<tiviViet.Models.plus.LiveChannel> LiveChannel
		{
			get;
			set;
		}

		public string Registered
		{
			get;
			set;
		}

		public string session
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public Session()
		{
		}
	}
}