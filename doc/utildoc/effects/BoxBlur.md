# BoxBlur
Applies box blur effect to a part of the video.

Namespace: `dotnetVEE.Computation.Utils.Effects`
Class Name: `BoxBlur`

# Methods
```cs
public static GeneratedFileNames ApplyPart(
    Video vid,
    int radius,
    StartEndTimestamp timestamp,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Applies Box Blur effect to a part of the video with the given radius.

- `Video vid`: Input video file.
- `int radius`: Radius for the blurring effect.
- `StartEndTimestamp timestamp`: Timestamps when this effect starts to apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.

```cs
public static GeneratedFileNames ApplyPart(
    Video vid,
    StartEndTimestamp timestamp,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Alias to previous method, except the radius is automatically set to the default value 7.

```cs
public static GeneratedFileNames ApplyAscending(
    Video vid,
    StartEndTimestamp timestamp,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Creates an effect where the blurring effect starts with 0% and slowly ascends until it hits 100%. Speed is automatically adjusted.

- `Video vid`: Input video file.
- `StartEndTimestamp timestamp`: Timestamps when this effect starts to apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.

```cs
public static GeneratedFileNames ApplyDescending(
    Video vid,
    StartEndTimestamp timestamp,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Creates an effect where the blurring effect starts with 100% and slowly descends until it hits 0%. Speed is automatically adjusted.

- `Video vid`: Input video file.
- `StartEndTimestamp timestamp`: Timestamps when this effect starts to apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.
