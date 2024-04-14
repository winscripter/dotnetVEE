# GlitchEffect
Adds a cool-looking glitch effect to a part of video that can be configured.

```cs
public class GlitchEffect : IUtility
```

### Example
The following example uses this utility to create a glitch effect inside of a video between 00:00:01.500 and 00:00:03.000 (Hours:Minutes:Seconds.Milliseconds):

```cs
using dotnetVEE;
using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Computation.Utils;
using System.Collections.ObjectModel;

Video video = Video.Create("seagulls.mp4");

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

var config = new GlitchConfiguration(
    new StartEndTimestamp(new TimeSpan(0, 0, 0, 1, 500), new TimeSpan(0, 0, 0, 3)),
    0.05F,
    -20,
    20);

var eff = new GlitchEffect(config, DeleteGeneratedFiles.None);
eff.Run(video, ref progress);
```

You can tweak `GlitchConfiguration`'s 2nd, 3rd, and 4th parameters to produce different and more interesting results. The effect also produces random results as you apply it on a video again.
