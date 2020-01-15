using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium.GUI
{
    public class OptionsMenu : Menu
    {
        protected readonly Settings settings;
        
        public OptionsMenu(GameManager gMan) : base(gMan)
        {
            settings = gMan.platform.GameSettings;

            //Back button
            buttons.Add(new Button(gMan, gMan.textures["hud"], 
                new Rectangle(384, 152, 128, 24), 
                new Vector2(16, 16), 
                (Button b) => {
                    gMan.ChangeMenuState(new MainMenu(gMan));
                }));
#if WINDOWS
            //FullScreen checkBox
            buttons.Add(new CheckBox(gMan, new Vector2(16, 60), (Button b) => {
                gMan.platform.GameSettings.fullScreen = ((CheckBox)b).IsChecked;
            }, gMan.platform.GameSettings.fullScreen));

            //Rebind menu button
            buttons.Add(new Button(gMan, gMan.textures["hud"], 
                new Rectangle(384, 128, 128, 24),
                new Vector2(16, 200),
                (Button b) => {
                    gMan.ChangeMenuState(new RebindMenu(gMan));
                }));
#endif
            //Sound Checkbox
            buttons.Add(new CheckBox(gMan, new Vector2(16, 92), (Button b) => {
                gMan.platform.GameSettings.soundEnabled = ((CheckBox)b).IsChecked;
            }, gMan.platform.GameSettings.soundEnabled));

            //Music Checkbox
            buttons.Add(new CheckBox(gMan, new Vector2(16, 124), (Button b) => {
                gMan.platform.GameSettings.musicEnabled = ((CheckBox)b).IsChecked;
            }, gMan.platform.GameSettings.musicEnabled));

            //Jump-To-Fly for Bug Checkbox
            buttons.Add(new CheckBox(gMan, new Vector2(16, 156), (Button b) => {
                gMan.platform.GameSettings.jumpFly = ((CheckBox)b).IsChecked;
            }, gMan.platform.GameSettings.jumpFly));

            //Vsync Checkbox
            buttons.Add(new CheckBox(gMan, new Vector2(160, 124), (Button b) => {
                gMan.platform.GameSettings.vSync = ((CheckBox)b).IsChecked;
            }, gMan.platform.GameSettings.vSync));
        }

        public override void OnLeave()
        {
            gMan.platform.ApplyVideoSettings();
            gMan.platform.SaveSettings();
        }

        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            DrawTrippyBackground(sp);
            DrawButtons(sp);

#if WINDOWS
            sp.DrawString(gMan.fonts["default"], "FULLSCREEN", new Vector2(52, 68), Color.White);
#endif
            sp.DrawString(gMan.fonts["default"], "SOUND", new Vector2(52, 100), Color.White);
            sp.DrawString(gMan.fonts["default"], "MUSIC", new Vector2(52, 132), Color.White);
            sp.DrawString(gMan.fonts["default"], "FLY BY JUMPING IN MIDAIR", new Vector2(52, 164), Color.White);
            sp.DrawString(gMan.fonts["default"], "VSYNC", new Vector2(196, 132), Color.White);

            sp.End();
        }
    }
}
