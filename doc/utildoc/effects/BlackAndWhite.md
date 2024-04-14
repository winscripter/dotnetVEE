# BlackAndWhite
Makes a part of the video consist of black and white pixels only.

Namespace: `dotnetVEE.Computation.Utils.Effects`
Class Name: `BlackAndWhite`

# Methods
```cs
public static GeneratedFileNames ApplyToPart(
    Video vid,
    StartEndTimestamp timestamp,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Applies black and white effect to a part of video.

- `Video vid`: Input video file.
- `StartEndTimestamp timestamp`: Timestamps where this effect will apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.
