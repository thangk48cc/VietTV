using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class ZingArticleDetailByLiveStream
	{
		public ObservableCollection<ZingArticleDetailByArticle> Articles
		{
			get;
			set;
		}

		public bool IsLiveStream
		{
			get;
			set;
		}

		public ZingArticleDetailByLiveStream()
		{
		}
	}
}