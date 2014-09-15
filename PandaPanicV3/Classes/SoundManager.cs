using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace PandaPanicV3
{
    class SoundManager
    {
        Dictionary<string, SoundEffect> sounds;

        SoundManager(ref ContentManager Content)
        {
            SoundEffect.MasterVolume = 0.1f;

            Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

            sounds.Add("death", Content.Load<SoundEffect>("Music\\death_converted"));
            sounds.Add("victory", Content.Load<SoundEffect>("Music\\victory_converted"));
            sounds.Add("thrust", Content.Load<SoundEffect>("Music\\thrust_converted"));
            sounds.Add("stab", Content.Load<SoundEffect>("Music\\stab_converted"));
            sounds.Add("hit", Content.Load<SoundEffect>("Music\\hit_converted"));
        }

        void playNewSong(ref ContentManager content)
        {
            String songName = "Music\\" + new Random().Next(1, 11);
            Song song = content.Load<Song>(songName);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
            MediaPlayer.Volume = .4f;
            if (songName.Contains("6")) MediaPlayer.Volume = 0.2f;
            if (songName.Contains("9") || songName.Contains("10")) MediaPlayer.Volume = 1;
        }
    }
}
