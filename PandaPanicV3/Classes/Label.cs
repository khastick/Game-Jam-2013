using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PandaPanicV3
{
    public class Label
    {
        public readonly static int WIDTH,HEIGHT;
        static Color BOX_COLOR; // the color of the box

        Rectangle   box;
        string      text;
        Vector2     textPosition;

        public string Text
        {
            get { return text; }
            set { text = value;}
        }

        static Label(){
            WIDTH = 300;
            HEIGHT = 32;
            BOX_COLOR = Color.Black;
        }

        public Label(Vector2 position, string text, Rectangle box)
        {
            this.box = box;
            this.text = text;
            textPosition = position;
        }

        public Label(Vector2 position, string text):
            this(position,text,new Rectangle((int)position.X -5, (int)position.Y,WIDTH,HEIGHT)){}

        public Label(Vector2 position, string text, int width, int height) :
            this(position, text, new Rectangle((int)position.X - 5, (int)position.Y, width, height)) { }

        public void Draw(){
            Game1.batch.Draw(Artist.textures["square"], box, BOX_COLOR);
            Artist.fontRenderer.DrawText(Game1.batch, (int)textPosition.X, (int)textPosition.Y, text);
        }
    }
}
