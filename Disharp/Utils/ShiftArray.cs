using System;

namespace Disharp.Utils
{
	public static class ShiftArray
	{
		public static void ShiftLeft<T>(T[] arr, int shifts)
		{
			Array.Copy(arr, shifts, arr, 0, arr.Length - shifts);
			Array.Clear(arr, arr.Length - shifts, shifts);
		}
	}
}