/*
Copyright (C) 2016-present Alexander Lunsford

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;

namespace Mors_Arcium
{
    public class AudioSystem
    {
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        private static readonly float MAX_MUSIC_VOLUME = 0.5f;
        private Dictionary<string, Song> songs = new Dictionary<string, Song>();
        private Song currentSong;
        private Song nextSong;
        private float musicVolume = MAX_MUSIC_VOLUME;

        private IPlatformOutlet platform;

        /// <summary>
        /// Sounds are attenuated based off of their distances to this position.
        /// Must be continually set to the center of the screen in world-space.
        /// </summary>
        public Vector2 ListenerPosition { get; set; }

        public AudioSystem(IPlatformOutlet platform)
        {
            this.platform = platform;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = MAX_MUSIC_VOLUME;
        }

        public void LoadContent(ContentManager content)
        {
            DirectoryInfo soundDir = new DirectoryInfo(content.RootDirectory + "/sounds");
            foreach (FileInfo file in soundDir.EnumerateFiles())
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (!sounds.ContainsKey(name)) sounds.Add(name, content.Load<SoundEffect>("sounds/" + name));
                Console.WriteLine("Sound Loaded: " + file.Name);
            }

            DirectoryInfo musicDir = new DirectoryInfo(content.RootDirectory + "/music");
            foreach (FileInfo file in musicDir.EnumerateFiles())
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (!songs.ContainsKey(name)) songs.Add(name, content.Load<Song>("music/" + name));
                Console.WriteLine("Song Loaded: " + file.Name);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (nextSong != currentSong)
            {
                musicVolume -= 0.01f;
                if (musicVolume <= 0.0f)
                {
                    musicVolume = MAX_MUSIC_VOLUME;
                    if (currentSong != null)
                    {
                        MediaPlayer.Stop();
                    }
                    currentSong = nextSong;
                    if (currentSong != null)
                    {
                        MediaPlayer.Play(currentSong);
                    }
                }
            }
            MediaPlayer.Volume = platform.GameSettings.musicEnabled ? musicVolume : 0.0f;
        }

        /// <summary>
        /// Begins transition from one music track to another.
        /// </summary>
        /// <param name="songName">Filename (without extension) of song. Use blank string to stop playing music.</param>
        /// <param name="instantly">If true, the music will change instantly without fading out the previous track.</param>
        public void ChangeMusic(string songName, bool instantly = false)
        {
            if (songName.Equals(""))
            {
                nextSong = null;
            }
            else
            {
                nextSong = songs[songName];
            }
            if (instantly)
            {
                musicVolume = 0.0f;
            }
        }

        /// <summary>
        /// Plays a sound that gets quieter when it is off-screen.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="sourcePosition">Position of the object emitting the sound.</param>
        public void Play3DSound(string soundName, Vector2 sourcePosition)
        {
            if (platform.GameSettings.soundEnabled)
            {
                float distance = (sourcePosition - ListenerPosition).Length();
                float attenuation = 1.0f - (Math.Max(0.0f, distance - 160.0f) / 80.0f);
                if (attenuation > 0.0f) Play2DSound(soundName, attenuation);
            }
        }

        /// <summary>
        /// Plays a sound with a set volume.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="volume"></param>
        public void Play2DSound(string soundName, float volume = 1.0f)
        {
            if (platform.GameSettings.soundEnabled)
            {
                sounds[soundName].Play(volume, 0.0f, 0.0f);
            }
        }
    }
}
