using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;
using System;

namespace SpenSoft.DanBeamNG.Pages.Configuration_Pages
{
    public partial class Configuration_Edit : ComponentBase
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? ID { get; set; }

        [Parameter]
        public String? ImageID { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public ConfigurationDataService_Interface? ConfigurationDataService { get; set; }

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        [Inject]
        public VehicleImageDataService_Interface? imageDataService { get; set; }

        [Inject]
        public ClassificationsDataService_Interface? classificationsDataService { get; set; }

        [Inject]
        public BodyStyleDataService_Interface? bodyStyleDataService { get; set; }

        [Inject]
        public CountriesDataService_Interface? countriesDataService { get; set; }

        [Inject]
        public DriveTrainDataService_Interface? driveTrainDataService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties
        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties
        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods
        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public VConfiguration? Configuration { get; set; }

        public Vehicle? Vehicle { get; set; }

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Countries_List { get; set; }

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<DriveTrain>? DriveTrains_List { get; set; }

        public VehicleImage? vehicleImage { get; set; } = null;

        public Boolean isDisabled { get; set; } = true;

        public String? ImageName { get; set; }

        public String? ImageIDString { get; set; }

        ////////////////////////////////////////////////////////////////////
        // Store Screen State
        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved;
        // 
        ////////////////////////////////////////////////////////////////////



        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            if (String.IsNullOrEmpty(ID) == false)
            {
                int? id = int.Parse(ID);
                if (ConfigurationDataService != null)
                {
                    Configuration = (await ConfigurationDataService.GetConfigurationByID(id));
                    if (Configuration != null)
                    {
                        if (VehicleDataService != null) Vehicle = (await VehicleDataService.GetVehicleByID(Configuration.VehicleID));
                        if (Configuration.ImageID.HasValue == true)
                        {
                            if (imageDataService != null) vehicleImage = await imageDataService.GetVehicleImageById(Configuration.ImageID.Value);
                            if (vehicleImage != null)
                            {
                                ImageName = vehicleImage.ImageName;
                                ImageIDString = vehicleImage.ImageID.ToString();
                            }

                        }
                    }
                    if (bodyStyleDataService != null) BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).ToList();
                    if (classificationsDataService != null) Classifications_List = (await classificationsDataService.GetAllClassifications()).ToList();
                    if (countriesDataService != null) Countries_List = (await countriesDataService.GetAllCountries()).ToList();
                    if (driveTrainDataService != null) DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain()).ToList();
                }
            }
        }

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The Configuration data had validation errors.  Please correct and try again";
        }

        protected async Task ValidSumitHandler()
        {
            Saved = false;
            if (Configuration != null)
            {
                if (ConfigurationDataService != null)
                {
                    if (Configuration.ID == 0)
                    {
                        VConfiguration? addedConfiguration = await ConfigurationDataService.AddConfiguration(Configuration);
                        if (addedConfiguration != null)
                        {
                            if (imageDataService != null)
                            {
                                VehicleImage? vi = await imageDataService.GetVehicleImageByName(ImageName);
                                if (vi == null)
                                {
                                    vehicleImage = await imageDataService.AddVehicleImage(vehicleImage);
                                }
                            }
                            StatusClass = "alert-success";
                            Message = "New Configuration added";
                            Saved = true;
                        }
                        else
                        {
                            StatusClass = "alert-danger";
                            Message = "The new Configuration could not be dded";
                            Saved = false;
                        }
                    }
                    else
                    {
                        await ConfigurationDataService.UpdateConfiguration(Configuration);

                        if ((imageDataService != null) && (Configuration.ImageID.HasValue == true))
                        {
                            VehicleImage? vi = await imageDataService.GetVehicleImageById(Configuration.ImageID.Value);
                            if (vi == null)
                            {
                                vehicleImage = await imageDataService.AddVehicleImage(vehicleImage);
                                ImageIDString = vehicleImage.ImageID.ToString();
                            }
                            else
                            {
                                ImageIDString = vi.ImageID.ToString();
                            }
                        }

                        StatusClass = "alert-success";
                        Message = "Configuration data updated";
                        Saved = true;
                    }
                }
            }
        }



        private static List<T> GetCodes<T>()
        {
            List<T> codes = new List<T>();
            foreach (T t in Enum.GetValues(typeof(T))) { codes.Add(t); }
            return codes;
        }


        private async void LoadImage(InputFileChangeEventArgs e)
        {
            ImageName = e.File.Name;
            vehicleImage = await imageDataService.GetVehicleImageByName(ImageName);
            if (vehicleImage != null)
            {
                ImageName = vehicleImage.ImageName;
                ImageIDString = vehicleImage.ImageID.ToString();
                Configuration.ImageID = vehicleImage.ImageID;
                if (ConfigurationDataService != null)
                {
                    await ConfigurationDataService.UpdateConfiguration(Configuration);
                    NavigationManager?.NavigateTo($"EditConfiguration/{ID}/{Configuration.ImageID}");
                }

                return;
            }
            vehicleImage = new VehicleImage() { ImageID = Guid.NewGuid(), ImageName = this.ImageName };
            MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(ms);
            //vehicleImage.ImageData = ms.ToArray();
            await imageDataService.AddVehicleImage(vehicleImage);

            Configuration.ImageID = vehicleImage.ImageID;
            if (ConfigurationDataService != null)
            {
                await ConfigurationDataService.UpdateConfiguration(Configuration);
                NavigationManager?.NavigateTo($"EditConfiguration/{ID}/{Configuration.ImageID}");
            }
        }


#if false
        private async void NameChanged(String name)
        {
            if (vehicleImage == null)
            {
                isDisabled = true;
                return;
            }
            VehicleImage vi = await imageDataService.GetVehicleImageById(vehicleImage.ImageID);
            if (vi != null)
            {
                vi.ImageName = name;
                vehicleImage.ImageName = name;
                await imageDataService.UpdateVehicleImage(vehicleImage);
            }
            ImageName = name;

            if (String.IsNullOrEmpty(ImageName) == true) isDisabled = true;
            else if (vehicleImage == null) isDisabled = true;
            else isDisabled = false;
        }
#endif

        private async void ImageID_Changed(String imageID)
        {
            if (String.IsNullOrEmpty(imageID) == true) return;
            Guid guid = Guid.Empty;
            Guid.TryParse(imageID, out guid);
            if (guid == Guid.Empty) return;
            if (imageDataService != null)
            {
                vehicleImage = await imageDataService.GetVehicleImageById(guid);
                if (vehicleImage != null)
                {
                    ImageName = vehicleImage.ImageName;
                    ImageIDString = vehicleImage.ImageID.ToString();
                    Configuration.ImageID = guid;
                    if (ConfigurationDataService != null)
                    {
                        await ConfigurationDataService.UpdateConfiguration(Configuration);
                        NavigationManager?.NavigateTo($"EditConfiguration/{ID}/{Configuration.ImageID}");
                    }

                }
                else
                {
                    ImageName = null;
                    ImageIDString = Guid.Empty.ToString();
                }
            }
            else
            {
                ImageName = null;
                ImageIDString = Guid.Empty.ToString();
            }

        }
    }
}
