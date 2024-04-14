# BokehBlur
Applies Bokeh Blur effect to a video.

Namespace: `dotnetVEE.Computation.Utils.Effects`
Class Name: `BokehBlur`

# Methods
```cs
public static GeneratedFileNames ApplyToPart(
    Video vid,
    StartEndTimestamp timestamp,
    BokehBlurOptions options,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Applies static Bokeh Blur effect to a part of the video.

- `Video vid`: Input video file.
- `StartEndTimestamp timestamp`: Timestamps when this effect starts.
- `BokehBlurOptions options`: Options for this effect.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.

```cs
public static GeneratedFileNames ApplyToPart(
    Video vid,
    StartEndTimestamp timestamp,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Applies static Bokeh Blur effect to a part of the video using default settings for the blurring effect.

- `Video vid`: Input video file.
- `StartEndTimestamp timestamp`: Timestamps when this effect starts.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.
