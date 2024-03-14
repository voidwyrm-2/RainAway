using Menu;
using UnityEngine;

namespace NuclearPasta.RainAway
{
	public static class PauseMenuText
	{
        //taken from https://github.com/SabreML/MusicAnnouncements/blob/master/src/PauseMenuText.cs

        // The 'Rain Away is currently enabled/disabled' text in the top right corner.
        private static MenuLabel RainAwayStateLabel;

		public static void SetupHooks()
		{
			On.Menu.PauseMenu.ctor += PauseMenuHK;
			On.Menu.PauseMenu.ShutDownProcess += PauseMenu_ShutDownProcessHK;
			On.HUD.TextPrompt.Draw += TextPrompt_DrawHK;
		}

		// Called when the pause menu is opened.
		// this adds a label in the top right corner of the screen displaying its name.
		private static void PauseMenuHK(On.Menu.PauseMenu.orig_ctor orig, PauseMenu self, ProcessManager manager, RainWorldGame game)
		{
			orig(self, manager, game);

			float posX = game.rainWorld.options.ScreenSize.x - Options.StatusLabelX.Value;
			float posY = game.rainWorld.options.ScreenSize.y - Options.StatusLabelY.Value;

			RainAwayStateLabel = new MenuLabel(self, self.pages[0], $"Rain Away is currently {Plugin.status}", new Vector2(posX, posY), Vector2.zero, true);
			RainAwayStateLabel.label.color = Menu.Menu.MenuRGB(Menu.Menu.MenuColors.MediumGrey);
			RainAwayStateLabel.label.alignment = FLabelAlignment.Right; // Align on the right so that longer song names don't go off the screen.
			RainAwayStateLabel.label.alpha = 0f; // Start out invisible in order to fade in.
            RainAwayStateLabel.label.scale = Options.StatusLabelScale.Value;


            self.pages[0].subObjects.Add(RainAwayStateLabel);
			Debug.Log($"(RainAway) Rain Away is currently {Plugin.status}");
		}

		private static void PauseMenu_ShutDownProcessHK(On.Menu.PauseMenu.orig_ShutDownProcess orig, PauseMenu self)
		{
			if (RainAwayStateLabel != null)
			{
				RemoveCurrentlyPlayingText(); // Called before `orig()` so that it doesn't waste time trying to remove itself twice.
			}
			orig(self);
		}

		// Called when the "Paused" text at the bottom left of the screen updates its appearance. (Around every frame)
		// This is used to steal its alpha value and fade in along with it, or to smoothly fade out if the song ends.
		private static void TextPrompt_DrawHK(On.HUD.TextPrompt.orig_Draw orig, HUD.TextPrompt self, float timeStacker)
		{
			orig(self, timeStacker);
			if (RainAwayStateLabel == null) // No text to modify.
			{
				return;
			}

			float newAlpha;
			newAlpha = self.label.alpha; // Fade in.

			RainAwayStateLabel.label.alpha = newAlpha;
		}

		// Clean up our variables.
		private static void RemoveCurrentlyPlayingText()
		{
			RainAwayStateLabel.RemoveSprites();
			RainAwayStateLabel.owner.RemoveSubObject(RainAwayStateLabel);
			RainAwayStateLabel = null;
		}
	}
}
