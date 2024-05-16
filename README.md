# dotnetVEE
Modern &amp; Powerful Cross-Platform Managed Video Editing library for .NET that operates on FFmpeg/FFprobe; stands for .NET Video Editing Engine.

# Available on NuGet!
It is called [`Winscripter.VideoEditingTools.dotnetVEE`](https://www.nuget.org/packages/Winscripter.VideoEditingTools.dotnetVEE). Yes, a bit of a detailed name, but I made it because I felt like it (searching up `dotnetVEE` should yield this result):
- it was developed by me
- it is a video editing tool
- it is called `dotnetVEE`.

# What can it do?
More than what I am going to describe!
- A wide variety of effects, *including* Glitch effect
- A way to create a custom effect or utility
- A way to manage a part of the video between two timestamps: start &amp; end
- Audio management
- Video/Audio information query
- Video to GIF conversion
- Extracting audio and frames from a video
- Direct processing on video metadata
- Easy to use FFmpeg/FFprobe invocation, async invocation, and output capture
- Zooming in/out of a video; adding text; adding moving text; adding rectangle; adding moving rectangle
- Progressive Notification to keep track of the effect/utility/task progress

# Compatibility
dotnetVEE runs on .NET 6 and later runtimes.

However, dotnetVEE automatically looks for *ffmpeg.exe* and *ffprobe.exe* on Windows, and *ffmpeg* and *ffprobe* on anything
except Windows. This ensures compatibility. If you're releasing an application that uses dotnetVEE but is compatible with
multiple operating systems, it's important to include ffmpeg.exe/ffprobe.exe for a Windows release and ffmpeg/ffprobe for
macOS or Linux release, and especially the version of ffmpeg for these systems. File naming plays an important role at locating
ffmpeg/ffprobe.

If you're releasing a Class Library that uses dotnetVEE, like a wrapper or extension, then including ffmpeg/ffprobe is
*optional*. Well, including all of them for Windows, macOS, and Linux will probably yield the size of around 500MB, which
is quite a lot.

# Documentation
Documentation can be found in the `doc` folder in the root of the GitHub repository.

# Progressive Notification
A great feature of `dotnetVEE` is Progressive Notification, which is a way you pass `ObservableCollection`
to a utility, and it will add values representing the percentage (how much data is processed) of the progress
until it hits 100%. This can help you build, for example, progress bars when using dotnetVEE in WPF, which is
essential when it comes to creating video editors.

Documentation for Progressive Notification can be found [here](https://github.com/winscripter/dotnetVEE/blob/main/doc/6.%20Progressive%20Notification.md).

# Examples
Obtaining information about a video:
```cs
using dotnetVEE;

Video video = Video.Load("adorable_cats.mp4");
Console.WriteLine($"FPS: {video.RoundedFPS}"); // 60
Console.WriteLine($"Total Frames: {video.FrameCount}"); // 3917
Console.WriteLine($"Resolution: {video.Resolution.X}x{video.Resolution.Y}"); // 1920x1080
```

Changing audio in a video between 2 timestamps (30%, default is 100%):
```cs
using dotnetVEE;
using dotnetVEE.Abstractions;
using dotnetVEE.Computation.Audio;

Video vid = Video.Create("seagulls.mp4");

AudioManager.AlterVolume(
    vid,
    new StartEndTimestamp(new TimeSpan(0, 0, 0, 1), new TimeSpan(0, 0, 0, 6)),
    new Volume(30F),
    "seagulls-silentpart.mp4");
```

Extracting audio from a video:
```cs
using dotnetVEE;
using dotnetVEE.Computation.Audio;

Video vid = Video.Create("seagulls.mp4");

AudioKind? kind = AudioManager.AutomatedExtractAudio(vid, "audio.mp3");
if (kind is null)
{
    Console.WriteLine("The audio format of the video is unsupported.");
}
else
{
    Console.WriteLine($"The audio type is {kind?.ToString()}");
}
```

Glitch effect:
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

Tweaking video speed by 3.5 times between two timestamps:
```cs
using dotnetVEE.Computation.Utils;
using System.Collections.ObjectModel;

Video video = Video.Create("seagulls.mp4");

var none = new ObservableCollection<float>();

Speed speed = new Speed(350F, new StartEndTimestamp(new TimeSpan(0, 0, 0, 1, 500), new TimeSpan(0, 0, 0, 7, 500)));
speed.Run(video, ref none);
```

Adding text to video:
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
