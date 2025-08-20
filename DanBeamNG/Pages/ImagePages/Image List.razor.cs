using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Data;
using SpenSoft.DanBeamNG.Services;
using System.Reflection.Metadata;

namespace SpenSoft.DanBeamNG.Pages.ImagePages
{
    public partial class Image_List : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? ID { get; set; }

        [Parameter]
        public String? Reverse { get; set; }

        [Parameter]
        public String? Filter
        {
            get => searchData.Filter;
            set => searchData.Filter = value;
        }

        [Parameter]
        public String? Assigned { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public VehicleImageDataService_Interface? VehicleImageDataService { get; set; }

        [Inject]
        public ImagesDataDataService_Interface? imageDataService { get; set; }

        [Inject]
        public NavigationManager? navigationManager { get; set; }

        [Inject]
        public IJSRuntime? JSRuntime { get; set; }

        //[Inject]
        //public HttpClient? _client { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Properties

        private List<VehicleImage?>? fullImageList { get; set; } = null;

        public int MaxColumns { get; set; } = 4;

        #endregion Private Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public List<VehicleImage?>? ImageList { get; set; } = null;

        public VehicleImage? vehicleImage { get; set; } = new VehicleImage();

        public int itemHeight { get; } = 50;

        public SearchData searchData { get; set; } = new SearchData();

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message1 = String.Empty;
        protected String Message2 = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Error = false;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            Error = false;
            try
            {
                fullImageList = null;
                if (String.IsNullOrEmpty(Assigned) == false)
                {
                    if ((Boolean.TryParse(Assigned, out Boolean assigned) && assigned == false))
                    {
                        if (VehicleImageDataService != null) fullImageList = (await VehicleImageDataService.GetUnassignedImages()).OrderBy(x => x?.ImageName).ToList();
                    }
                }
                if (fullImageList == null)
                {
                    if (VehicleImageDataService != null) fullImageList = (await VehicleImageDataService.GetAllVehicleImage()).OrderBy(x => x?.ImageName).ToList();
                }
            }
            catch (Exception ex)
            {
                StatusClass = "alert-danger"; Message1 = $"The following Exception error was thrown while trying to retrieve the image list: {ex.Message}";
                Message2 = "Please see runtime log for more details";
                Error = true;
            }
        }


