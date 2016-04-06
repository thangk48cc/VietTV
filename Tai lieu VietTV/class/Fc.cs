using System;
using System.IO;
using System.Text;

namespace tiviViet.ViewModels.Mytv
{
	internal class Fc
	{
		public Fc()
		{
		}

		public string Base64Decode(string data)
		{
			string str;
			try
			{
				Decoder decoder = (new UTF8Encoding()).GetDecoder();
				byte[] numArray = new byte[data.get_Length()];
				byte[] numArray1 = Convert.FromBase64String(data);
				char[] chrArray = new char[decoder.GetCharCount(numArray1, 0, (int)numArray1.Length)];
				decoder.GetChars(numArray1, 0, (int)numArray1.Length, chrArray, 0);
				str = new string(chrArray);
			}
			catch (Exception exception)
			{
				str = "";
			}
			return str;
		}

		public string Base64Encode(string data)
		{
			string base64String;
			try
			{
				byte[] numArray = new byte[data.get_Length()];
				base64String = Convert.ToBase64String(Encoding.get_UTF8().GetBytes(data));
			}
			catch (Exception exception)
			{
				throw new Exception(string.Concat("Error in base64Encode", exception.get_Message()));
			}
			return base64String;
		}

		public string DeCodeTid(string tid)
		{
			return this.Base64Decode(this.Base64Decode(this.Base64Decode(tid.Remove(11, 1).Remove(9, 1).Remove(7, 1).Remove(5, 1).Remove(3, 1).Remove(1, 1)).Remove(11, 1).Remove(9, 1).Remove(7, 1).Remove(5, 1).Remove(3, 1).Remove(1, 1)).Remove(11, 1).Remove(9, 1).Remove(7, 1).Remove(5, 1).Remove(3, 1).Remove(1, 1));
		}

		public string EnCodeTid(string tidTime)
		{
			return this.Base64Encode(this.Base64Encode(tidTime).Insert(1, "1").Insert(3, "2").Insert(5, "3").Insert(7, "4").Insert(9, "5").Insert(11, "6")).Insert(1, "1").Insert(3, "2").Insert(5, "3").Insert(7, "4").Insert(9, "5").Insert(11, "6");
		}

		public DateTime FromPhpTime(long ticks)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
			return dateTime.Add(new TimeSpan(0, 0, (int)ticks));
		}

		public string GetTid(long tid, DateTime timetid)
		{
			long num = tid + (this.Time(DateTime.get_Now()) - this.Time(timetid));
			return this.EnCodeTid(num.ToString());
		}

		public static void LogMo(string mess)
		{
			try
			{
				FileStream fileStream = new FileStream(string.Concat(Directory.GetCurrentDirectory(), "/log.txt"), 6, 2);
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.WriteLine(mess);
				streamWriter.Flush();
				streamWriter.Close();
				fileStream.Close();
			}
			catch
			{
			}
		}

		public long Time(DateTime time)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
			return (long)time.Subtract(dateTime).get_TotalSeconds();
		}

		public TimeSpan ToTimer(int number)
		{
			return new TimeSpan(number / 3600, number / 60 % 60, number % 60);
		}

		public string ToTimerString(int number)
		{
			int num = number / 3600;
			int num1 = number / 60 % 60;
			int num2 = number % 60;
			string str = (num2 >= 10 ? num2.ToString() : string.Concat("0", num2.ToString()));
			string str1 = (num1 >= 10 ? num1.ToString() : string.Concat("0", num1.ToString()));
			return string.Concat(new string[] { (num >= 10 ? num.ToString() : string.Concat("0", num.ToString())), ":", str1, ":", str });
		}
	}
}