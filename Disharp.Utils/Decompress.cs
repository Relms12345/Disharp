using System.Text;
using Ionic.Zlib;

namespace Disharp.Utils
{
	public static class Decompress
	{
		public static string DecompressStream(this byte[] stream)
		{
			var decompressedStream = ZlibStream.UncompressBuffer(stream);

			return Encoding.UTF8.GetString(decompressedStream, 0, decompressedStream.Length);
		}
	}
}