using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using tiviViet.ViewModels;

namespace tiviViet.Models.ServerClip2
{
	public class Vod
	{
		public object Createddate
		{
			get;
			set;
		}

		public string Date
		{
			get
			{
				return ConverTime.FooballCell(this.Modifieddate);
			}
		}

		public string Description
		{
			get;
			set;
		}

		public List<object> Grouplist
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

		public string Mediaid
		{
			get;
			set;
		}

		public double Modifieddate
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public List<ObjectlistVod> Objectlist
		{
			get;
			set;
		}

		public string Objectlistresult
		{
			get;
			set;
		}

		public string Pictures
		{
			get;
			set;
		}

		public string Tentran
		{
			get
			{
				return this.Name.Substring(this.Name.IndexOf(":") + 1, this.Name.LastIndexOf(":") - this.Name.IndexOf(":") - 1);
			}
		}

		public string Tengiaidau
		{
			get
			{
				return this.Name.Substring(0, this.Name.IndexOf(":"));
			}
		}

		public bool Type
		{
			get;
			set;
		}

		public Vod()
		{
		}
	}
}