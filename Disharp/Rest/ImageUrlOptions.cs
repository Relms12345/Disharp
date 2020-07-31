namespace Disharp.Rest
{
	public abstract class ImageUrlOptions
	{
		public string Extension { get; set; } = "png";
		public int Size { get; set; } = 100;
		public bool Dynamic { get; set; } = true;
	}
}