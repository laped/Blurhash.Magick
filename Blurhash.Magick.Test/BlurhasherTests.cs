using FluentAssertions;
using ImageMagick;

namespace Blurhash.Magick.Test;
public class BlurhasherTests
{
    private const string SourceHash = "|20Ktja2e-eSe8g2e9gNene,dCg$gOf8gieTf+eTgNd=g$eSeSgOeng3f7heeng4e.eTfie.g3e.Z#d;g4gOg4f7e.f6f,gigNf+eSeSe-g3emg3g3e-eTgOgNf+g3eTf+eTf+emf+e.eTgNeng3e.gOe:fif5f6g3eTf+";

    [Fact]
    public void DecodingTests()
    {
        var result = Blurhasher.Decode(SourceHash, 200, 200);

        var sourceImage = new MagickImage(Path.Combine(AppContext.BaseDirectory, "Resources", "Expectations", "BlurResult1.png"));
        result.Signature.Should().Be(sourceImage.Signature);
    }

    [Fact]
    public void EncodingTests()
    {
        var sourceImage = new MagickImage(Path.Combine(AppContext.BaseDirectory, "Resources", "Specimens", "Sample.png"));

        var result = Blurhasher.Encode(sourceImage, 9, 9);

        result.Should().Be(SourceHash);
    }
}
