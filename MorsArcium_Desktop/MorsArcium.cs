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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System;
using Mors_Arcium;

namespace MorsArcium_Desktop
{
    public class MorsArcium : Game, IPlatformOutlet
    {
        public Rectangle GameViewport { get => gameViewport; }
        public Settings GameSettings { get => settings; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        Rectangle gameViewport;

        GameManager gMan;
        Settings settings;

        MouseState mouseState;
        Vector2 mouseMenuCoords;

        public MorsArcium()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            settings = new Settings();

            //Load settings from XML
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(Directory.GetCurrentDirectory() + "/settings.xml");

                DeserializeBoolSetting(doc, "musicEnabled", out settings.musicEnabled);
                DeserializeBoolSetting(doc, "soundEnabled", out settings.soundEnabled);
                DeserializeBoolSetting(doc, "vSync", out settings.vSync);
                DeserializeBoolSetting(doc, "fullScreen", out settings.fullScreen);
                DeserializeBoolSetting(doc, "jumpFly", out settings.jumpFly);
                DeserializeBoolSetting(doc, "playedBefore", out settings.playedBefore);
                DeserializeBinding(doc, "aimUp", out settings.aimUp);
                DeserializeBinding(doc, "aimDown", out settings.aimDown);
                DeserializeBinding(doc, "moveLeft", out settings.moveLeft);
                DeserializeBinding(doc, "moveRight", out settings.moveRight);
                DeserializeBinding(doc, "jump", out settings.jump);
                DeserializeBinding(doc, "attack", out settings.attack);
                DeserializeBinding(doc, "special", out settings.special);
                DeserializeBinding(doc, "pause", out settings.pause);

            }
            catch (FileNotFoundException ex)
            {
                ex.ToString(); //Get rid of IDE warning

                Console.WriteLine("Settings file not found. Creating new one.");

                settings.aimUp = new KeyBinding(Keys.W);
                settings.aimDown = new KeyBinding(Keys.S);
                settings.moveLeft = new KeyBinding(Keys.A);
                settings.moveRight = new KeyBinding(Keys.D);
                settings.jump = new KeyBinding(Keys.Space);
                settings.attack = new KeyBinding(Keys.J);
                settings.special = new KeyBinding(Keys.K);
                settings.pause = new KeyBinding(Keys.Enter);
                settings.musicEnabled = true;
                settings.soundEnabled = true;
                settings.vSync = false;
                settings.fullScreen = false;
                settings.jumpFly = false;
                settings.playedBefore = false;

                SaveSettings();
            }

            gMan = new GameManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Window.Title = "MORS ARCIUM";
            Window.Position = new Point(64, 64);
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
            graphics.ApplyChanges();

            ApplyVideoSettings();

            gameViewport = GraphicsDevice.Viewport.Bounds;

            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            gMan.LoadContent(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mouseState = Mouse.GetState();
            mouseMenuCoords = new Vector2(mouseState.X / 3.0f, mouseState.Y / 3.0f);

            gMan.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(renderTarget);
            gMan.Draw(GraphicsDevice, gameTime);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, null, null, null);
            spriteBatch.Draw(renderTarget, gameViewport, Color.White);
            spriteBatch.End();
        }

        public Mors_Arcium.GUI.Button.State ProcessMenuButton(Mors_Arcium.GUI.Button button)
        {
            if (mouseMenuCoords.X > button.Position.X 
                && mouseMenuCoords.X < button.Position.X + button.Source.Width
                && mouseMenuCoords.Y > button.Position.Y
                && mouseMenuCoords.Y < button.Position.Y + button.Source.Height)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    return Mors_Arcium.GUI.Button.State.PRESSED;
                }
                else
                {
                    return Mors_Arcium.GUI.Button.State.HOVER;
                }
            }
            return Mors_Arcium.GUI.Button.State.DEFAULT;
        }

        public void ApplyVideoSettings()
        {
            graphics.SynchronizeWithVerticalRetrace = settings.vSync;
            if (graphics.IsFullScreen != settings.fullScreen) graphics.ToggleFullScreen();
            graphics.ApplyChanges();
        }

        public void SaveSettings()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);

            XmlElement settingsElem = doc.CreateElement("settings");
            
            SerializeBoolSetting(settingsElem, "musicEnabled", settings.musicEnabled);
            SerializeBoolSetting(settingsElem, "soundEnabled", settings.soundEnabled);
            SerializeBoolSetting(settingsElem, "fullScreen", settings.fullScreen);
            SerializeBoolSetting(settingsElem, "vSync", settings.vSync);
            SerializeBoolSetting(settingsElem, "jumpFly", settings.jumpFly);
            SerializeBoolSetting(settingsElem, "playedBefore", settings.playedBefore);
            SerializeBinding(settingsElem, "aimUp", settings.aimUp);
            SerializeBinding(settingsElem, "aimDown", settings.aimDown);
            SerializeBinding(settingsElem, "moveLeft", settings.moveLeft);
            SerializeBinding(settingsElem, "moveRight", settings.moveRight);
            SerializeBinding(settingsElem, "jump", settings.jump);
            SerializeBinding(settingsElem, "attack", settings.attack);
            SerializeBinding(settingsElem, "special", settings.special);
            SerializeBinding(settingsElem, "pause", settings.pause);

            doc.AppendChild(settingsElem);

            doc.Save("settings.xml");

            Console.WriteLine("Saved settings.");
        }

        private void SerializeBinding(XmlElement parent, string name, Binding binding)
        {
            XmlElement elem = parent.OwnerDocument.CreateElement(name);
            elem.AppendChild(binding.Serialize(parent.OwnerDocument));
            parent.AppendChild(elem);
        }

        private void DeserializeBinding(XmlDocument doc, string name, out Binding binding)
        {
            XmlElement elem = (XmlElement) doc.DocumentElement.SelectSingleNode(name + "/*");
            switch (elem.Name)
            {
                case "keyBinding": binding = KeyBinding.Deserialize(elem); break;
                case "mouseButtonBinding": binding = MouseButtonBinding.Deserialize(elem); break;
                case "joystickButtonBinding": binding = JoystickButtonBinding.Deserialize(elem); break;
                case "joystickHatBinding": binding = JoystickHatBinding.Deserialize(elem); break;
                case "joystickAxisBinding": binding = JoystickAxisBinding.Deserialize(elem); break;
                default: binding = null; break;
            }
        }

        private void SerializeBoolSetting(XmlElement parent, string name, bool value)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name);
            
            XmlAttribute attribute = parent.OwnerDocument.CreateAttribute("value");
            attribute.Value = value.ToString();
            element.Attributes.Append(attribute);

            parent.AppendChild(element);
        }

        private void DeserializeBoolSetting(XmlDocument doc, string name, out bool setting)
        {
            Boolean.TryParse(doc.DocumentElement.SelectSingleNode(name + "/@value").Value, out setting);
        }
    }
}
