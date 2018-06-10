using ReactiveUI;
using SSBakery.Models;
using SSBakery.Services;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class PhotoCellViewModel : ViewModelBase
    {
        private FacebookPhoto _model;
        private string _imageUrl;

        public PhotoCellViewModel(FacebookPhoto model, IScreen hostScreen = null)
            : base(hostScreen)
        {
            _model = model;
        }

        public string ImageUrl
        {
            get
            {
                if(_imageUrl == null)
                {
                    int minWidthIdx = 0;
                    for(int i = 1; i < _model.Images.Count; ++i)
                    {
                        if(_model.Images[i].Width < _model.Images[minWidthIdx].Width)
                        {
                            minWidthIdx = i;
                        }
                    }

                    _imageUrl = _model.Images[minWidthIdx].Source;
                }

                return _imageUrl;
            }
        }
    }
}
