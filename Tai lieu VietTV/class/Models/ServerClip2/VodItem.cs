using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.ServerClip2
{
	public class VodItem
	{
		public List<Content> Contents
		{
			get;
			set;
		}

		public long Createddate
		{
			get;
			set;
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

		public long Modifieddate
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public List<tiviViet.Models.ServerClip2.Objectlist> Objectlist
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

		public bool Type
		{
			get;
			set;
		}

		public VodItem()
		{
		}
	}
}