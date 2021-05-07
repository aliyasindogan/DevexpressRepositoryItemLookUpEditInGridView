using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;

namespace DevexpressRepositoryItemLookUpEditInGridView
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        private Reader newReader = new Reader();
        private List<Reader> newReaders = new List<Reader>();

        public XtraForm1()
        {
            InitializeComponent();
            InitData();

            gridControl1.DataSource = Readers;

            // Create an in-place LookupEdit control.
            RepositoryItemLookUpEdit riLookup = new RepositoryItemLookUpEdit();
            riLookup.DataSource = TimeZones;
            riLookup.ValueMember = "Id";
            riLookup.DisplayMember = "TimeZoneName";

            riLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            riLookup.DropDownRows = TimeZones.Count;

            riLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            riLookup.AutoSearchColumnIndex = 1;

            gridControl1.RepositoryItems.Add(riLookup);

            gridView1.Columns["TimeZoneID"].ColumnEdit = riLookup;
            gridView1.Columns.ColumnByFieldName("Id").OptionsColumn.AllowEdit = false;
            gridView1.Columns.ColumnByFieldName("ReaderName").OptionsColumn.AllowEdit = false;
            //gridView1.OptionsSelection.MultiSelect = true;
            //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            //gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DefaultBoolean.True;

            gridView1.CellValueChanging += GridView1_CellValueChanging;
            gridView1.BestFitColumns();
        }

        private void GridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            Debug.WriteLine(" Id Value: " + gridView1.GetRowCellValue(e.RowHandle, "Id") +
                            " Column: " + e.Column.Name +
                            " Value: " + e.Value +
                            " RowHandle: " + e.RowHandle.ToString());
            newReaders.Add(new Reader()
            {
                ReaderActive = e.Column.Name == "colReaderActive" ? (bool)e.Value : Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "ReaderActive")),
                TimeZoneID = e.Column.Name == "colTimeZoneID" ? (int)e.Value : Convert.ToInt32(gridView1.GetRowCellValue(e.RowHandle, "TimeZoneID")),
                Id = Convert.ToInt32(gridView1.GetRowCellValue(e.RowHandle, "Id")),
                ReaderName = Convert.ToString(gridView1.GetRowCellValue(e.RowHandle, "ReaderName")),
            });
        }

        private List<TimeZone> TimeZones = new List<TimeZone>();
        private List<Reader> Readers = new List<Reader>();

        private void InitData()
        {
            var timeZones = new TimeZone[]
            {
                new TimeZone{Id = 1, TimeZoneName = "TimeZoneName1"},
                new TimeZone{Id = 2, TimeZoneName = "TimeZoneName2"},
                new TimeZone{Id = 3, TimeZoneName = "TimeZoneName3"},
                new TimeZone{Id = 4, TimeZoneName = "TimeZoneName4"},
            };
            TimeZones.AddRange(timeZones);

            var readers = new Reader[]
            {
                new Reader{Id = 1, TimeZoneID = 1,ReaderActive = false,ReaderName = "Reader1"},
                new Reader{Id = 2, TimeZoneID = 1,ReaderActive = false,ReaderName = "Reader2"},
                new Reader{Id = 3, TimeZoneID = 1,ReaderActive = false,ReaderName = "Reader3"},
                new Reader{Id = 4, TimeZoneID = 1,ReaderActive = false,ReaderName = "Reader4"},
                new Reader{Id = 5, TimeZoneID = 1,ReaderActive = false,ReaderName = "Reader5"},
                new Reader{Id = 6, TimeZoneID = 1,ReaderActive = false,ReaderName = "Reader6"},
                new Reader{Id = 7, TimeZoneID = 1,ReaderActive = false,ReaderName = "Reader7"},
                new Reader{Id = 8, TimeZoneID = 1,ReaderActive = false,ReaderName = "Reader8"},
            };
            Readers.AddRange(readers);
        }

        private class Reader
        {
            public int Id { get; set; }
            public string ReaderName { get; set; }
            public bool ReaderActive { get; set; }
            public int TimeZoneID { get; set; }
        }

        private class TimeZone
        {
            public int Id { get; set; }

            public string TimeZoneName { get; set; }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var reader in newReaders)
            {
                Debug.WriteLine(reader.Id + " - " + reader.ReaderName + " - " + reader.ReaderActive + " - " + reader.TimeZoneID);
            }
            newReaders.RemoveAll(x => x.Id > 0);
        }

        private void btnReaderActive_Click(object sender, EventArgs e)
        {
            List<Reader> readerActiveList = new List<Reader>();
            foreach (var reader in Readers)
            {
                var data = Readers.Find(x => x.Id == reader.Id);
                readerActiveList.Add(new Reader()
                {
                    Id = data.Id,
                    ReaderActive = true,
                    ReaderName = data.ReaderName,
                    TimeZoneID = data.TimeZoneID,
                });
            }
            gridControl1.DataSource = readerActiveList;
        }
    }
}