#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace ClicWars.DirectX
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class ClicWarsGame : Game
	{
		GraphicsDeviceManager m_graphics;
		SpriteBatch m_spriteBatch;

		Texture2D m_texture;
		Point m_size = new Point(20, 20);
		Point m_location;

		Int32 squaresAcross = 10;
		Int32 squaresDown = 10;

		public ClicWarsGame()
			: base()
		{
			m_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			m_spriteBatch = new SpriteBatch(GraphicsDevice);

			m_texture = Content.Load<Texture2D>(@"basic-terrain");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			//if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			//	this.Exit();

			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Left))
			{
				m_location.X = MathHelper.Clamp(m_location.X - 2, 0, (m_size.X - squaresAcross) * 32);
			}

			if (ks.IsKeyDown(Keys.Right))
			{
				m_location.X = MathHelper.Clamp(m_location.X + 2, 0, (m_size.X - squaresAcross) * 32);
			}

			if (ks.IsKeyDown(Keys.Up))
			{
				m_location.Y = MathHelper.Clamp(m_location.Y - 2, 0, (m_size.Y - squaresDown) * 32);
			}

			if (ks.IsKeyDown(Keys.Down))
			{
				m_location.Y = MathHelper.Clamp(m_location.Y + 2, 0, (m_size.Y - squaresDown) * 32);
			}
			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			m_spriteBatch.Begin();

			Vector2 firstSquare = new Vector2(m_location.X / 32, m_location.Y / 32);
			int firstX = (int)firstSquare.X;
			int firstY = (int)firstSquare.Y;

			Vector2 squareOffset = new Vector2(m_location.X % 32, m_location.Y % 32);
			int offsetX = (int)squareOffset.X;
			int offsetY = (int)squareOffset.Y;

			for (int y = 0; y < squaresDown; y++)
			{
				for (int x = 0; x < squaresAcross; x++)
				{
					m_spriteBatch.Draw(
						m_texture,
						new Rectangle((x * 32) - offsetX, (y * 32) - offsetY, 32, 32),
						GetSourceRectangle(0),
						Color.White);
				}
			}

			m_spriteBatch.End();
			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}

		private Rectangle GetSourceRectangle(int index)
		{
			return new Rectangle(index * 32, 0, 32, 32);
		}
	}
}
