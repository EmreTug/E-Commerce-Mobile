using System;
using Xamarin.Forms;

namespace exampleproject1.Helpers
{
    public interface IHasCollectionView
    {
       CollectionView CollectionView { get; }
    }

    public interface IHasCollectionViewModel
    {
        IHasCollectionView View { get; set; }
    }
}
