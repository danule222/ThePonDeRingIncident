using Godot;
using System.Collections.Generic;

public partial class NTPI_AL_MusicController : Node
{
  public static NTPI_AL_MusicController Instance { get; private set; }

  private Dictionary<EGameState, AudioStream> BackgroundMusic;
  private AudioStreamPlayer2D PlayerBackMusic;

  public override void _Ready()
  {
    PlayerBackMusic = new AudioStreamPlayer2D();
    AddChild(PlayerBackMusic);
    PlayerBackMusic.VolumeDb = -15f;

    BackgroundMusic = new Dictionary<EGameState, AudioStream>()
    {
      {EGameState.MainMenu, GD.Load<AudioStream>("res://Audio/Music/Music_SPARKS (Midnight ver.).ogg")},
      {EGameState.Investigation, GD.Load<AudioStream>("res://Audio/Music/Music_Doggy god’s street (Midnight ver.).ogg")},
      {EGameState.Interrogation, GD.Load<AudioStream>("res://Audio/Music/Music_メイジ・オブ・ヴァイオレット (Midnight ver.).ogg")}
    };

    PlayBackgroundMusic(EGameState.MainMenu);

    Instance = this;
  }

  public void PlayBackgroundMusic(EGameState state)
  {
    PlayerBackMusic.Stop();

    PlayerBackMusic.Stream = BackgroundMusic[state];

    PlayerBackMusic.Play();
  }

  public void SetMusicVolume(float Db)
  {
    PlayerBackMusic.VolumeDb = Db;
  }

  public float GetMusicVolume() => PlayerBackMusic.VolumeDb;
}