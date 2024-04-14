# AdaptiveThreshold
Applies adaptive threshold to a part of video.

Namespace: `dotnetVEE.Computation.Utils.Effects`
Class Name: `AdaptiveThreshold`

# Methods
```cs
public static GeneratedFileNames AnimatePartEasy(
    Video vid,
    StartEndTimestamp timestamps,
    StartEndTimestamp part,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Applies static adaptive threshold between two timestamps with the default threshold limit.
- `Video vid`: Input video file
- `StartEndTimestamp timestamps`: Timestamps where every effect should apply
- `StartEndTimestamp part`: Timestamps where this effect will apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.

```cs
public static GeneratedFileNames AnimatePart(
    Video vid,
    float thresholdLimit,
    StartEndTimestamp timestamps,
    StartEndTimestamp part,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Applies static adaptive threshold between two timestamps using a custom threshold limit.
- `Video vid`: Input video file
- `float thresholdLimit`: The threshold limit for the effect.
- `StartEndTimestamp timestamps`: Timestamps where every effect should apply
- `StartEndTimestamp part`: Timestamps where this effect will apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.

```cs
public static GeneratedFileNames AnimatePartEasy(
    Video vid,
    StartEndTimestamp timestamps,
    StartEndTimestamp part,
    ref ObservableCollection<float> progress,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
***A variant of `AnimatePartEasy(Video, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)` that supports progressive notification.***

```cs
public static GeneratedFileNames AnimatePart(
    Video vid,
    float thresholdLimit,
    StartEndTimestamp timestamps,
    StartEndTimestamp part,
    ref ObservableCollection<float> progress,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
***A variant of `AnimatePartEasy(Video, float, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)` that supports progressive notification.***
