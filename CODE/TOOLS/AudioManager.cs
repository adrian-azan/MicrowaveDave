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
		var gruntAudioPlayer = AllocateAudioPlayer();
		var audioPlayer = AllocateAudioPlayer();

		Array<AudioStream> deathStreams = new Array<AudioStream>();
		deathStreams.Add(ResourceLoader.Load<AudioStreamWav>("res://ART/SOUND/Captain Falcon 11.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStreamWav>("res://ART/SOUND/Captain Falcon 12.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStreamWav>("res://ART/SOUND/Captain Falcon 13.wav"));
		gruntAudioPlayer.Stream = deathStreams.PickRandom();
		gruntAudioPlayer.Play();
		
		
		Array<AudioStream> fxStreams = [
			ResourceLoader.Load<AudioStream>("res://ART/SOUND/Strong Hit.wav"),
			ResourceLoader.Load<AudioStream>("res://ART/SOUND/Smash Hit.wav"),
			ResourceLoader.Load<AudioStream>("res://ART/SOUND/Home Run Bat Hit.wav")
		];
		audioPlayer.Stream = fxStreams.PickRandom();
		audioPlayer.Play();
	}

	public void Punch()
	{
		var audioPlayer = AllocateAudioPlayer();

		Array<AudioStream> deathStreams = new Array<AudioStream>();

		deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Punch 2.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Punch 3.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Punch 4.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Punch 1.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Punch 5.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Punch 6.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Punch 7.wav"));
		deathStreams.Add(ResourceLoader.Load<AudioStream>("res://ART/SOUND/Punch 8.wav"));

		audioPlayer.Stream = deathStreams.PickRandom();
		audioPlayer.Play();
	}

	public void Block()
	{
		var audioPlayer = AllocateAudioPlayer();

		Array<AudioStream> deathStreams =
		[
			ResourceLoader.Load<AudioStreamWav>("res://ART/SOUND/Bonus Scoring.wav"),
			ResourceLoader.Load<AudioStreamWav>("res://ART/SOUND/Bumper Hit.wav"),
		];

		
		audioPlayer.Stream =  deathStreams.PickRandom();
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
