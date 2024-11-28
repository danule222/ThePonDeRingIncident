using Godot;
using System.Collections.Generic;

public partial class NTPI_AL_AudioController : Node
{
  public static NTPI_AL_AudioController Instance { get; private set; }

  private Dictionary<EGameState, AudioStream> BackgroundMusic;
  private AudioStreamPlayer2D PlayerBackMusic;

  public override void _Ready()
  {
    PlayerBackMusic = new AudioStreamPlayer2D();
    AddChild(PlayerBackMusic);
    PlayerBackMusic.Bus = "Music";

    AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("TV"), Mathf.LinearToDb(0.2f));

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

  public void SetMasterVolume(float Db)
  {
    AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Db);
  }

  public void SetMusicVolume(float Db)
  {
    AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Db);
  }

  public void SetEffectsVolume(float Db)
  {
    AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Effects"), Db);
  }

  public void SetTVVolume(float Db)
  {
    AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("TV"), Db);
  }

  public float GetMasterVolume() => AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Master"));

  public float GetMusicVolume() => AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Music"));

  public float GetEffectsVolume() => AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Effects"));

  public float GetTVVolume() => AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("TV"));
}