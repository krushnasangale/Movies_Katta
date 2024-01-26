namespace MoviesKatta.ViewControls;

public partial class VideoCell
{
    
    public static readonly BindableProperty ParentContextProperty = BindableProperty.Create(
        "ParentContext",
        typeof(object),
        typeof(VideoCell),
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            if (newValue is not null && bindableObject is VideoCell cell && newValue != oldValue)
            {
                cell.ParentContext = newValue;
            }
        });

    public object ParentContext
    {
        get { return GetValue(ParentContextProperty); }
        set { SetValue(ParentContextProperty, value); }
    }
    public VideoCell()
    {
        InitializeComponent();
    }
}