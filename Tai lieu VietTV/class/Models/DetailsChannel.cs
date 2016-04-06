using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class DetailsChannel
	{
		public bool Active
		{
			get;
			set;
		}

		public object Createddate
		{
			get;
			set;
		}

		public string Currentstatus
		{
			get;
			set;
		}

		public string Channelid
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public List<tiviViet.Models.Grouplist> Grouplist
		{
			get;
			set;
		}

		public bool Hot
		{
			get;
			set;
		}

		public string Icon
		{
			get;
			set;
		}

		public object Modifieddate
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public List<tiviViet.Models.Objectlist> Objectlist
		{
			get;
			set;
		}

		public string Objectlistresult
		{
			get;
			set;
		}

		public double Starttime
		{
			get;
			set;
		}

		public object Stoptime
		{
			get;
			set;
		}

		public bool Type
		{
			get;
			set;
		}

		public string Thumbnail
		{
			get;
			set;
		}

		public bool Trial
		{
			get;
			set;
		}

		public object Updatetime
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public DetailsChannel()
		{
		}
	}
}