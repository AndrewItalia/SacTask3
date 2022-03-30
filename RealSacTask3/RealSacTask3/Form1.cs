using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealSacTask3
{
    public partial class Form1 : Form
    {
        List<TextbookSales> TbSales = new List<TextbookSales>();
        string TBfilter;
        BindingSource bs = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            // Calls CSV load function
            LoadCSVFile();
            bs.DataSource = TbSales;
            dgvTextSales.DataSource = bs;
        }

        private void LoadCSVFile()
        {
            // Hard coded the filepath of the data as the layout does not reqiure a choose file button
            string filepath = @"C:\Demo\Task3_Shop_Data.csv";
            List<string> lines = new List<string>();
            lines = File.ReadAllLines(filepath).ToList();
            foreach (string line in lines)
            {
                // Split the lines into seperate columns 
                List<string> fields = line.Split(',').ToList();
                TextbookSales t = new TextbookSales();
                // Use the data from the table to classify the datagridview table and display values and columns
                // Starting from 0
                t.TextbookName = fields[0];
                t.Subject = fields[1];
                t.PurchasePrice = float.Parse(fields[4]);
                t.SalePrice = fields[5];
                t.Rating = fields[6];
                // Adds the data to the datagridveiw table
                TbSales.Add(t);
            }
        }
        // Creates a search function for when the user selects one of the options in the dropdown combo box
        private List<TextbookSales> Search(string target, string filter)
        {
            List<TextbookSales> results = new List<TextbookSales>();
            foreach (TextbookSales t in TbSales)
            {
                // When the filter is equal to the subject it will add all subjects realated to the string  
                if (filter == "Subject")
                {
                    if (t.Subject.ToLower().Contains(target.ToLower())) { results.Add(t);}
                }
                // When the filter is equal to the Textbook it will add all textbooks realated to the string
                if (filter == "Textbook")
                {
                    if (t.TextbookName.ToLower().Contains(target.ToLower())) { results.Add(t);}
                }
                // When the filter is equal to the Rating it will add all Ratings realated to the string
                if (filter == "Rating")
                {
                    if (t.Rating.ToLower() == target.ToLower()) results.Add(t);
                }
            }
            return results;
        }

        private void DropboxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // gets the value from the ComboBox and assigns it to the textbook filter varible
            TBfilter = DropboxFilter.Text;
            // if the textbook filter varible is equal to "Rating" it will then call the sort by rating function
            if (TBfilter == "Rating") SortByRatingSelSort(TbSales);
            dgvTextSales.DataSource = bs;
            bs.ResetBindings(false);
        }


        private void SortByRatingSelSort(List<TextbookSales> Sales)
        {
            //I used the Sorting and search algoritms powerpoint as refrence for this code
            //This is a selection sort that includes a search for the rating of the textbooks
            int min;
            // the integer is valued as a minimum
            string temp;
            // temporary value holder
            for (int i = 0; i < Sales.Count - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < Sales.Count; j++)
                {

                    if (int.TryParse(Sales[j].Rating, out int ratingJ))
                    { 
               // It works by finding the smallest element in the rating catergory and putting it at the beginning of the list and then repeating that process on the unsorted remainder of the data.
                      
                        if (int.TryParse(Sales[min].Rating, out int ratingMin))
                            // outputs smallest rating
                        { if (ratingJ < ratingMin) min = j; }
                        else
                        {
                            min = j;
                        }
                    }
                }
                //Switchs the current element in Sales[j] with the current smallest one in Sales[min]
                temp = Sales[min].Rating;
                Sales[min].Rating = Sales[i].Rating;
                Sales[i].Rating = temp;
            }


        }

        private void txtSearchbox_TextChanged(object sender, EventArgs e)
        {
            // sb = SearchBox
            List<TextbookSales> sb = Search(txtSearchbox.Text, TBfilter);
            bs.DataSource = sb;
            dgvTextSales.DataSource = sb;
            bs.ResetBindings(false);
        }
    }
}
