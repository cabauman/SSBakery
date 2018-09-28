using System;
using System.Linq;
using System.Reactive.Disposables;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using Square.Connect.Model;
using SSBakery;
using SSBakery.Models;
using SSBakery.Services;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class AlbumCellViewModel : ViewModelBase, IAlbumCellViewModel
    {
        private FacebookAlbum _model;
        private string _imageUrl;

        public AlbumCellViewModel(FacebookAlbum model, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _model = model;
        }

        public FacebookAlbum FacebookAlbum
        {
            get { return _model; }
        }

        public string Name
        {
            get { return _model.Name; }
        }

        public int Count
        {
            get { return _model.Count; }
        }

        public string ImageUrl
        {
            get
            {
                if(_imageUrl == null)
                {
                    int minWidthIdx = 0;
                    for(int i = 1; i < _model.CoverPhoto.Images.Count; ++i)
                    {
                        if(_model.CoverPhoto.Images[i].Width < _model.CoverPhoto.Images[minWidthIdx].Width)
                        {
                            minWidthIdx = i;
                        }
                    }

                    _imageUrl = _model.CoverPhoto.Images[minWidthIdx].Source;
                }

                return _imageUrl;
            }
        }
    }
}
