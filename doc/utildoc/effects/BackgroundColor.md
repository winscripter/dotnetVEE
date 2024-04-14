# BackgroundColor
Applies background color to a part of the video.

Namespace: `dotnetVEE.Computation.Utils.Effects`
Class Name: `BackgroundColor`

# Methods
```cs
public static GeneratedFileNames ApplyPart(
    Video vid,
    StartEndTimestamp timestamps,
    Rgba64 color,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Applies background color to a video between the two timestamps.

- `Video vid`: Input video file
- `StartEndTimestamp timestamps`: Timestamps where this effect will apply
- `Rgba64 color`: Background color (can overload with: `RgbaVector`, `RgbaRF`, `Rgba32`, `Color`).
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.

```cs
public static GeneratedFileNames ApplyPart(
    Video vid,
    StartEndTimestamp timestamps,
    RgbaVector color,
    ref ObservableCollection<float> progress,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Same as the previous `ApplyPart`, except this one supports Progressive Notification. `RgbaVector color` can overload with `Rgba64`, `RgbaRF`, `Rgba32`, `Color`.
