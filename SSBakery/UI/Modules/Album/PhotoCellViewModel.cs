using GameCtor.RxNavigation;
using SSBakery.Models;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class PhotoCellViewModel : ViewModelBase
    {
        private FacebookPhoto _model;
        private string _imageUrl;
        private string _imageUrlMaxSize;

        public PhotoCellViewModel(FacebookPhoto model, IViewStackService viewStackService = null)
            : base(viewStackService)
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
                    int maxWidthIdx = 0;
                    for (int i = 1; i < _model.Images.Count; ++i)
                    {
                        if(_model.Images[i].Width < _model.Images[minWidthIdx].Width)
                        {
                            minWidthIdx = i;
                        }
                        else if(_model.Images[i].Width > _model.Images[maxWidthIdx].Width)
                        {
                            maxWidthIdx = i;
                        }
                    }

                    _imageUrl = _model.Images[minWidthIdx].Source;
                    _imageUrlMaxSize = _model.Images[maxWidthIdx].Source;
                }

                return _imageUrl;
            }
        }

        public string ImageUrlMaxSize => _imageUrlMaxSize;
    }
}
