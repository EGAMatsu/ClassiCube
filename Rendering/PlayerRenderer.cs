﻿using System;
using System.Collections.Generic;
using System.Drawing;
using ClassicalSharp.GraphicsAPI;
using OpenTK;

namespace ClassicalSharp.Renderers {
	
	public class PlayerRenderer {
		
		Vector3 pos;
		public Game Window;
		public IGraphicsApi Graphics;
		public Player Player;
		Texture nameTexture;
		float nameWidth, nameHeight;
		public int PlayerTextureId = -1, MobTextureId = -1;
		int nameTextureId = -1;
		
		public PlayerRenderer( Player player, Game window ) {
			Player = player;
			Window = window;
			Graphics = window.Graphics;
			
			using( Font font = new Font( "Arial", 14 ) ) {
				List<DrawTextArgs> parts = Utils2D.SplitText( Graphics, player.DisplayName, true );
				Size size = Utils2D.MeasureSize( parts, font, true );
				nameTexture = Utils2D.MakeTextTexture( parts, font, size, 0, 0 );			
				nameWidth = size.Width;
				nameHeight = size.Height;
				nameTextureId = nameTexture.ID;
			}
		}
		
		public void Dispose() {
			Graphics.DeleteTexture( ref PlayerTextureId );
			Graphics.DeleteTexture( ref nameTextureId );
		}
		
		public void Render( double deltaTime ) {
			pos = Player.Position;
			Player.Model.RenderModel( Player, this );
			DrawName();
		}
		
		const float nameScale = 50f;
		private void DrawName() {
			Graphics.PushMatrix();
			Graphics.Translate( pos.X, pos.Y + Player.Model.NameYOffset, pos.Z );
			// Do this to always have names facing the player
			float yaw = Window.LocalPlayer.YawDegrees;
			Graphics.RotateY( 0f - yaw );
			// NOTE: Do this instead with network player's yaw to have names rotate with them instead.
			//Graphics.RotateY( 180f - yaw );
			Graphics.Scale( 1 / nameScale, -1 / nameScale, 1 / nameScale ); // -y to flip text
			Graphics.Translate( -nameWidth / 2f, -nameHeight, 0f );
			
			nameTexture.Render( Graphics );
			
			Graphics.PopMatrix();
			Graphics.Texturing = false;
			Graphics.AlphaTest = false;
		}
	}
}