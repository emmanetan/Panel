using DynamicControlApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicControlsApp
{
    public partial class AddEditMovie : Form
    {
        private Boolean IsEdit;
        private Movie OriginalMovie;
        public Movie EditedMovie;
        public Movie NewMovie;
        public Boolean DataSaved;

        public AddEditMovie()
        {
            InitializeComponent();
            IsEdit = false;
        }

        public AddEditMovie(Movie movie)
        {
            InitializeComponent();
            IsEdit = true;
            OriginalMovie = movie;
        }

        private void AddEditMovie_Load(object sender, EventArgs e)
        {
            DataSaved = false;

            if (IsEdit)
            {
                PopulateOriginalMovie();
                this.Text = "Edit";
            }

            else
            {
                ClearInput();
                this.Text = "Add";
            }
        }

        private void PopulateOriginalMovie()
        {
            TxtTitle.Text = OriginalMovie.Title;
            TxtImagePath.Text = OriginalMovie.ImagePath;
            DtpReleaseDate.Text = OriginalMovie.ReleaseDate.ToString();
        }

        private void ClearInput()
        {
            TxtTitle.Clear();
            TxtImagePath.Clear();
            DtpReleaseDate.Text = DateTime.Now.ToString();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            List<String> errors;

            errors = ValidateInput();

            if (errors.Count > 0)
            {
                ShowErrors(errors, 5);
                return;
            }

            StoreInput();
            DataSaved = true;
            this.Close();
        }

        private List<string> ValidateInput()
        {
            List<String> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(TxtTitle.Text))
                errors.Add("Title required");

            if (string.IsNullOrWhiteSpace(TxtImagePath.Text))
                errors.Add("Image Path required");

            return errors;
        }

        private void StoreInput()
        {
            string title;
            string imgPath;
            DateTime releaseDate;
            int id;

            title = TxtTitle.Text;
            imgPath = TxtImagePath.Text;
            releaseDate = DateTime.Parse(DtpReleaseDate.Text.ToString());


            if (IsEdit)
                EditedMovie = new Movie(OriginalMovie.Id,
                                        title, imgPath, releaseDate);
            else
            {
                id = Convert.ToInt32(DateTime.Now.ToString("ddHHmmss"));
                NewMovie = new Movie(id, title, imgPath, releaseDate);
            }

        }

        private void ShowErrors(List<string> errors, int max)
        {
            MessageBoxIcon icon;
            MessageBoxButtons buttons;
            string text = null;

            icon = MessageBoxIcon.Error;
            buttons = MessageBoxButtons.OK;

            if (max > errors.Count)
                max = errors.Count;

            for (int i = 0; i < max; i++)
            {
                text += errors[i] + "\n";
            }

            MessageBox.Show(text, "", buttons, icon);
        }
    }
}