using System;
using ImageMagick;

namespace Blurhash.Magick;
public static class Blurhasher
{
    /// <summary>
    /// Encodes a picture into a Blurhash string
    /// </summary>
    /// <param name="image">The picture to encode</param>
    /// <param name="componentsX">The number of components used on the X-Axis</param>
    /// <param name="componentsY">The number of components used on the Y-Axis</param>
    /// <returns>The resulting Blurhash string</returns>
    public static string Encode(MagickImage image, int componentsX, int componentsY)
    {
        var blurhashPixels = new Pixel[image.Width, image.Height];

        var pixels = image.GetPixels();
        var channelsCount = Convert.ToInt32(pixels.Channels);

#if NET8_0_OR_GREATER
        for (int y = 0; y < image.Height; y++)
		{
			var pcv = pixels.GetReadOnlyArea(0, y, image.Width, 1);
			for (int x = 0; x < pcv.Length; x += channelsCount)
			{
				var r = pcv[x];
				var g = pcv[x + 1];
				var b = pcv[x + 2];

				var index = x / channelsCount;
				ref var pixel = ref blurhashPixels[index, y];
				pixel.Red = MathUtils.SRgbToLinear(r);
				pixel.Green = MathUtils.SRgbToLinear(g);
				pixel.Blue = MathUtils.SRgbToLinear(b);
			}
		}
#else
        for (int y = 0; y < image.Height; y++)
        {
            var pcv = pixels.GetArea(0, y, image.Width, 1);
            for (int x = 0; x < pcv.Length; x += channelsCount)
            {
                var r = pcv[x];
                var g = pcv[x + 1];
                var b = pcv[x + 2];

                var index = x / channelsCount;
                ref var pixel = ref blurhashPixels[index, y];
                pixel.Red = MathUtils.SRgbToLinear(r);
                pixel.Green = MathUtils.SRgbToLinear(g);
                pixel.Blue = MathUtils.SRgbToLinear(b);
            }
        }
#endif

        var blurhash = Core.Encode(blurhashPixels, componentsX, componentsY);
        return blurhash;
    }

    /// <summary>
    /// Decodes a Blurhash string into a <c>ImageMagick.MagickImage</c>
    /// </summary>
    /// <param name="blurhash">The blurhash string to decode</param>
    /// <param name="outputWidth">The desired width of the output in pixels</param>
    /// <param name="outputHeight">The desired height of the output in pixels</param>
    /// <param name="punch">A value that affects the contrast of the decoded image. 1 means normal, smaller values will make the effect more subtle, and larger values will make it stronger.</param>
    /// <returns>The decoded preview</returns>
    public static MagickImage Decode(string blurhash, int outputWidth, int outputHeight, double punch = 1.0)
    {
        var pixelData = new Pixel[outputWidth, outputHeight];
        Core.Decode(blurhash, pixelData, punch);

        byte[] outputData = new byte[outputWidth * outputHeight * 3];

        int index = 0;
        for (int y = 0; y < outputHeight; y++)
        {
            for (int x = 0; x < outputWidth; x++)
            {
                var pixel = pixelData[x, y];

                outputData[index++] = (byte)MathUtils.LinearTosRgb(pixel.Red);
                outputData[index++] = (byte)MathUtils.LinearTosRgb(pixel.Green);
                outputData[index++] = (byte)MathUtils.LinearTosRgb(pixel.Blue);
            }
        }

        var image = new MagickImage();
        var settings = new PixelReadSettings((uint)outputWidth, (uint)outputHeight, StorageType.Char, PixelMapping.RGB);
        image.ReadPixels(outputData, settings);
        image.Format = MagickFormat.Png;

        return image;
    }
}
