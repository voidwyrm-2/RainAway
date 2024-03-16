using Menu.Remix.MixedUI;
using UnityEngine;

namespace NuclearPasta.RainAway;

sealed class Options : OptionInterface
{
    //taken from https://github.com/Dual-Iron/no-damage-rng/blob/master/src/Plugin.cs
    //thanks dual, you're a life saver

    public static Configurable<bool> EnableStatusLabel;
    public static Configurable<float> StatusLabelX;
    public static Configurable<float> StatusLabelY;
    public static Configurable<float> StatusLabelScale;

    public Options()
    {
        EnableStatusLabel = config.Bind("nc_cfgEnableStatusLabel", true);
        StatusLabelX = config.Bind("nc_cfgStatusLabelX", 20.01f, new ConfigAcceptableRange<float>(0, 1000));
        StatusLabelY = config.Bind("nc_cfgStatusLabelY", 45f, new ConfigAcceptableRange<float>(0, 1000));
        StatusLabelScale = config.Bind("nc_cfgStatusLabelScale", 0.75f, new ConfigAcceptableRange<float>(0, 1));
    }

    public override void Initialize()
    {
        base.Initialize();

        Tabs = new OpTab[] { new OpTab(this) };

        var labelTitle = new OpLabel(20, 600 - 30, "Rain Away! Options", true);

        var top = 200;
        var labelSTE = new OpLabel(new(20, 600 - top), Vector2.zero, "Status label enabled?", FLabelAlignment.Left);
        var checkEST = new OpCheckBox(EnableStatusLabel, new Vector2(180, 600 - top - 6)) {
            description = "Turns on or off the \"Rain Away is currently enable/disabled\" message on the pause menu",
        };

        var labelSLX = new OpLabel(new(20, 600 - top - 40), Vector2.zero, "Status label X position", FLabelAlignment.Left);
        var floatsliderSLX = new OpFloatSlider(StatusLabelX, new Vector2(180, 600 - top - 46), 320) {
            description = "Position of the label on the horizontal",
        };

        var labelSLY = new OpLabel(new(20, 600 - top - 80), Vector2.zero, "Status label Y position", FLabelAlignment.Left);
        var floatsliderSLY = new OpFloatSlider(StatusLabelY, new Vector2(180, 600 - top - 86), 320)
        {
            description = "Position of the label on the vertical",
        };

        var labelSLS = new OpLabel(new(20, 600 - top - 120), Vector2.zero, "Status label scale factor", FLabelAlignment.Left);
        var floatsliderSLS = new OpFloatSlider(StatusLabelScale, new Vector2(180, 600 - top - 126), 320)
        {
            description = "Scale of the label",
        };

        Tabs[0].AddItems(
            labelTitle, // Title, simple as that
            labelSTE, // Status Label Enabled
            checkEST, // EnablecStatuscLabel
            labelSLX, // Status Label X
            floatsliderSLX, // Status Label X
            labelSLY, // Status Label Y
            floatsliderSLY, // Status Label Y
            labelSLS, // Status Label Scale
            floatsliderSLS // Status Label Scale
        );
    }
}
