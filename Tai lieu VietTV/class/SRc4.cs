using System;

namespace tiviViet.ViewModels
{
	public static class SRc4
	{
		public static byte[] Decrypt(byte[] pwd, byte[] data)
		{
			return SRc4.Encrypt(pwd, data);
		}

		public static byte[] Encrypt(byte[] pwd, byte[] data)
		{
			int[] numArray = new int[256];
			int[] numArray1 = new int[256];
			byte[] numArray2 = new byte[(int)data.Length];
			for (int i = 0; i < 256; i++)
			{
				numArray[i] = pwd[i % (int)pwd.Length];
				numArray1[i] = i;
			}
			int num = 0;
			int num1 = num;
			int num2 = num;
			while (num1 < 256)
			{
				num2 = (num2 + numArray1[num1] + numArray[num1]) % 256;
				int num3 = numArray1[num1];
				numArray1[num1] = numArray1[num2];
				numArray1[num2] = num3;
				num1++;
			}
			int num4 = 0;
			int num5 = num4;
			int num6 = num4;
			int num7 = num4;
			while (num5 < (int)data.Length)
			{
				num7 = (num7 + 1) % 256;
				num6 = (num6 + numArray1[num7]) % 256;
				int num8 = numArray1[num7];
				numArray1[num7] = numArray1[num6];
				numArray1[num6] = num8;
				int[] numArray3 = numArray1;
				int num9 = numArray3[(numArray3[num7] + numArray1[num6]) % 256];
				numArray2[num5] = (byte)(data[num5] ^ num9);
				num5++;
			}
			return numArray2;
		}
	}
}