using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class ConfigurationEditCard : ComponentBase
    {
        [Parameter]
        public VConfiguration? config { get; set; }

        [Parameter]
        public List<KeyValuePair<Guid, String>>? Classifications_List { get; set; }

        [Parameter]
        public List<KeyValuePair<Guid, String>>? Countries_List { get; set; }

        [Parameter]
        public List<KeyValuePair<Guid, String>>? BodyStyles_List { get; set; }

        [Parameter]
        public List<KeyValuePair<Guid, String>>? DriveTrains_List { get; set; }

        [Parameter]
        public List<VehicleImage>? Image_List { get; set; } = null;

        [Parameter]
        public String? ImageName { get; set; }

        [Parameter]
        public String? ImageID { get; set; }

        [Parameter]
        public EventCallback<String?> ImageName_Delegate { get; set; }

        [Parameter]
        public EventCallback<IBrowserFile?> loadCallback_Delegate { get; set; }


        public Guid? ClassificationID
        {
            get
            {
                return config?.ClassificationID;
            }
            set
            {
                config.ClassificationID = value;
            }
        }
        public String? ImageFile { get; set; }

        private void ImageID_Changed(String imageID)
        {
            ImageID = imageID;
        }

        private async void ImageName_Changed(String imagename)
        {
            await ImageName_Delegate.InvokeAsync(imagename);
        }

        private async void LoadImage(InputFileChangeEventArgs e)
        {
            //ImageName = e.File.Name;
            //if (config != null) config.ImageID = Guid.NewGuid();
            await loadCallback_Delegate.InvokeAsync(e.File);
        }
    }
}