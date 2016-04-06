using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.ServerBongda2
{
	internal class UserInfoRequest
	{
		[JsonProperty("userinfo")]
		public UserInfo Result
		{
			get;
			set;
		}

		public UserInfoRequest()
		{
		}
	}
}