# AddText
Adds static text to a video between the given two timestamps.

```cs
public class AddText : IUtility
{
    // ...
}
```

### Example
The example below adds red text at 128x36 titled "Hello, World!" with the Space Grotesk as a font which is 30 pixels in size, and appears at 00:00:01.500 and vanishes at 00:00:03.000 (hours:minutes:seconds.milliseconds). The example also includes progressive notification.

```cs
using dotnetVEE;
using dotnetVEE.Abstractions;
using dotnetVEE.Computation.Options;
using dotnetVEE.Computation.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

Video video = Video.Create("seagulls.mp4");

Font font = new FontCollection()
                .Add("SpaceGrotesk.ttf") // Add font file SpaceGrotesk.ttf- does not require the font to be installed
                .CreateFont(30F); // Make its size 30 pixels

var options = new TextComputationOptions(
    "Hello, World!",
    new Rgba32(0xFF, 0x00, 0x00) /* red RGB*/,
    new Point(128, 36),
    font);

var timestamp = new StartEndTimestamp(
    new TimeSpan(0, 0, 0, 1, 500),
    new TimeSpan(0, 0, 0, 3)); // the text will appear at 00:00:01.500 and will vanish at 00:00:03.000

bool leaveGeneratedFile = true; // set this to true if you don't want the original video to be overwrtiten.

IUtility util = new AddText(options, timestamp, leaveGeneratedFile);

var collection = new ObservableCollection<float>();
collection.CollectionChanged += (s, e) =>
{
    int lastObj = (int)collection.LastOrDefault();
    lastObj = lastObj > 100 ? 100 : lastObj;

    string contents = new string('|', lastObj).PadRight(100, ' ');
    Console.WriteLine($"[{contents}]");
};
util.Run(video, ref collection);
```
