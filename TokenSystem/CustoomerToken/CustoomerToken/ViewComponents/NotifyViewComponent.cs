using CustoomerToken.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustoomerToken.ViewComponents
{
    public class NotifyViewComponent : ViewComponent
    {
        private readonly NotifyServices _notifyServices;

        public NotifyViewComponent(NotifyServices notifyServices)
        {
            this._notifyServices = notifyServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_notifyServices.Notifications);
        }
    }
}
