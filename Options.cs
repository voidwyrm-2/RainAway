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

    public Options()
    {
        EnableStatusLabel = config.Bind("nc.cfgEnableStatusLabel", true);
        StatusLabelX = config.Bind("nc.cfgStatusLabelX", 20.01f, new ConfigAcceptableRange<float>(0, 1000));
        StatusLabelY = config.Bind("nc.cfgStatusLabelY", 45f, new ConfigAcceptableRange<float>(0, 1000));
    }

    public override void Initialize()
    {
        base.Initialize();

        Tabs = new OpTab[] { new OpTab(this) };

        var labelTitle = new OpLabel(20, 600 - 30, "Rain Away! Options", true);

        var top = 200;
        var labelDmgMul = new OpLabel(new(20, 600 - top), Vector2.zero, "Status Label Enabled?", FLabelAlignment.Left);
        var draggerDmgMul = new OpCheckBox(EnableStatusLabel, new Vector2(180, 600 - top - 6)) {
            description = "Turns on or off the \"Rain Away is currently enable/disabled\" message on the pause menu",
        };

        var labelDmgRegen = new OpLabel(new(20, 600 - top - 40), Vector2.zero, "Recovery cooldown", FLabelAlignment.Left);
        var labelSLX = new OpLabel(new(516, 600 - top - 40), Vector2.zero, "position of the label on the horizontal", FLabelAlignment.Left);
        var floatsliderSLX = new OpSlider(StatusLabelX, new Vector2(180, 600 - top - 46), 320) {
            description = "After this delay, you rapidly recover from damage. If set to -1, damage is only reset after sleeping.",
        };

        Tabs[0].AddItems(
            labelTitle,
            labelDmgMul,
            draggerDmgMul,
            labelDmgRegen,
            labelSLX,
            floatsliderSLX
        );
    }
}
