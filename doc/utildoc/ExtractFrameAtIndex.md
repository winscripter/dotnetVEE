# ExtractFrameAtIndex
Extracts a single frame (image) from a video at a given index.
```cs
public class ExtractFrameAtIndex : IUtility
{
    // ...
}
```

Remarks:
- Index starts with 0. For example, to extract the 54th frame, use index 53 instead.
- Progressive notification is mostly redundant for this utility, because it is very fast and only uses 0%/100% as possible values.

### Example
The example below extracts the 29th frame from a video and saves the frame as `frame.png`:

```cs
using dotnetVEE;
using dotnetVEE.Computation.Utils;
using System.Collections.ObjectModel;

var video = Video.Create("seagulls.mp4");
var empty = new ObservableCollection<float>();
new ExtractFrameAtIndex(28, "frame.png").Run(video, ref empty);
```
