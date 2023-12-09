namespace DataModels;

public record AppDimensions(
    int MinHeight,
    int MaxHeight,
    int MinWidth,
    int MaxWidth,
    int DefaultWidth,
    int DefaultHeight
);