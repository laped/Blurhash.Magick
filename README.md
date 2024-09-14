# Blurhash.Magick
Blurhash implementation for Magick.NET, based on [Blurhash.Core](https://github.com/MarkusPalcer/blurhash.net)

## Installation
```bash
dotnet add package Blurhash.Magick
```

## Usage

### Encode - Get a blurhash string from an MagickImage
```csharp
using Blurhash.Magick;

using (var image = new MagickImage("input.jpg"))
{
	var blurhash = Blurhasher.Encode(sourceImage, 9, 9);
}
```

### Decode - Turn a blurhash string into an MagickImage
```csharp	
using Blurhash.Magick;

string sourceHash = "|20Ktja2e-eSe8g2e9gNene,dCg$gOf8gieTf+eTgNd=g$eSeSgOeng3f7heeng4e.eTfie.g3e.Z#d;g4gOg4f7e.f6f,gigNf+eSeSe-g3emg3g3e-eTgOgNf+g3eTf+eTf+emf+e.eTgNeng3e.gOe:fif5f6g3eTf+";

var result = Blurhasher.Decode(sourceHash, 200, 200);
```