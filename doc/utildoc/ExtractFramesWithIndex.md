# ExtractFramesWithIndex
Extracts all frames between the given start &amp; end indexes from a video.

```cs
public class ExtractFramesWithIndex : IUtility
{
    // ...
}
```

Remarks:
- Frames are saved in the output directory and are named by frame_xxxxxxx.png, where x is a digit - the x is a number, padded with up to 6 zeroes to the left, and they ascend. For example, frame_0000000.png, frame_0000001.png, frame_0000002.png, etc. FFmpeg can recognize this pattern using `outputDirectoryName/frame_%07d.png`.
- Frame indexes start with 0. For example, to specify 72, use 71 instead.

### Example
The preceding example extracts all frames starting with frame 340 all the way to frame 630 and saves them into a folder named `output`:
```cs
using dotnetVEE;
using dotnetVEE.Computation.Utils;
using System.Collections.ObjectModel;

var video = Video.Create("seagull.mp4");
var empty = new ObservableCollection<float>();
new ExtractFramesAtIndex(339, 629, "output")
    .Run(video, ref empty);
```
