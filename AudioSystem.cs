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
    class AudioSystem
    {
        /*public struct Sound
        {
            public SoundEffect soundEffect;
            public SoundEffectInstance[] instances;
            public int nextInstance;
            public Sound(SoundEffect effect, int numInsts)
            {
                nextInstance = 0;
                soundEffect = effect;
                instances = new SoundEffectInstance[numInsts];
                for (int i = 0; i < instances.Length; ++i)
                {
                    instances[i] = effect.CreateInstance();
                }
            }
        }*/

        private static MorsArcium game;
        private static ContentManager content;
        private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        private static readonly float MAX_MUSIC_VOLUME = 0.5f;
        private static Dictionary<string, Song> songs = new Dictionary<string, Song>();
        private static Song currentSong;
        private static Song nextSong;
        private static float musicVolume = MAX_MUSIC_VOLUME;

        public static Vector2 ListenerPosition { get; set; }

        public static void Initialize(MorsArcium g)
        {
            game = g;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = MAX_MUSIC_VOLUME;
        }

        public static void LoadContent(MorsArcium g)
        {
            game = g;

            content = new ContentManager(game.Services, "Content");

            DirectoryInfo soundDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "/Content/sounds");
            foreach (FileInfo file in soundDir.EnumerateFiles())
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (!sounds.ContainsKey(name)) sounds.Add(name, content.Load<SoundEffect>("sounds/" + name));
                Console.WriteLine("Sound Loaded: " + file.Name);
            }

            DirectoryInfo musicDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "/Content/music");
            foreach (FileInfo file in musicDir.EnumerateFiles())
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (!songs.ContainsKey(name)) songs.Add(name, content.Load<Song>("music/" + name));
                Console.WriteLine("Song Loaded: " + file.Name);
            }
        }

        public static void UnloadContent()
        {
            content.Unload();
        }

        public static void Update(GameTime gameTime)
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
            MediaPlayer.Volume = game.musicEnabled ? musicVolume : 0.0f;
        }

        /// <summary>
        /// Begins transition from one music track to another.
        /// </summary>
        /// <param name="songName">Filename (without extension) of song. Use blank string to stop playing music.</param>
        /// <param name="instantly">If true, the music will change instantly without fading out the previous track.</param>
        public static void ChangeMusic(string songName, bool instantly = false)
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

        public static void Play3DSound(string soundName, Vector2 sourcePosition)
        {
            if (game.soundEnabled)
            {
                float distance = (sourcePosition - ListenerPosition).Length();
                float attenuation = 1.0f - (Math.Max(0.0f, distance - 160.0f) / 80.0f);
                if (attenuation > 0.0f) Play2DSound(soundName, attenuation);
            }
        }
        public static void Play2DSound(string soundName, float volume = 1.0f)
        {
            if (game.soundEnabled)
            {
                sounds[soundName].Play(volume, 0.0f, 0.0f);
            }
        }
    }
}
