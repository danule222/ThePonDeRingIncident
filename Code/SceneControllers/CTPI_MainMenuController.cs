using Godot;

public partial class CTPI_MainMenuController : Control
{
  private CTPI_Button BTN_Play;
  private CTPI_Button BTN_Settings;
  private CTPI_Button BTN_Credits;
  private CTPI_Button BTN_Exit;
  private VBoxContainer VBX_MainMenu;
  private TextureRect IMG_Logo;
  private CTPI_Settings PNL_Settings;
  private Panel PNL_Credits;

  // Credits
  private CTPI_Button BTN_Credits_Back;
  private RichTextLabel RTL_Credits;

  public override void _Ready()
  {
    Input.MouseMode = Input.MouseModeEnum.Visible;

    VBX_MainMenu = GetNode<VBoxContainer>("VBX_MainMenu");
    IMG_Logo = GetNode<TextureRect>("IMG_Logo");
    BTN_Play = VBX_MainMenu.GetNode<CTPI_Button>("BTN_Play");
    BTN_Settings = VBX_MainMenu.GetNode<CTPI_Button>("BTN_Settings");
    BTN_Credits = VBX_MainMenu.GetNode<CTPI_Button>("BTN_Credits");
    BTN_Exit = VBX_MainMenu.GetNode<CTPI_Button>("BTN_Exit");
    PNL_Settings = GetNode<CTPI_Settings>("PNL_Settings");
    PNL_Credits = GetNode<Panel>("PNL_Credits");
    BTN_Credits_Back = PNL_Credits.GetNode<CTPI_Button>("HBoxContainer/BTN_Credits_Back");
    RTL_Credits = PNL_Credits.GetNode<RichTextLabel>("RTL_Credits");

    RTL_Credits.MetaClicked += OpenURL;

    PNL_Settings.ParentMenu = VBX_MainMenu;

    BTN_Play.Pressed += PlayPressed;
    BTN_Settings.Pressed += SettingsPressed;
    BTN_Credits.Pressed += CreditsPressed;
    BTN_Credits_Back.Pressed += CreditsBackPressed;
    BTN_Exit.Pressed += ExitPressed;

    // Music
    NTPI_AL_MusicController.Instance.PlayBackgroundMusic(EGameState.MainMenu);

    VBX_MainMenu.VisibilityChanged += MainMenuVisibilityChanged;
  }

  public override void _Process(double delta)
  {

  }

  private void OpenURL(Variant url)
  {
    OS.ShellOpen((string)url);
  }

  private void MainMenuVisibilityChanged()
  {
    if (VBX_MainMenu.Visible)
      IMG_Logo.Visible = true;
    else
      IMG_Logo.Visible = false;
  }

  private void PlayPressed()
  {
    GetTree().ChangeSceneToFile("res://Scenes/S_Investigation.tscn");
  }

  private void SettingsPressed()
  {
    VBX_MainMenu.Visible = false;
    PNL_Settings.Visible = true;
  }

  private void CreditsPressed()
  {
    VBX_MainMenu.Visible = false;
    PNL_Credits.Visible = true;
  }

  private void CreditsBackPressed()
  {
    PNL_Credits.Visible = false;
    VBX_MainMenu.Visible = true;
  }

  private void ExitPressed()
  {
    GetTree().Quit();
  }
}