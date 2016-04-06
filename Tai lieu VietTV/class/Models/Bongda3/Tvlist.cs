using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.Bongda3
{
	public class Tvlist
	{
		public string Active
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Hot
		{
			get;
			set;
		}

		public string Icon
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}

		public string Ispay
		{
			get;
			set;
		}

		public string Link
		{
			get;
			set;
		}

		public string LinkDirect
		{
			get;
			set;
		}

		public string LinkDirect0
		{
			get;
			set;
		}

		public string LinkDirect1
		{
			get;
			set;
		}

		public string LinkDirect2
		{
			get;
			set;
		}

		public string LinkDirect3
		{
			get;
			set;
		}

		public string LinkDirectIos
		{
			get;
			set;
		}

		public string Live
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Notice
		{
			get;
			set;
		}

		public string Order
		{
			get;
			set;
		}

		public string Security
		{
			get;
			set;
		}

		public string SelectLink
		{
			get;
			set;
		}

		public string SelectLinkIos
		{
			get;
			set;
		}

		public string SelectLinkWap
		{
			get;
			set;
		}

		public string SelectLinkWindow
		{
			get;
			set;
		}

		public string Servers
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public string Tid
		{
			get;
			set;
		}

		public string TimeDate
		{
			get
			{
				return this.TimeStart.ToString("dd-MM-yyyy");
			}
		}

		public DateTime TimeStart
		{
			get;
			set;
		}

		public string TimeTime
		{
			get
			{
				return this.TimeStart.ToString("HH:mm");
			}
		}

		public string UrlGroup
		{
			get;
			set;
		}

		public string Views
		{
			get;
			set;
		}

		public Tvlist()
		{
		}
	}
}