using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using tiviViet.Models;

namespace tiviViet.ViewModels
{
	internal class GetDataShedule
	{
		public static ObservableCollection<DetailsChannel> LisDetailsChannels
		{
			get;
			set;
		}

		public static ObservableCollection<Grouplist> LisGrouplist
		{
			get;
			set;
		}

		public GetDataShedule()
		{
			GetDataShedule.LisDetailsChannels = new ObservableCollection<DetailsChannel>();
			this.GetShedule();
		}

		private async void GetShedule()
		{
			GetDataShedule.<GetShedule>d__9 variable = new GetDataShedule.<GetShedule>d__9();
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetDataShedule.<GetShedule>d__9>(ref variable);
		}
	}
}