using Godot;
using Godot.Collections;
using System.Linq;

public partial class AudioManager : Node
{
    public static AudioManager _Instance;

    private Array<AudioStreamPlayer> _players;

    public override void _Ready()
    {
        _players = new Array<AudioStreamPlayer>();
        _Instance = this;
    }

    public void EnemyDeath()
    {
        var audioPlayer = AllocateAudioPlayer();

        Array<AudioStream> deathStreams = new Array<AudioStream>();

        deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Grunt1.mp3"));
        deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Grunt2.mp3"));

        audioPlayer.Stream = deathStreams.PickRandom();
        audioPlayer.Play();
    }

    private AudioStreamPlayer AllocateAudioPlayer()
    {
        AudioStreamPlayer newPlayer = new AudioStreamPlayer();
        newPlayer.Finished += newPlayer.QueueFree;

        _players.Add(newPlayer);
        AddChild(newPlayer);
        return newPlayer;
    }
}