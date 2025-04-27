using DynamicControlsApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicControlApp
{
    public partial class Form1: Form
    {
        private List<Movie> Movies = new List<Movie>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void AddMovieToUI(Movie movie)
        {
            //Create panel
            Panel panel;
            panel = new Panel();
            panel.Name = String.Format("PnlMovie{0}", movie.Id);
            panel.BackColor = Color.White;
            panel.Size = new Size(125, 205);
            panel.Margin = new Padding(10);
            panel.Tag = movie.Id;

            //Create picture box
            PictureBox picBox;
            picBox = new PictureBox();
            picBox.Name = String.Format("PbMovieImage{0}", movie.Id);
            picBox.Size = new Size(100, 148);
            picBox.Location = new Point(12, 10);
            picBox.SizeMode = PictureBoxSizeMode.Zoom;

            if (File.Exists(movie.ImagePath))
                picBox.Image = Image.FromFile(movie.ImagePath);

            picBox.Tag = movie.Id;

            //Create title label
            Label labelTitle;
            labelTitle = new Label();
            labelTitle.Name = String.Format("LblMovieTitle{0}", movie.Id);
            labelTitle.Text = movie.Title;
            labelTitle.Location = new Point(12, 165);
            labelTitle.ForeColor = Color.Black;
            labelTitle.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelTitle.AutoSize = true;
            labelTitle.Tag = movie.Id;

            //Create year label
            Label labelYear;
            labelYear = new Label();
            labelYear.Name = String.Format("LblMovieYear{0}", movie.Id);
            labelYear.Text = movie.ReleaseDate.Year.ToString();
            labelYear.Location = new Point(12, 185);
            labelYear.ForeColor = Color.Gray;
            labelYear.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelYear.Tag = movie.Id;

            //Set Context Menu
            panel.ContextMenuStrip = contextMenuStrip1;

            //Add controls to panel 
            panel.Controls.Add(picBox);
            panel.Controls.Add(labelTitle);
            panel.Controls.Add(labelYear);

            //Add Event Handlers 
            panel.DoubleClick += new EventHandler(Edit_DoubleClick);

            foreach (Control c in panel.Controls)
            {
                c.DoubleClick += new EventHandler(Edit_DoubleClick);
            }

            //Add panel to flowlayoutpanel
            flowLayoutPanel1.Controls.Add(panel);

        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddEditMovie form = new AddEditMovie();

            form.ShowDialog();

            //Add new movie to list and UI
            if (form.DataSaved)
            {
                Movies.Add(form.NewMovie);
                AddMovieToUI(form.NewMovie);
            }
        }

        private void UpdateMovieInUI(Movie movie)
        {
            Control control;
            PictureBox picBox;
            string name;

            //Find picturebox and update movie image
            name = String.Format("PbMovieImage{0}", movie.Id);
            control = this.Controls.Find(name, true).FirstOrDefault();
            picBox = (PictureBox)control;

            if (File.Exists(movie.ImagePath))
                picBox.Image = Image.FromFile(movie.ImagePath);
            else
                picBox.Image = null;

            //Find movie title label and update text
            name = String.Format("LblMovieTitle{0}", movie.Id);
            control = this.Controls.Find(name, true).FirstOrDefault();
            control.Text = movie.Title;

            //Find movie year label and update text
            name = String.Format("LblMovieYear{0}", movie.Id);
            control = this.Controls.Find(name, true).FirstOrDefault();
            control.Text = movie.ReleaseDate.Year.ToString();
        }

        private void Edit_DoubleClick(object sender, EventArgs e)
        {
            Control c;
            int id;
            int index;
            Movie movie;
            AddEditMovie form;

            //Get movie using control tag/id
            c = (Control)sender;
            id = (int)c.Tag;
            movie = Movies.Find(x => x.Id == id);

            //Open Add/Edit form
            form = new AddEditMovie(movie);
            form.ShowDialog();

            //Update movie in list and UI
            if (form.DataSaved)
            {
                index = Movies.FindIndex(x => x.Id == id);
                Movies[index].Copy(form.EditedMovie);
                UpdateMovieInUI(Movies[index]);
            }
        }

        private void DeleteMovieFromUI(Movie movie)
        {
            Control panel;
            string name;

            //Find panel 
            name = String.Format("PnlMovie{0}", movie.Id);
            panel = this.Controls.Find(name, true).FirstOrDefault();

            //Remove event handlers
            panel.DoubleClick -= new EventHandler(Edit_DoubleClick);

            foreach (Control c in panel.Controls)
            {
                c.DoubleClick -= new EventHandler(Edit_DoubleClick);
            }

            //Remove panel
            flowLayoutPanel1.Controls.Remove(panel);
            panel.Dispose();
        }

        private void deleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem;
            ContextMenuStrip menuStrip;
            Control control;
            int id;
            int index = -1;
            Movie movie;

            //Find selected control
            menuItem = (ToolStripMenuItem)sender;
            menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();
            control = menuStrip.SourceControl;

            //Get movie from control tag/id
            id = (int)control.Tag;
            movie = Movies.Find(x => x.Id == id);

            //Delete movie from list 
            index = Movies.FindIndex(x => x.Id == id);
            Movies.RemoveAt(index);

            //Delete movie from UI
            DeleteMovieFromUI(movie);
        }
    }
}
