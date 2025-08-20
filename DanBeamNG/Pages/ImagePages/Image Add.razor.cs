using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.ImagePages
{
    public partial class Image_Add : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters
        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public VehicleImageDataService_Interface? imageDataService { get; set; }

        [Inject]
        public ImagesDataDataService_Interface? imagesDataDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Properties

        //private VehicleImage? vehicleImage { get; set; } = new VehicleImage();

        #endregion Private Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public String? imageName { get; set; }
        public Guid? imageID { get; set; }

        public Boolean loading { get; set; } = false;
        public String fname { get; set; } = String.Empty;

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected List<String> Messages = new List<String>();
        protected List<String> ErrorMessages = new List<String>();
        protected String StatusClass = String.Empty;
        protected Boolean Saved;
        private String? ImageFiles { get; set; }

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            Saved = true;
            loading = false;
            fname = String.Empty;
            Messages = new List<String>();
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Messages.Add("The vehicle data had validation errors.  Please correct and try again");
            Saved = true;
            loading = false;
            fname = String.Empty;
        }

        protected async Task ValidSumitHandler()
        {
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        //private Boolean isDisabled()
        //{
        //    if (vehicleImages == null) return true;
        //    if (String.IsNullOrEmpty(vehicleImages.ImageName) == true) return true;
        //    return false;
        //}

        private async Task UploadFile(InputFileChangeEventArgs e)
        {
            Messages.Clear();
            ErrorMessages.Clear();
            Random r = new Random((int)(int.MaxValue & DateTime.Now.Ticks));
            foreach (var file in e.GetMultipleFiles())
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    await file.OpenReadStream().CopyToAsync(ms);

                    Guid id = Guid.NewGuid();

                    var vi = new VehicleImage() { ImageName = Path.GetFileNameWithoutExtension(file.Name), ImageID = id, ImageEntered = DateTime.Now };
                    if (imageDataService != null) await imageDataService.AddVehicleImage(vi);


                    var data = new ImagesData() { ImageData = ms.ToArray(), ImageID = id };
                    if (imagesDataDataService != null) imagesDataDataService?.AddImagesData(data);
                    Messages.Add($"Added Image: {file.Name}");
                }
                catch
                {
                    ErrorMessages.Add($"Image: {file.Name} failed");
                }
            }
            Saved = true;
        }

        private void ClearMessages()
        {
            Saved = false;
            Messages.Clear();
            ErrorMessages.Clear();
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
