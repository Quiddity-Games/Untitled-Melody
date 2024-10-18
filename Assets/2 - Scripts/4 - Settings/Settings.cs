using Tools.Settings;
public static class Settings
{

    public static IntSetting MetronomeMode = new IntSetting("Metronome Mode", 0, 0, 3);
    public static BoolSetting SecondaryBars = new BoolSetting("Secondary Bars Enabled", false);
    public static BoolSetting MetronomeBlink = new BoolSetting("Metronome Blink Enabled", false);
    public static BoolSetting MetronomeRings = new BoolSetting("Metronome Rings Enabled", false);
    public static BoolSetting Windowed = new BoolSetting("Windowed Mode", false);
    public static BoolSetting ContrastEnabled = new BoolSetting("Enable Contrast", false);
    public static FloatSetting Contrast = new FloatSetting("Contrast Value", 0f, -1f, 1f);
    public static FloatSetting MusicVolume = new FloatSetting("Music Volume", 0.8f, 0.0001f, 1f);
    public static FloatSetting SoundVolume = new FloatSetting("Sound Volume", 0.8f, 0.0001f, 1f);

}