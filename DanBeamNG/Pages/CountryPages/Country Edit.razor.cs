using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.CountryPages
{
    public partial class Country_Edit : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? ID { get; set; }


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
            if (String.IsNullOrEmpty(ID) == false)
            {
                Guid? id = Guid.Parse(ID);

                if (countryDataService == null)
                {
                    StatusClass = "alert-danger";
                    Message = $"The countryDataService object is null";
                    Saved = false;
                    return;
                }
                country = await countryDataService.GetCountriesByID(id);
                Saved= true;
            }
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
            Saved = true;
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
                                Saved = false;
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
                    Countries? testcountry = await countryDataService.GetCountriesByName(country.Name);
                    if (testcountry != null)
                    {
                        StatusClass = "alert-danger";
                        Message = $"A Countries record already exists for: {country.Name}";
                    }
                    else
                    {
                        await countryDataService.UpdateCountries(country);
                        StatusClass = "alert-success";
                        Message = "Country data updated";
                        Saved = false;
                    }
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
