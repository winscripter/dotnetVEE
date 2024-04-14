# BinaryThreshold
Applies the Binary Threshold effect to a part of the video.

Namespace: `dotnetVEE.Computation.Utils.Effects`
Class Name: `BinaryThreshold`

# Methods
```cs
public static GeneratedFileNames AnimatePart(
    Video vid,
    BinaryThresholdOptions options,
    StartEndTimestamp timestamps,
    StartEndTimestamp part,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Adds static Binary Threshold effect to a part of video.

- `Video vid`: Input video file.
- `BinaryThresholdOptions options`: Options for this effect.
- `StartEndTimestamp timestamps`: Timestamps where every effect starts to apply.
- `StartEndTimestamp part`: Timestamps where this effect will apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleaning task.

```cs
public static GeneratedFileNames AnimateAscending(
    Video vid,
    BinaryThresholdAnimatableOptions options,
    StartEndTimestamp timestamps,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Creates an animation where the binary threshold starts with 0, and slowly ascends until it hits 1. Speed is automatically adjusted.

- `Video vid`: Input video file
- `BinaryThresholdAnimatableOptions options`: Options for this animation
- `StartEndTimestamp timestamps`: Timestamps where this effect will apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleanup task.

```cs
public static GeneratedFileNames AnimateDescending(
    Video vid,
    BinaryThresholdAnimatableOptions options,
    StartEndTimestamp timestamps,
    DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
```
Creates an animation where the binary threshold starts with 1, and slowly descends until it hits 0. Speed is automatically adjusted.

- `Video vid`: Input video file
- `BinaryThresholdAnimatableOptions options`: Options for this animation
- `StartEndTimestamp timestamps`: Timestamps where this effect will apply.
- `DeleteGeneratedFiles cleanupMode`: Settings for the cleanup task.
