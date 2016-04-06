using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	internal class NewPapper
	{
		public ObservableCollection<tiviViet.Models.Articlelist> Articlelist
		{
			get;
			set;
		}

		public NewPapper()
		{
		}
	}
}