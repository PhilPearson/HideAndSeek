using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HideAndSeek
{
	public partial class Form1 : Form
	{
		Location currentLocation;

		RoomWithDoor livingRoom;
		RoomWithHidingPlace diningRoom;
		RoomWithDoor kitchen;

		OutsideWithDoor frontYard;
		OutsideWithDoor backYard;
		OutsideWithHidingPlace garden;
		OutsideWithHidingPlace driveway;

		Room stairs;
		RoomWithHidingPlace upStairsHallway;
		RoomWithHidingPlace masterBedroom;
		RoomWithHidingPlace secondBedroom;
		RoomWithHidingPlace bathroom;

		Opponent opponent;
		int Moves;

		public Form1() {
			InitializeComponent();
			CreateObjects();
			opponent = new Opponent(frontYard);
			ResetGame(false);
		}

		private void CreateObjects() {
			livingRoom = new RoomWithDoor("Living Room", "an antique carpet", "and oak door with a brass knob", "behind the couch");
			diningRoom = new RoomWithHidingPlace("Dining Room", "a crystal chandelier", "under the dining table");
			kitchen = new RoomWithDoor("Kitchen", "stainless steel applicances", "a screen door", "in the pantry");

			frontYard = new OutsideWithDoor("Front Yard", false, "an oak door with a brass knob");
			backYard = new OutsideWithDoor("Back yard", true, "a screen door");
			garden = new OutsideWithHidingPlace("Garden", false, "in the shed");
			driveway = new OutsideWithHidingPlace("Driveway", false, "in the garage");

			stairs = new Room("Stairs","a picture of a dog");
			upStairsHallway = new RoomWithHidingPlace("Upstairs Hallway", "a light switch", "in the linen cupboard");
			masterBedroom = new RoomWithHidingPlace("Master Bedroom", "a king sized bed","under the bed");
			secondBedroom = new RoomWithHidingPlace("Second Bedroom", "a small bed", "under the bed");
			bathroom = new RoomWithHidingPlace("Bathroom", "an antique bath", "in the shower");


			diningRoom.Exits = new Location[] { livingRoom, kitchen };
			livingRoom.Exits = new Location[] { diningRoom, stairs };
			kitchen.Exits = new Location[] { diningRoom };
			frontYard.Exits = new Location[] { backYard, garden, driveway };
			backYard.Exits = new Location[] { frontYard, garden, driveway };
			garden.Exits = new Location[] { backYard, frontYard };
			driveway.Exits = new Location[] { backYard, frontYard };
			stairs.Exits = new Location[] { livingRoom, upStairsHallway };
			upStairsHallway.Exits = new Location[] { masterBedroom, secondBedroom, bathroom, stairs };
			masterBedroom.Exits = new Location[] { upStairsHallway };
			secondBedroom.Exits = new Location[] { upStairsHallway };
			bathroom.Exits = new Location[] { upStairsHallway };

			livingRoom.DoorLocation = frontYard;
			frontYard.DoorLocation = livingRoom;

			kitchen.DoorLocation = backYard;
			backYard.DoorLocation = kitchen;

			
		}

		private void RedrawForm() {
			exits.Items.Clear();
			for (int i = 0; i < currentLocation.Exits.Length; i++)
				exits.Items.Add(currentLocation.Exits[i].Name);
			exits.SelectedIndex = 0;

			description.Text = currentLocation.Description + "\r\n(move #" + Moves + ")";

			if(currentLocation is IHidingPlace) {
				IHidingPlace hidingPlace = currentLocation as IHidingPlace;
				check.Text = "Check " + hidingPlace.HidingPlace;
				check.Visible = true;
			} else {
				check.Visible = false;
			}

			if (currentLocation is IHasExteriorDoor)
				goThroughTheDoor.Visible = true;
			else
				goThroughTheDoor.Visible = false;
		}

		private void ResetGame(bool displayMessage) {
			if (displayMessage) {
				//MessageBox.Show("You found me in " + Moves + " moves!");
				IHidingPlace foundLocation = currentLocation as IHidingPlace;
				description.Text = "You found your opponent in " + Moves + " moves! He was hiding " + foundLocation.HidingPlace + ".";
			}
			Moves = 0;
			hide.Visible = true;
			goHere.Visible = false;
			check.Visible = false;
			goThroughTheDoor.Visible = false;
			exits.Visible = false;
        }

		private void MoveToANewLocation(Location newLocation) {
			//Moves++;
			currentLocation = newLocation;
			RedrawForm();

			/*
			exits.Items.Clear();
			for (int i = 0; i < currentLocation.Exits.Length; i++)
				exits.Items.Add(currentLocation.Exits[i].Name);
			exits.SelectedIndex = 0;

			description.Text = currentLocation.Description;

			if (currentLocation is IHasExteriorDoor)
				goThroughTheDoor.Visible = true;
			else
				goThroughTheDoor.Visible = false;

			*/

		}

		private void goHere_Click(object sender, EventArgs e) {
			MoveToANewLocation(currentLocation.Exits[exits.SelectedIndex]);
		}

		private void goThroughTheDoor_Click(object sender, EventArgs e) {
			IHasExteriorDoor hasDoor = currentLocation as IHasExteriorDoor;
			MoveToANewLocation(hasDoor.DoorLocation);
		}

		private void check_Click(object sender, EventArgs e) {
			Moves++;
			if (opponent.Check(currentLocation)) {
				MessageBox.Show("You found me in "+ Moves + " moves!");
				ResetGame(true);
			}
			else
				MessageBox.Show("Nope, not here, you have "+(10- Moves) +" trys left");
		}

		private void hide_Click(object sender, EventArgs e) {
			hide.Visible = false;

			for(int i=1; i <= 10; i++) {
				opponent.Move();
				description.Text = i + "... ";
				Application.DoEvents();
				System.Threading.Thread.Sleep(200);
			}

			IHidingPlace foundLocation = opponent.MyLocation as IHidingPlace;
			//description.Text = "I'm hiding " + foundLocation.HidingPlace + "\r\n";
			description.Text += "Ready or not, here I come!";
			Application.DoEvents();
			System.Threading.Thread.Sleep(500);

			goHere.Visible = true;
			exits.Visible = true;
			MoveToANewLocation(livingRoom);
		}
	}
}
