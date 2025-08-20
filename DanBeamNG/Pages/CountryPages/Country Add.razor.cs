using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.CountryPages
{
    public partial class Country_Add : ComponentBase
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
        public CountriesDataService_Interface? countryDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public Countries? country { get; set; } = null;

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            country = new Countries();
            Saved = true;
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The Country data had validation errors.  Please correct and try again";
            Saved = false;
        }

        protected async Task ValidSumitHandler()
        {
            Saved = false;
            if (country != null)
            {
                if (country.ID == Guid.Empty)
                {
                    if (countryDataService != null)
                    {
                        Countries? testcountry = await countryDataService.GetCountriesByName(country.Name);
                        if (testcountry != null)
                        {
                            StatusClass = "alert-danger";
                            Message = $"A Countries record already exists for: {country.Name}";
                        }
                        else
                        {
                            country.ID = Guid.NewGuid();
                            Countries? addedVehicle = await countryDataService.AddCountries(country);
                            if (addedVehicle != null)
                            {
                                StatusClass = "alert-success";
                                Message = $"New Country: {country.Name} added";
                            }
                            else
                            {
                                StatusClass = "alert-danger";
                                Message = "The new Country could not be added";
                            }
                        }
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = "The countryDataService is null";
                    }
                }
                else if (countryDataService != null)
                {
                    await countryDataService.UpdateCountries(country);
                    StatusClass = "alert-success";
                    Message = "Country data updated";
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "The countryDataService is null";
                }
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "The Country object is null";
            }
        }

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

    }
}