        protected override async Task OnParametersSetAsync()
        {
            if (String.IsNullOrEmpty(Assigned) == false)
            {
                await HandleUnassigned();
            }
            else
            {
                await HandleAssigned();
            }
        }


        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message1 = String.Empty;
            Error = false;
        }

        protected async Task ValidSumitHandler()
        {
            await Filter_Changed(searchData.Filter);
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods

        public async Task Filter_Changed(String filter)
        {
            Filter = filter;
            StateHasChanged();
            if (String.IsNullOrEmpty(Assigned) == false)
            {
                await HandleUnassigned();
            }
            else
            {
                await HandleAssigned();
            }
        }

        public async void ClearSearchData() => Filter_Changed(String.Empty);

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private async Task HandleAssigned()
        {
            if (fullImageList == null) fullImageList = (await VehicleImageDataService.GetAllVehicleImage()).ToList();
            if (String.IsNullOrEmpty(Reverse) == false)
            {
                Boolean rev = false;
                Boolean.TryParse(Reverse, out rev);
                if (rev == true)
                {
                    ImageList = fullImageList
                        .OrderByDescending(x => new DateTime(x.ImageEntered.Year, x.ImageEntered.Month, x.ImageEntered.Day, x.ImageEntered.Hour, x.ImageEntered.Minute, 0))
                        .ThenBy(x => x?.ImageName)
                        .ToList();
                }
                else
                {
                    ImageList = fullImageList
                        .OrderBy(x => x?.ImageName)
                        .ToList();
                }
            }
            else
            {
                ImageList = fullImageList;
            }

            if (String.IsNullOrEmpty(Filter) == false)
            {
                ImageList = fullImageList.Where(x => x.ImageName.Contains(Filter, StringComparison.OrdinalIgnoreCase) || x.ImageID.ToString().Contains(Filter, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        private async Task HandleUnassigned()
        {
            if ((Boolean.TryParse(Assigned, out Boolean assigned) && assigned == false))
            {
                List<VehicleImage?>? unassignedImageList = null;
                if (VehicleImageDataService != null)
                {
                    unassignedImageList = (await VehicleImageDataService.GetUnassignedImages()).OrderBy(x => x?.ImageName).ToList();
                    if (String.IsNullOrEmpty(Filter) == false)
                    {
                        ImageList = unassignedImageList.Where(x => x.ImageName.ToUpper().Contains(Filter.ToUpper())).ToList();
                    }
                    else ImageList = unassignedImageList;
                }
            }
        }

        private Byte[]? GetImageData(Guid? imageID)
        {
            return null;
        }

        private async void Delete_Handler(Guid? imageID)
        {
            if (imageID == null) return;
            if (VehicleImageDataService != null) await VehicleImageDataService.DeleteVehicleImage(imageID);
            await OnParametersSetAsync();
        }

#if true
        private async Task GenerateReport()
        {
        }

#else
        private async Task GenerateReport()
        {
            GlobalFontSettings.FontResolver = new FailsafeFontResolver();
#if true
            if ((ImageList != null) && (ImageList.Count > 0))
            {
                PDF_Helper pdf = new PDF_Helper();
                pdf.Create_Document();
                pdf.Document.SecurityHandler.SetEncryptionToV5(false);
                pdf.AddPage(PageSize.Letter, PageOrientation.Portrait);
                var f = new XFont("Times New Roman", 10, XFontStyleEx.Regular);
                pdf.Set_Default_Font(f);
                if (pdf.CurrentPage != null)
                {
                    pdf.Margin(0, 0, 0, pdf.CurrentPage.Width.Inch, pdf.CurrentPage.Height.Inch);
                    pdf.CurrentRegion = new XRect(0, 0, pdf.CurrentPage.Width.Inch, pdf.CurrentPage.Height.Inch);
                    pdf.SetTabsEvenColumns(0, pdf.CurrentPage.Width.Inch, MaxColumns, .25);
                }
                pdf.GoToTopOfPage();
                pdf.AddNewPageDelegate = AddNewPage;
                int ColumnPosition = 0;
                if (pdf.CurrentMargin != null)
                {
                    XRect? lastRect = XRect.FromLTRB(0, .5, 2, 2.5);
                    foreach (var image in ImageList)
                    {
                        var t = await Write_Image(pdf, ColumnPosition, lastRect, image);
                        lastRect = t.Item1;
                        ColumnPosition = t.Item2 + 1;
                    }
                }

                // Save the document to memory stream.
                using (var memoryStream = new MemoryStream())
                {
                    pdf.Save(memoryStream);
                    byte[] pdfBytes = memoryStream.ToArray();
                    var fileName = "Report.pdf";
                    if (JSRuntime != null)
                        await JSRuntime.InvokeVoidAsync("saveAsFile", fileName, Convert.ToBase64String(pdfBytes));

                    //    FileInfo fi = new FileInfo("Report.pdf");
                    //if (fi.Exists == true) fi.Delete();
                    //pdf.Save(fi.FullName);
                }
            }
#else
            // Create a new PDF document.
            var document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";
            document.Info.Subject = "Just a simple Hello-World program.";

            // Create an empty page in this document.
            var page = document.AddPage();
            //page.Size = PageSize.Letter;

            // Get an XGraphics object for drawing on this page.
            var gfx = XGraphics.FromPdfPage(page);

            // Draw two lines with a red default pen.
            var width = page.Width.Point;
            var height = page.Height.Point;
            gfx.DrawLine(XPens.Red, 0, 0, width, height);
            gfx.DrawLine(XPens.Red, width, 0, 0, height);

            // Draw a circle with a red pen which is 1.5 point thick.
            var r = width / 5;
            gfx.DrawEllipse(new XPen(XColors.Red, 1.5), XBrushes.White, new XRect(width / 2 - r, height / 2 - r, 2 * r, 2 * r));


#if true
            // Create a font.
            var font = new XFont("Times New Roman", 20, XFontStyleEx.BoldItalic);

            // Draw the text.
            gfx.DrawString("Hello, PDFsharp!", font, XBrushes.Black,
                new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.Center);
#endif

            // Save the document...
            var filename = PdfFileUtility.GetTempPdfFullFileName("samples/HelloWorldSample");
            document.Save(filename);
            // ...and start a viewer.
            PdfFileUtility.ShowDocument(filename);
#endif
        }
#endif

#if false
        private double AddNewPage(PDF_Helper pdf)
        {
            pdf.AddPage(pdf.CurrentPage.Size, pdf.CurrentPage.Orientation);
            pdf.GoToTopOfPage();
            return pdf.CurrentPoint.Y;
        }

        private async Task<(XRect?, int)> Write_Image(PDF_Helper pdf, int colPos, XRect? rect, VehicleImage? image)
        {
            if (rect == null) return (null, 0);
            if (colPos < 0)
            {
                throw new ArgumentOutOfRangeException($"Column position is out of range: {colPos}");
            }

            double Top = rect.Value.Top;
            if (colPos >= MaxColumns)
            {
                colPos = 0;
                Top = rect.Value.Bottom + XUnit.FromInch(.25).Inch;
            }
            rect = pdf.GetColumnRegion(colPos, rect.Value.Size.Height);
            rect = new XRect(new XPoint(rect.Value.Left, Top), rect.Value.Size);
            Draw_Lines(pdf, rect);
            Write_Image_Info(pdf, rect, image);
            await Draw_Image(pdf, rect, image);
            return (rect, colPos);
        }

        private async Task Draw_Image(PDF_Helper pdf, XRect? rect, VehicleImage? image)
        {
            if ((image == null) || (image?.ImageID == null) || (image.ImageID == Guid.Empty)) return;
            if (rect == null) return;

            XRect r = XRect.FromLTRB(rect.Value.Left, rect.Value.Top + XUnit.FromInch(.5).Inch, rect.Value.Right, rect.Value.Bottom);
            r.Inflate(-0.125, -0.125);

            ImagesData? id = await imageDataService.GetImagesDataById(image.ImageID);
            //String fname = $"/Images/{image.ImageID}.jpg";
            //if (_client != null)
            //{
            //    Byte[] bs = await _client.GetByteArrayAsync(fname);
            //    if (bs != null)
            if ((id != null) && (id.ImageData != null))
            {
                using (MemoryStream ms = new MemoryStream(id.ImageData))
                {
                    //ms.Write(id.ImageData);
                    //ms.Position = 0;
                    using (XImage i = XImage.FromStream(ms))
                    {
                        pdf.DrawImage(i, r);
                    }
                }
            }
            //}
        }

        private void Write_Image_Info(PDF_Helper pdf, XRect? rect, VehicleImage image)
        {
            if (rect == null) return;
            XRect r = XRect.FromLTRB(rect.Value.Left, rect.Value.Top, rect.Value.Right, rect.Value.Top + XUnit.FromInch(.25).Inch);
            pdf.WriteLine(r, image.ImageName, pdf.Brush_Blue, XStringFormats.Center, pdf.CurrentFontBold);
            r = XRect.FromLTRB(r.Left, r.Bottom, r.Right, rect.Value.Top + XUnit.FromInch(.5).Inch);
            pdf.WriteLine(r, $"{image.ImageID}", pdf.Brush_Black, XStringFormats.Center, pdf.Tiny_M4_Font);
        }

        private void Draw_Lines(PDF_Helper pdf, XRect? rect)
        {
            if (rect == null) return;
            pdf.DrawBox(rect);
            Double y = rect.Value.Top + XUnit.FromInch(.5).Inch;
            pdf.DrawLine(new XPoint(rect.Value.Left, y), new XPoint(rect.Value.Right, y));
        }
#endif

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
