# RectangleMotionUtility
Creates a rectangle that can move from one place to another.

```cs
public class RectangleMotionUtility : IUtility
```

Remarks:
- The rectangle will be hollowed.

### Example
The preceding example demonstrates how to use this utility to create a 64x64 rectangle with 4-pixel red border thickness that starts at 128x128 and ends at 430x440, starting at 00:00:02.000 and ending at 00:00:05.000. Speed is adjusted automatically.

```cs
using dotnetVEE;
using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Computation.Options;
using dotnetVEE.Computation.Options.Common;
using dotnetVEE.Computation.Utils;
using SixLabors.ImageSharp;
using System.Collections.ObjectModel;

Video video = Video.Create("seagulls.mp4");

var progress = new ObservableCollection<float>();

var rectOptions = new RectangleMotionObjectOptions(
    new SizeF(64, 64), // size of the rectangle
    4, // rectangle border thickness
    new RgbaRF(255F, 0F, 0F, 1F)); // rectangle border color - rgba(0xFF, 0x00, 0x00, 0x01)

var start = new Point(128, 128); // initial coordinate
var end = new Point(430, 440); // final coordinate
var starttime = new TimeSpan(0, 0, 0, 2); // initial timestamp
var endtime = new TimeSpan(0, 0, 0, 5); // final timestamp

var options = new RectangleMotionOptions(
    rectOptions,
    new BaseMotionOptions(start, end, starttime, endtime));

var util = new RectangleMotionUtility(options, DeleteGeneratedFiles.FramesDirectoryOnly);
util.Run(video, ref progress);

string? videoFile = util.FileNames?.VideoFileName; // this is the file name of the output video file
```
Please run the program above, and you should see a moving rectangle between 00:00:02.000 and 00:00:05.000.

*Note*: Progressive Notification can also be implemented:
```cs
var progress = new ObservableCollection<float>();
progress.CollectionChanged += (s, e) =>
{
    int lastObj = (int)progress.Last();
    string pipes = new string('|', lastObj);

    if (lastObj > 100)
    {
        return;
    }

    string fmt = $"[{pipes.PadRight(100, ' ')}]";
    Console.WriteLine(fmt);
};
```
