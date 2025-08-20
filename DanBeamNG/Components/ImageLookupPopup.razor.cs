using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class ImageLookupPopup : ComponentBase
    {
        private static Logger logger = Logger_Helper.GetCurrentLogger();

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public VConfiguration? vConfiguration { get; set; }

        [Parameter]
        public List<VehicleImage>? images { get; set; }

        [Parameter]
        public Dictionary<Guid, Byte[]>? imageData { get; set; } = null;

        [Parameter]
        public EventCallback<VConfiguration?> imageSelected { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public ConfigurationDataService_Interface? configurationDataService { get; set; }

        [Inject]
        public NavigationManager? navigationManager { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Properties

        private VConfiguration? _vConfiguration;
        private Boolean _SortAlpha = false;

        private List<VehicleImage>? allImages
        {
            get;
            set;
        }

        #endregion Private Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public Guid? selectedImageID { get; set; }

        public String? Filter { get; set; }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties
        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override void OnParametersSet()
        {
            _vConfiguration = vConfiguration;
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods

        public void Assign()
        {
            try
            {
                if (configurationDataService == null)
                {
                    logger.Debug("configurationDataService is NULL", nameof(Assign));
                    return;
                }

                if (vConfiguration == null)
                {
                    logger.Debug("vConfiguration is NULL", nameof(Assign));
                    return;
                }

                if (navigationManager == null)
                {
                    logger.Debug("navigationManager is NULL", nameof(Assign));
                    return;
                }

                vConfiguration.ImageID = selectedImageID;
                String msg = $"vConfiguration: {JsonConvert.SerializeObject(vConfiguration)}";
                logger.Debug(msg);

                logger.Debug($"Updating vConfiguration");
                configurationDataService.UpdateConfiguration(vConfiguration);
                logger.Debug($"Updated vConfiguration");

                String url = $"EditVehicle/{vConfiguration.VehicleID}";
                logger.Debug($"Navigating to: {url}");
                navigationManager.NavigateTo(url, true);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, nameof(Assign));
            }
            finally
            {
                _vConfiguration = null;
                StateHasChanged();
            }
        }


        public async Task Clear()
        {
            if (vConfiguration != null)
            {
                vConfiguration.ImageID = null;
                await imageSelected.InvokeAsync(vConfiguration);
                StateHasChanged();
                return;
            }


#if false
            try
            {
                if (clientLogger == null)
                {
                    Console.WriteLine("clientlogger is NULL");
                    return;
                }

                if (configurationDataService == null)
                {
                    logger.Debug("configurationDataService is NULL");
                    return;
                }

                if (vConfiguration == null)
                {
                    logger.Debug("vConfiguration is NULL");
                    return;
                }

                if (navigationManager == null)
                {
                    logger.Debug("navigationManager is NULL");
                    return;
                }


                vConfiguration.ImageID = null;
                String msg = $"vConfiguration: {JsonConvert.SerializeObject(vConfiguration)}";
                logger.Debug(msg);


                logger.Debug($"Updating vConfiguration");
                configurationDataService.UpdateConfiguration(vConfiguration);
                logger.Debug($"Updated vConfiguration");

                String url = $"EditVehicle/{vConfiguration.VehicleID}";
                logger.Debug($"Navigating to: {url}");
                navigationManager.NavigateTo(url, true);
            }
            catch (Exception e)
            {
                logger.Debug(e.Message, "Error");
            }
            finally
            {
                _vConfiguration = null;
                StateHasChanged();
            }
#endif
        }

        public void Cancel()
        {
            _vConfiguration = null;
        }

        public void SortAlphaChanged(Boolean ischecked)
        {
            if (images != null)
            {
                if (allImages == null) allImages = new List<VehicleImage>(images);
                if (ischecked)
                {
                    images = allImages.OrderBy(x => x.ImageName).ToList();
                }
                else
                {
                    images = allImages
                            .OrderByDescending(x => new DateTime(x.ImageEntered.Year, x.ImageEntered.Month, x.ImageEntered.Day, x.ImageEntered.Hour, x.ImageEntered.Minute, 0))
                            .ThenBy(x => x.ImageName)
                            .ToList();
                }
            }
            Filter_Changed(Filter);
        }

        public void AlphaSort()
        {
            if (allImages == null) allImages = new List<VehicleImage>(images);
            images = allImages.OrderBy(x => x.ImageName).ToList();
            Filter_Changed(Filter);
        }

        public void ReverseSort()
        {
            if (allImages == null) allImages = new List<VehicleImage>(images);
            images = allImages
                    .OrderByDescending(x => new DateTime(x.ImageEntered.Year, x.ImageEntered.Month, x.ImageEntered.Day, x.ImageEntered.Hour, x.ImageEntered.Minute, 0))
                    .ThenBy(x => x.ImageName)
                    .ToList();
            Filter_Changed(Filter);
        }

        public void Filter_Changed(String? filter)
        {
            if (String.IsNullOrEmpty(filter) == false)
            {
                if (images != null)
                {
                    if (allImages == null) allImages = new List<VehicleImage>(images);
                    images = images.Where(x => x.ImageName.ToUpper().Contains(filter.ToUpper())).ToList();
                }
            }
            else if ((allImages != null) && (images != null))
            {
                if (images.Count != allImages.Count)
                {
                    images = new List<VehicleImage>(allImages);
                }
            }
            Filter = filter;
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        //private String? GetImage(Guid? imageID) => $"/Images/{@imageID}.jpg";

        private async void imageAssignCallBack(Guid? imageID)
        {
            if (vConfiguration != null)
            {
                vConfiguration.ImageID = imageID;
                await imageSelected.InvokeAsync(vConfiguration);
            }
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
