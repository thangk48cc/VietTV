using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.plus
{
	internal class ObjectPlus
	{
		public List<DataPlus> Data
		{
			get;
			set;
		}

		public int IsLogin
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public ObjectPlus()
		{
		}
	}
}