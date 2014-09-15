using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Visuals.Flat;
using Nuclex.Input;

namespace PandaPanicV3
{
    partial class PandaPanic
    {
        ButtonControl startButton;
        ButtonControl creditButton;
        ButtonControl instructionButton;
        ButtonControl exitButton;

        private void createDesktopControls(Screen mainScreen)
        {
            startButton = new ButtonControl();
            creditButton = new ButtonControl();
            instructionButton = new ButtonControl();
            exitButton = new ButtonControl();

            startButton.Text = "New Game"; // configure the start button
            startButton.Bounds = new UniRectangle(
              new UniScalar(-0.1f, 0.0f), new UniScalar(0.0f, 445.0f), 100, 32
            );
            startButton.Pressed += new EventHandler(
                delegate(object sender, EventArgs arguments){
                    _cont.start();
                    gui.Visible = false;
                });
            mainScreen.Desktop.Children.Add(startButton);

            creditButton.Text = "Credits"; // configure the credit button
            creditButton.Bounds = new UniRectangle(
              new UniScalar(0.1f, 0.0f), new UniScalar(0.0f, 445.0f), 100, 32
            );
            creditButton.Pressed += new EventHandler(
                delegate(object sender, EventArgs arguments){
                    _cont.credits();
                });
            mainScreen.Desktop.Children.Add(creditButton);

            instructionButton.Text = "Instructions"; // configure the instruction button
            instructionButton.Bounds = new UniRectangle(
              new UniScalar(0.3f, 0.0f), new UniScalar(0.0f, 445.0f), 100, 32
            );
            instructionButton.Pressed += new EventHandler(
                delegate(object sender, EventArgs arguments){
                    _cont.instructions();
                });
            mainScreen.Desktop.Children.Add(instructionButton);

            exitButton.Text = "Exit"; // configure the exit button
            exitButton.Bounds = new UniRectangle(
              new UniScalar(0.5f, 0.0f), new UniScalar(0.0f, 445.0f), 100, 32
            );
            exitButton.Pressed += new EventHandler(
                delegate(object sender, EventArgs arguments) { 
                    Exit(); 
                });
            mainScreen.Desktop.Children.Add(exitButton);
        }

    }
}
