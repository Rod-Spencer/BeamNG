using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Pages.ClassificationPages;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.CountryPages
{
    public partial class Country_List : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        //[Parameter]
        //public String? ID { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public CountriesDataService_Interface? CountriesDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Properties

        //private Logger logger = Logger_Helper.GetCurrentLogger();

        #endregion Private Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public List<Countries>? CountryList { get; set; } = null;

        public Countries? country { get; set; } = new Countries();

        public int itemHeight { get; } = 50;

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
                CountryList = null;
                CountryList = (await CountriesDataService.GetAllCountries()).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                //logger.Error(Exception_Helper.FormatExceptionString(ex));
                StatusClass = "alert-danger"; Message1 = $"The following Exception error was thrown while trying to retrieve the image list: {ex.Message}";
                Message2 = "Please see runtime log for more details";
                Error = true;
            }
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods

        public async Task Delete_Country_Handler(Countries? cls)
        {
            if (cls == null) return;
            Error = false;
            try
            {
                if (CountriesDataService != null)
                {
                    await CountriesDataService.DeleteCountries(cls.ID);
                    var clsList = CountriesDataService.GetAllCountries();
                    if (clsList != null)
                    {
                        CountryList = clsList?.Result?.OrderBy(x => x.Name).ToList();
                    }

                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                //logger.Error(Exception_Helper.FormatExceptionString(ex));
                StatusClass = "alert-danger"; Message1 = $"The following Exception error was thrown while trying to delete the country: {ex.Message}";
                Message2 = "Please see runtime log for more details";
                Error = true;
            }
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
