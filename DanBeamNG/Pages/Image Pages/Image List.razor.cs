using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;
using Microsoft.AspNetCore.Components;

namespace SpenSoft.DanBeamNG.Pages.Image_Pages
{
    public partial class Image_List : ComponentBase
    {
        [Parameter]
        public String? ID { get; set; }

        public IEnumerable<VehicleImage>? ImageList { get; set; }

        public VehicleImage? vehicleImage { get; set; } = new VehicleImage();

        [Inject]
        public VehicleImageDataService_Interface? VehicleImageDataService { get; set; }

        ////////////////////////////////////////////////////////////////////
        // Store Screen State
        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved;
        // 
        ////////////////////////////////////////////////////////////////////


        protected override async Task OnInitializedAsync()
        {
            ImageList = (await VehicleImageDataService.GetAllVehicleImage()).ToList();
        }

        protected async Task DeleteVehicle()
        {
            if (VehicleImageDataService != null) await VehicleImageDataService.DeleteVehicleImage(vehicleImage.ImageID);

            StatusClass = "alert-success";
            Message = "Deleted successfully";

            Saved = true;
        }
    }
}
