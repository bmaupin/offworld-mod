Abandoned attempt to mod Offworld Trading Company game music. It works but:

- It hangs the game for about 20 seconds every time the mod is loaded and unloaded
  - Probably due to loading the asset file, which is ~400 MB
  - This can happen with some frequency; e.g. when starting a daily/infinite challenge map, then again when finishing it
- Mods are disabled for Daily Challenge and Infinite Map Challenge so it's only really good for campaign or skirmish

The easier option is just to mute the in-game music (and menu music) and set something like this for the game's launch options in Steam (this also bypasses the launcher):

```
(mpv --no-audio-display --shuffle ~/Music/"Some Other Music") & eval $(echo "%command%" | sed "s/-multidisplay/\/nolauncher/") && killall mpv
```

Or to play the main theme once and then other music after:

```
(sleep 8; mpv --no-audio-display ~/Music/"Offworld Trading Company/01 Red Planet Nocturne.mp3"; mpv --no-audio-display --shuffle ~/Music/"Some Other Music") & eval $(echo "%command%" | sed "s/-multidisplay/\/nolauncher/") && killall mpv
```
