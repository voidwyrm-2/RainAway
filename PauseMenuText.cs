using Menu;
using UnityEngine;

namespace NuclearPasta.RainAway
{
	public static class PauseMenuText
	{
        //copied from https://github.com/SabreML/MusicAnnouncements/blob/master/src/PauseMenuText.cs

        // The 'Currently Playing: x' text in the top right corner.
        private static MenuLabel currentlyPlayingLabel;

		public static void SetupHooks()
		{
			On.Menu.PauseMenu.ctor += PauseMenuHK;
			On.Menu.PauseMenu.ShutDownProcess += PauseMenu_ShutDownProcessHK;
			//On.HUD.TextPrompt.Draw += TextPrompt_DrawHK;
		}

		// Called when the pause menu is opened.
		// this adds a label in the top right corner of the screen displaying its name.
		private static void PauseMenuHK(On.Menu.PauseMenu.orig_ctor orig, PauseMenu self, ProcessManager manager, RainWorldGame game)
		{
			orig(self, manager, game);

			float posX = game.rainWorld.options.ScreenSize.x - 10.01f;
			float posY = game.rainWorld.options.ScreenSize.y - 13f;

			currentlyPlayingLabel = new MenuLabel(self, self.pages[0], $"Rain Away is currently {Plugin.status}", new Vector2(posX, posY), Vector2.zero, true);
			currentlyPlayingLabel.label.color = Menu.Menu.MenuRGB(Menu.Menu.MenuColors.MediumGrey);
			currentlyPlayingLabel.label.alignment = FLabelAlignment.Right; // Align on the right so that longer song names don't go off the screen.
			currentlyPlayingLabel.label.alpha = 0f; // Start out invisible in order to fade in.

			self.pages[0].subObjects.Add(currentlyPlayingLabel);
			Debug.Log($"(RainAway) Rain Away is currently {Plugin.status}");
		}

		private static void PauseMenu_ShutDownProcessHK(On.Menu.PauseMenu.orig_ShutDownProcess orig, PauseMenu self)
		{
			if (currentlyPlayingLabel != null)
			{
				RemoveCurrentlyPlayingText(); // Called before `orig()` so that it doesn't waste time trying to remove itself twice.
			}
			orig(self);
		}

		/*
		// Called when the "Paused" text at the bottom left of the screen updates its appearance. (Around every frame)
		// This is used to steal its alpha value and fade in along with it, or to smoothly fade out if the song ends.
		private static void TextPrompt_DrawHK(On.HUD.TextPrompt.orig_Draw orig, HUD.TextPrompt self, float timeStacker)
		{
			orig(self, timeStacker);
			if (currentlyPlayingLabel == null) // No text to modify.
			{
				return;
			}

			float newAlpha;
			if (MusicAnnouncementsMod.SongToAnnounce != null)
			{
				newAlpha = self.label.alpha; // Fade in.
			}
			else // If the text exists but there's no song, then it ended while the menu was open.
			{
				newAlpha = RWCustom.Custom.LerpAndTick(currentlyPlayingLabel.label.alpha, 0f, 0.02f, 0.02f); // Fade out.
				if (newAlpha <= 0f)
				{
					Debug.Log("(MusicAnnouncements) Song ended, removing menu text");
					RemoveCurrentlyPlayingText();
					return;
				}
			}

			currentlyPlayingLabel.label.alpha = newAlpha;
			labelMusicSprite.alpha = newAlpha;
		}
		*/

		// Clean up our variables.
		private static void RemoveCurrentlyPlayingText()
		{
			currentlyPlayingLabel.RemoveSprites();
			currentlyPlayingLabel.owner.RemoveSubObject(currentlyPlayingLabel);
			currentlyPlayingLabel = null;
		}
	}
}
