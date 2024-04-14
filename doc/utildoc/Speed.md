# Speed
Speeds up a part of video.

```cs
public class Speed : IUtility
```

Remarks:
- Audio will be removed from the video when speeding up the video. It is recommended that you first extract the audio from the video, then speed the audio up, and then after speeding up the video, add the audio back.

### Example
The preceding example speeds up the video by 3.5 times starting with 00:00:01.500 and ending with 00:00:07.500 (Hours:Minutes:Seconds.Milliseconds):
```cs
using dotnetVEE.Computation.Utils;
using System.Collections.ObjectModel;

Video video = Video.Create("seagulls.mp4");

var none = new ObservableCollection<float>();

Speed speed = new Speed(350F, new StartEndTimestamp(new TimeSpan(0, 0, 0, 1, 500), new TimeSpan(0, 0, 0, 7, 500)));
speed.Run(video, ref none);
```

*Note*: The first argument of the `Speed` constructor is the percentage of how much the video should be sped up. 350F means `350%`, and the default value is 100% (100F).

The output video is generated in a file with a random name. To access it, use the `OutputVideoFilePath` property of the `Speed` utility:
```cs
/// <summary>
/// Path to the output video file, or <see langword="NULL" /> if it's unset.
/// </summary>
public string? OutputVideoFilePath { get; private set; } = null;
```
The value of this property will be `null`, as long as the `Run(Video, ref ObservableCollection<float>)` method was not called.

The property will hold the name of the generated video file, which contains the changes. The original video file will be kept unchanged.
