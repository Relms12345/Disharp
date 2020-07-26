using System.Text;
using Ionic.Zlib;

namespace Disharp.Utils
{
	public static class Compress
	{
		public static string CompressStream(this byte[] stream)
		{
			var compressedStream = ZlibStream.CompressBuffer(stream);

			return Encoding.UTF8.GetString(compressedStream, 0, compressedStream.Length);
		}
	}
}