using Microsoft.JSInterop;

namespace SpenSoft.DanBeamNG.Services
{
    public class Browser_Service
    {
        private readonly IJSRuntime _js;

        public Browser_Service(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<Browser_Dimension> GetDimensions()
        {
            return await _js.InvokeAsync<Browser_Dimension>("getDimensions");
        }

    }
}


