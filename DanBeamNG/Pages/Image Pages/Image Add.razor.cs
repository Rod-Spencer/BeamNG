
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Image_Pages
{
    public partial class Image_Add : ComponentBase
    {
        public VehicleImage? vehicleImage { get; set; } = null;


        [Inject]
        public VehicleImageDataService_Interface? imageDataService { get; set; }

        ////////////////////////////////////////////////////////////////////
        // Store Screen State
        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved;
        // 
        ////////////////////////////////////////////////////////////////////



        protected override async Task OnInitializedAsync()
        {
            vehicleImage = new VehicleImage();
            Saved = true;
        }

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The Vehicle data had validation errors.  Please correct and try again";
            Saved = false;
        }

        protected async Task ValidSumitHandler()
        {
            Saved = false;
            if (vehicleImage != null)
            {
                if (vehicleImage.ImageID == Guid.Empty)
                {
                    if (imageDataService != null)
                    {
                        VehicleImage? testVehicleImage = await imageDataService.GetVehicleImageByName(vehicleImage.ImageName);
                        if (testVehicleImage != null)
                        {
                            StatusClass = "alert-danger";
                            Message = $"A VehicleImage record already exists for: {vehicleImage.ImageName}";
                        }
                        else
                        {
                            vehicleImage.ImageID = Guid.NewGuid();
                            VehicleImage? addedVehicle = await imageDataService.AddVehicleImage(vehicleImage);
                            if (addedVehicle != null)
                            {
                                StatusClass = "alert-success";
                                Message = $"New Vehicle Image added ({vehicleImage.ImageName})";
                            }
                            else
                            {
                                StatusClass = "alert-danger";
                                Message = "The new Vehicle could not be added";
                            }
                        }
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = "The VehicleDataService is null";
                    }
                }
                else if (imageDataService != null)
                {
                    await imageDataService.UpdateVehicleImage(vehicleImage);
                    StatusClass = "alert-success";
                    Message = "Vehicle data updated";
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "The VehicleDataService is null";
                }
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "The Vehicle object is null";
            }
        }

        private Boolean isDisabled()
        {
            if (vehicleImage == null) return true;
            if (String.IsNullOrEmpty(vehicleImage.ImageName) == true) return true;
            return false;
        }

        private async Task UploadFile(InputFileChangeEventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(ms);
            //vehicleImage.ImageData = ms.ToArray();
            vehicleImage.ImageName = e.File.Name;
            vehicleImage.ImageID = Guid.Empty;
        }
    }
}
