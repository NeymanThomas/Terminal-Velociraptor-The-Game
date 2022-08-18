/// <summary>
/// Stores the few user preferences the application makes use of as a json
/// file. The class itself holds properties for volume, quality, isFullscreen,
/// and a resolution index. These properties can be saved as a json file in
/// order to reload the same properties each time the application is started.
/// </summary>

public class UserPreferences
{
    public float volume; 
    public int quality;
    public bool isFullscreen;
    public int resolutionIndex;

    public UserPreferences() 
    {
        volume = 0;
        quality = 0;
        isFullscreen = true;
        resolutionIndex = 0;
    }

    public UserPreferences(float vol, int q, bool full, int reso) 
    {
        volume = vol;
        quality = q;
        isFullscreen = full;
        resolutionIndex = reso;
    }
}
