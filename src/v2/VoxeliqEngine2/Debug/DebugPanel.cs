﻿using System;
using System.Drawing;
using SlimDX;
using SlimDX.Direct3D10;
using VolumetricStudios.VoxeliqEngine.Core;
using VolumetricStudios.VoxeliqEngine.Screen;
using VolumetricStudios.VoxeliqEngine.Utility.Logging;
using Font = SlimDX.Direct3D10.Font;

namespace VolumetricStudios.VoxeliqEngine.Debug
{
    public class DebugPanel : GameComponent
    {
        private IGameWindow _gameWindow; // TODO: use graphics-device instead!
        private IPlayer _player;

        private FontDescription _fontDescription;
        private Font _font;

        private static readonly Logger Logger = LogManager.CreateLogger(); // logging-facility.

        public DebugPanel(Game game) : base(game, true)
        {            
        }

        public override void Initialize()
        {
            Logger.Trace("init");

            // import required services.
            this._gameWindow = (IGameWindow)this.Game.GetService(typeof(IGameWindow));
            this._player = (IPlayer) this.Game.GetService(typeof (IPlayer));

            this._fontDescription = new FontDescription()
            {
                Height = 22,
                FaceName = "Verdana",
                IsItalic = false,
                Width = 0,
                MipLevels = 1,
                CharacterSet = FontCharacterSet.Ansi,
                Precision = FontPrecision.TrueType,
                Quality = FontQuality.Antialiased,
                PitchAndFamily = FontPitchAndFamily.Fixed | FontPitchAndFamily.DontCare,
                Weight =  FontWeight.Normal
            };

            var texture = Resource.FromSwapChain<Texture2D>(this._gameWindow.SwapChain, 0);
            this._font = new Font(texture.Device, this._fontDescription);
        }

        public override void Draw(GameTime gameTime)
        {
            this.DrawText("fps:" + (int)gameTime.FramesPerSecond, 5, 5, Color.White);
            this.DrawText("mem: " + this.GetMemSize(GC.GetTotalMemory(false)), 95, 5, Color.White);
            this.DrawText("pos:" +  this._player.Position, 250, 5, Color.White);
            this.DrawText("chunks: n/a", 5, 35, Color.White);
            this.DrawText("blocks: n/a", 125, 35, Color.White);
            this.DrawText("genQ/buildQ: n/a", 255, 35, Color.White);
            this.DrawText("inf: off", 5, 70, Color.White);
            this.DrawText("fly: off", 75, 70, Color.White);
            this.DrawText("fog: off", 145, 70, Color.White);
        }

        private void DrawText(string text, int left, int top, Color4 color)
        {
            this._font.Draw(null, text, new Rectangle(left, top, 0, 0), FontDrawFlags.NoClip, color);
        }

        private string GetMemSize(long size)
        {
            int i;
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            double dblSByte = 0;
            for (i = 0; (int)(size / 1024) > 0; i++, size /= 1024) dblSByte = size / 1024.0;
            return dblSByte.ToString("0.00") + suffixes[i];
        }
    }
}
